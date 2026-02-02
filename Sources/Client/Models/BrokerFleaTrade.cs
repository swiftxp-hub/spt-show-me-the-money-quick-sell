using System.Collections.Generic;
using SwiftXP.SPT.ShowMeTheMoney.Client.Models;

namespace SwiftXP.SPT.ShowMeTheMoney.QuickSell.Client.Models;

public class BrokerFleaTrade
{
    public BrokerFleaTrade(string itemTemplateId, int price, params TradeItem[] items)
    {
        ItemTemplateId = itemTemplateId;
        Price = price;
        TradeItems.AddRange(items);
    }

    public string ItemTemplateId { get; private set; }

    public int Price { get; private set; }

    public List<TradeItem> TradeItems { get; private set; } = [];
}
