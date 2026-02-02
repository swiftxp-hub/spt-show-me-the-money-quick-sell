using System.Collections.Generic;
using SwiftXP.SPT.ShowMeTheMoney.Client.Models;

namespace SwiftXP.SPT.ShowMeTheMoney.QuickSell.Client.Models;

public class BrokerTraderTrade
{
    public BrokerTraderTrade(string traderId, params TradeItem[] items)
    {
        TraderId = traderId;
        TradeItems.AddRange(items);
    }

    public string TraderId { get; private set; }

    public List<TradeItem> TradeItems { get; private set; } = [];
}