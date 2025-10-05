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
using SwiftXP.SPT.ShowMeTheMoney.Models;
using SwiftXP.SPT.ShowMeTheMoney.QuickSell.Enums;
using SwiftXP.SPT.ShowMeTheMoney.QuickSell.Models;
using SwiftXP.SPT.ShowMeTheMoney.Services;

namespace SwiftXP.SPT.ShowMeTheMoney.QuickSell.Services;

public class BrokerService
{
    private static readonly Lazy<BrokerService> instance = new(() => new BrokerService());

    private BrokerService() { }

    public void Trade(BrokerTradeEnum trade, params Item[] items)
    {
        bool soldAnyThing = false;

        try
        {
            switch (trade)
            {
                case BrokerTradeEnum.Trader:
                    soldAnyThing = TradeToTraders(items);

                    break;

                case BrokerTradeEnum.Flea:
                    soldAnyThing = TradeOnFlea(items);

                    break;

                case BrokerTradeEnum.Best:
                    SplitItemsIntoBestTrade(items, out List<Item> traderItems, out List<Item> fleaItems, out List<Item> unsellableItems);

                    if (fleaItems.Any())
                        soldAnyThing |= TradeOnFlea([.. fleaItems]);

                    if (traderItems.Any())
                        soldAnyThing |= TradeToTraders([.. traderItems]);

                    break;

                default:
                    break;
            }
        }
        finally
        {
            if (soldAnyThing)
            {
                Singleton<GUISounds>.Instance.PlayUISound(EUISoundType.TradeOperationComplete);
            }
        }
    }

    private Dictionary<string, List<TradeItem>> GroupItemsByTraders(Item[] items, out List<Item> unsellableItems)
    {
        Dictionary<string, List<TradeItem>> result = [];
        unsellableItems = [];

        foreach (Item item in items)
        {
            TradeItem tradeItem = new(item);
            bool hasTraderPrice = TraderPriceService.Instance.GetBestTraderPrice(tradeItem);
            if (hasTraderPrice)
            {
                if (result.ContainsKey(tradeItem.TraderPrice!.TraderId!))
                {
                    result[tradeItem.TraderPrice!.TraderId!].Add(tradeItem);
                }
                else
                {
                    result.Add(tradeItem.TraderPrice!.TraderId!, [tradeItem]);
                }
            }
            else
            {
                unsellableItems.Add(item);
            }
        }

        return result;
    }

    private List<BrokerTrade> GroupItemsByTypeForFlea(Item[] items, out List<Item> unsellableItems)
    {
        List<BrokerTrade> result = [];
        unsellableItems = [];

        foreach (Item item in items)
        {
            if (item.CanSellOnRagfair)
            {
                TradeItem tradeItem = new(item);
                bool hasFleaPrice = FleaPriceService.Instance.GetFleaPrice(tradeItem, false);
                if (hasFleaPrice)
                {
                    if (result.Where(x => x.ItemTemplateId == tradeItem.Item.TemplateId).Any())
                    {
                        result.First(x => x.ItemTemplateId == tradeItem.Item.TemplateId).Items.Add(tradeItem);
                    }
                    else
                    {
                        result.Add(new(tradeItem.Item.TemplateId, tradeItem.FleaPrice!.SingleObjectPrice, tradeItem));
                    }
                }
                else
                {
                    unsellableItems.Add(item);
                }
            }
            else
            {
                unsellableItems.Add(item);
            }
        }

        return result;
    }

