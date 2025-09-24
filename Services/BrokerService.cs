using System;
using System.Collections.Generic;
using System.Linq;
using Comfort.Common;
using EFT.InventoryLogic;
using EFT.Trading;
using EFT.UI;
using SwiftXP.SPT.Common.Constants;
using SwiftXP.SPT.Common.Notifications;
using SwiftXP.SPT.Common.Sessions;
using SwiftXP.SPT.ShowMeTheMoney.Models;
using SwiftXP.SPT.ShowMeTheMoney.QuickSell.Enums;
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
                    Dictionary<string, List<TradeItem>> itemsByTrader = GroupItemsByTraders(items, out List<Item> itemsNotSellableAtTraders);
                    soldAnyThing = itemsByTrader.Any();

                    if (itemsNotSellableAtTraders.Any())
                        NotificationsService.Instance.SendLongAlert("Trader can't buy this item".Localized(null));

                    foreach (KeyValuePair<string, List<TradeItem>> itemByTrader in itemsByTrader)
                    {
                        SellItemsToTrader(itemByTrader.Key, [.. itemByTrader.Value]);
                    }

                    break;

                case BrokerTradeEnum.Flea:
                    if (SptSession.Session.RagFair.Available)
                    {
                        GetFleaSlotsForUser(out int currentOffersCount, out int maxOffersCount);

                        Dictionary<string, List<TradeItem>> itemsByGroup = GroupItemsByType(items, out List<Item> itemsNotSellableAtFlea);
                        soldAnyThing = itemsByGroup.Any();

                        if (itemsNotSellableAtFlea.Any())
                            NotificationsService.Instance.SendLongAlert("ragfair/This item cannot be placed at ragfair".Localized(null));

                        foreach (KeyValuePair<string, List<TradeItem>> itemByGroup in itemsByGroup)
                        {
                            if (currentOffersCount >= maxOffersCount)
                            {
                                NotificationsService.Instance.SendLongAlert("ragfair/Reached maximum amount of offers".Localized(null));
                                break;
                            }

                            SellItemsOnFlea([.. itemByGroup.Value]);
                            ++currentOffersCount;
                        }
                    }
                    else
                    {
                        NotificationsService.Instance.SendLongAlert("ragfair/Unlocked at character LVL {0}".Localized(null).Replace("{0}",
                            RagFairClass.Settings.minUserLevel.ToString()));
                    }

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

    private Dictionary<string, List<TradeItem>> GroupItemsByTraders(Item[] items, out List<Item> filteredItems)
    {
        Dictionary<string, List<TradeItem>> result = new();
        filteredItems = new();

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
                filteredItems.Add(item);
            }
        }

        return result;
    }

    private Dictionary<string, List<TradeItem>> GroupItemsByType(Item[] items, out List<Item> filteredItems)
    {
        Dictionary<string, List<TradeItem>> result = new();
        filteredItems = new();

        foreach (Item item in items)
        {
            if (item.CanSellOnRagfair)
            {
                TradeItem tradeItem = new(item);
                bool hasFleaPrice = FleaPriceService.Instance.GetFleaPrice(tradeItem, false);
                if (hasFleaPrice)
                {
                    if (result.ContainsKey(tradeItem.Item.TemplateId))
                    {
                        result[tradeItem.Item.TemplateId].Add(tradeItem);
                    }
                    else
                    {
                        result.Add(tradeItem.Item.TemplateId, [tradeItem]);
                    }
                }
                else
                {
                    filteredItems.Add(item);
                }
            }
            else
            {
                filteredItems.Add(item);
            }
        }

        return result;
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

    private void SellItemsOnFlea(TradeItem[] tradeItems)
    {
        string[] itemIds = [.. tradeItems.Select(x => x.Item.Id)];

        GClass2102[] requirements = [new()
        {
            count = tradeItems.First().FleaPrice!.GetTotalPriceInRouble(),
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