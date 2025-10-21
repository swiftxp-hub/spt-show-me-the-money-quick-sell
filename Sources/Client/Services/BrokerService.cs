using System;
using System.Collections.Generic;
using System.Linq;
using Comfort.Common;
using EFT.InventoryLogic;
using EFT.Trading;
using EFT.UI;
using SwiftXP.SPT.Common.ConfigurationManager;
using SwiftXP.SPT.Common.Constants;
using SwiftXP.SPT.Common.Notifications;
using SwiftXP.SPT.Common.Sessions;
using SwiftXP.SPT.ShowMeTheMoney.Client.Models;
using SwiftXP.SPT.ShowMeTheMoney.Client.Services;
using SwiftXP.SPT.ShowMeTheMoney.QuickSell.Client.Enums;
using SwiftXP.SPT.ShowMeTheMoney.QuickSell.Client.Models;

namespace SwiftXP.SPT.ShowMeTheMoney.QuickSell.Client.Services;

public class BrokerService
{
    private static readonly Lazy<BrokerService> instance = new(() => new BrokerService());

    private BrokerService() { }

    public void Trade(BrokerTradeTypeEnum brokerTradeType, params Item[] items)
    {
        bool soldAnyThing = false;

        try
        {
            if (!SptSession.Session.RagFair.Available && (brokerTradeType == BrokerTradeTypeEnum.Flea || brokerTradeType == BrokerTradeTypeEnum.Best))
                NotificationsService.Instance.SendLongAlert("ragfair/Unlocked at character LVL {0}".Localized(null).Replace("{0}",
                    RagFairClass.Settings.minUserLevel.ToString()));

            List<TradeItem> tradeItems = GetTradeItems(brokerTradeType, items);

            List<BrokerFleaTrade> brokerFleaTrades = GetBrokerFleaTrades(tradeItems);
            List<BrokerTraderTrade> brokerTraderTrades = GetBrokerTraderTrades(tradeItems);

            if (tradeItems.Any())
            {
                switch (brokerTradeType)
                {
                    case BrokerTradeTypeEnum.Trader:
                        NotificationsService.Instance.SendLongAlert("Trader can't buy this item".Localized(null));
                        break;

                    case BrokerTradeTypeEnum.Flea:
                        NotificationsService.Instance.SendLongAlert("ragfair/This item cannot be placed at ragfair".Localized(null));
                        break;

                    case BrokerTradeTypeEnum.Best:
                        NotificationsService.Instance.SendLongAlert("Not all items could be sold.");
                        break;
                }
            }

            foreach (BrokerFleaTrade brokerFleaTrade in brokerFleaTrades)
            {
                SellItemsOnFlea(brokerFleaTrade.Price, [.. brokerFleaTrade.TradeItems]);
            }

            foreach (BrokerTraderTrade brokerTraderTrade in brokerTraderTrades)
            {
                SellItemsToTrader(brokerTraderTrade.TraderId, [.. brokerTraderTrade.TradeItems]);
            }

            soldAnyThing = brokerFleaTrades.Any() || brokerTraderTrades.Any();
        }
        finally
        {
            if (soldAnyThing)
            {
                Singleton<GUISounds>.Instance.PlayUISound(EUISoundType.TradeOperationComplete);
            }
        }
    }

    private List<TradeItem> GetTradeItems(BrokerTradeTypeEnum brokerTradeType, Item[] items)
    {
        List<TradeItem> tradeItems = [];

        foreach (Item item in items)
        {
            TradeItem tradeItem = new(item);

            if (ShowMeTheMoney.Client.Plugin.Configuration!.EnableTraderPrices.IsEnabled() && (brokerTradeType == BrokerTradeTypeEnum.Trader || brokerTradeType == BrokerTradeTypeEnum.Best))
                TraderPriceService.Instance.GetBestTraderPrice(tradeItem);

            if (ShowMeTheMoney.Client.Plugin.Configuration!.EnableFleaPrices.IsEnabled() && SptSession.Session.RagFair.Available && (brokerTradeType == BrokerTradeTypeEnum.Flea || brokerTradeType == BrokerTradeTypeEnum.Best))
                FleaPriceService.Instance.GetFleaPrice(tradeItem, true);

            tradeItems.Add(tradeItem);
        }

        return tradeItems;
    }