    private void SplitItemsIntoBestTrade(Item[] items, out List<Item> traderItems, out List<Item> fleaItems, out List<Item> unsellableItems)
    {
        GetFleaSlotsForUser(out int currentOffersCount, out int maxOffersCount);

        traderItems = [];
        fleaItems = [];
        unsellableItems = [];

        List<(TradeItem, bool, bool)> tradeItems = [];
        foreach (Item item in items)
        {
            TradeItem tradeItem = new(item);
            bool hasTraderPrice = TraderPriceService.Instance.GetBestTraderPrice(tradeItem);
            bool hasFleaPrice = FleaPriceService.Instance.GetFleaPrice(tradeItem, true);

            tradeItems.Add(new(tradeItem, hasTraderPrice, hasFleaPrice));
        }

        foreach ((TradeItem tradeItem, bool hasTraderPrice, bool hasFleaPrice) in tradeItems)
        {
            bool tradeToTrader = false;
            bool tradeToFlea = SptSession.Session.RagFair.Available && (Plugin.Configuration!.AllowAnyNumberOfFleaOffers.IsEnabled() || currentOffersCount < maxOffersCount);

            if ((hasTraderPrice && hasFleaPrice && tradeItem.TraderPrice!.GetComparePriceInRouble() > tradeItem.FleaPrice!.GetComparePriceInRouble())
                || (hasTraderPrice && !hasFleaPrice))
            {
                tradeToTrader = true;
                tradeToFlea = false;
            }

            if (!tradeToTrader && !tradeToFlea && Plugin.Configuration!.SellToTraderIfFleaSlotsFull.IsEnabled() && hasTraderPrice)
            {
                tradeToTrader = true;
            }

            if (tradeToFlea)
            {
                fleaItems.Add(tradeItem.Item);
                ++currentOffersCount;
            }
            else if (tradeToTrader)
            {
                traderItems.Add(tradeItem.Item);
            }
            else
            {
                unsellableItems.Add(tradeItem.Item);
            }
        }
    }

    private bool TradeToTraders(Item[] items)
    {
        Dictionary<string, List<TradeItem>> itemsByTrader = GroupItemsByTraders(items, out List<Item> itemsNotSellableAtTraders);
        bool soldAnyThing = itemsByTrader.Any();

        if (itemsNotSellableAtTraders.Any())
            NotificationsService.Instance.SendLongAlert("Trader can't buy this item".Localized(null));

        foreach (KeyValuePair<string, List<TradeItem>> itemByTrader in itemsByTrader)
        {
            SellItemsToTrader(itemByTrader.Key, [.. itemByTrader.Value]);
        }

        return soldAnyThing;
    }

    private bool TradeOnFlea(Item[] items)
    {
        bool soldAnyThing = false;

        if (SptSession.Session.RagFair.Available)
        {
            GetFleaSlotsForUser(out int currentOffersCount, out int maxOffersCount);

            List<BrokerTrade> brokerFleaTrades = GroupItemsByTypeForFlea(items, out List<Item> itemsNotSellableAtFlea);
            soldAnyThing = brokerFleaTrades.Any();

            if (itemsNotSellableAtFlea.Any())
                NotificationsService.Instance.SendLongAlert("ragfair/This item cannot be placed at ragfair".Localized(null));

            foreach (BrokerTrade brokerTrade in brokerFleaTrades.OrderByDescending(x => x.Price))
            {
                if (!Plugin.Configuration!.AllowAnyNumberOfFleaOffers.IsEnabled() && currentOffersCount >= maxOffersCount)
                {
                    NotificationsService.Instance.SendLongAlert("ragfair/Reached maximum amount of offers".Localized(null));
                    break;
                }

                SellItemsOnFlea(brokerTrade.Price, [.. brokerTrade.Items]);
                ++currentOffersCount;
            }
        }
        else
        {
            NotificationsService.Instance.SendLongAlert("ragfair/Unlocked at character LVL {0}".Localized(null).Replace("{0}",
                RagFairClass.Settings.minUserLevel.ToString()));
        }

        return soldAnyThing;
    }

    private void SellItemsToTrader(string traderId, TradeItem[] tradeItems)
    {
        TraderClass traderClass = SptSession.Session.GetTrader(traderId);

        int totalPrice = tradeItems.Sum(x => x.TraderPrice!.TotalPrice ?? traderClass.GetUserItemPrice(x.Item)!.Value.Amount);
        TradingItemReference[] tradingItemReferences = [.. tradeItems.Select(x => new TradingItemReference { Item = x.Item, Count = x.Item.StackObjectsCount })];

        traderClass.iTraderInteractions.ConfirmSell(
            traderId,
            tradingItemReferences,
            totalPrice,
            new Callback(result => { })
        );
    }

    private void SellItemsOnFlea(int price, TradeItem[] tradeItems)
    {
        string[] itemIds = [.. tradeItems.Select(x => x.Item.Id)];

        GClass2102[] requirements = [new()
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