    private List<BrokerFleaTrade> GetBrokerFleaTrades(List<TradeItem> tradeItems)
    {
        List<BrokerFleaTrade> result = [];

        GetFleaSlotsForUser(out int currentOffersCount, out int maxOffersCount);

        foreach (TradeItem tradeItem in tradeItems.OrderByDescending(x => x.FleaPrice?.SingleObjectPrice ?? int.MinValue))
        {
            if (tradeItem.Item.CanSellOnRagfair && tradeItem.FleaPrice != null
                && (tradeItem.TraderPrice is null || tradeItem.FleaPrice.GetComparePriceInRouble() > tradeItem.TraderPrice.GetComparePriceInRouble()))
            {
                if (result.Any(x => x.ItemTemplateId == tradeItem.Item.TemplateId))
                {
                    result.First(x => x.ItemTemplateId == tradeItem.Item.TemplateId).TradeItems.Add(tradeItem);
                }
                else if (Plugin.Configuration!.AllowAnyNumberOfFleaOffers.IsEnabled() || currentOffersCount < maxOffersCount)
                {
                    result.Add(new(tradeItem.Item.TemplateId, tradeItem.FleaPrice!.SingleObjectPrice, tradeItem));
                    ++currentOffersCount;
                }
            }
        }

        foreach (TradeItem tradeItem in result.SelectMany(x => x.TradeItems))
        {
            tradeItems.Remove(tradeItem);
        }

        return result;
    }

    private List<BrokerTraderTrade> GetBrokerTraderTrades(List<TradeItem> tradeItems)
    {
        List<BrokerTraderTrade> result = [];

        foreach (TradeItem tradeItem in tradeItems.OrderByDescending(x => x.FleaPrice?.SingleObjectPrice ?? int.MinValue))
        {
            if (tradeItem.TraderPrice != null
                && (tradeItem.FleaPrice is null
                    || tradeItem.TraderPrice.GetComparePriceInRouble() > tradeItem.FleaPrice.GetComparePriceInRouble()
                    || Plugin.Configuration!.SellToTraderIfFleaSlotsFull.IsEnabled()))
            {
                if (result.Any(x => x.TraderId == tradeItem.TraderPrice.TraderId))
                {
                    result.First(x => x.TraderId == tradeItem.TraderPrice.TraderId).TradeItems.Add(tradeItem);
                }
                else
                {
                    result.Add(new(tradeItem.TraderPrice!.TraderId!, tradeItem));
                }
            }
        }

        foreach (TradeItem tradeItem in result.SelectMany(x => x.TradeItems))
        {
            tradeItems.Remove(tradeItem);
        }

        return result;
    }

    private void SellItemsToTrader(string traderId, TradeItem[] tradeItems)
    {
        TraderClass traderClass = SptSession.Session.GetTrader(traderId);

        int totalPrice = tradeItems.Sum(x => x.TraderPrice!.TotalPrice ?? traderClass.GetUserItemPrice(x.Item)!.Value.Amount);
        TradingItemReference[] tradingItemReferences = [.. tradeItems.Select(x => new TradingItemReference { Item = x.Item, Count = x.Item.StackObjectsCount })];

        traderClass.ITraderInteractions.ConfirmSell(
            traderId,
            tradingItemReferences,
            totalPrice,
            new Callback(result => { })
        );
    }

    private void SellItemsOnFlea(int price, TradeItem[] tradeItems)
    {
        string[] itemIds = [.. tradeItems.Select(x => x.Item.Id)];

        GClass2335[] requirements = [new()
        {
            count = price,
            _tpl = SptConstants.CurrencyIds.Roubles
        }];

        SptSession.Session.RagFair.AddOffer(
            false,
            itemIds,
            requirements,
            () => { }
        );
    }

    private void GetFleaSlotsForUser(out int currentOffersCount, out int maxOffersCount)
    {
        currentOffersCount = SptSession.Session.RagFair.MyOffersCount;
        maxOffersCount = SptSession.Session.RagFair.GetMaxOffersCount(SptSession.Session.RagFair.MyRating);
    }

    public static BrokerService Instance => instance.Value;
}