using System.Collections.Generic;
using SwiftXP.SPT.ShowMeTheMoney.Models;

namespace SwiftXP.SPT.ShowMeTheMoney.QuickSell.Models;

public class BrokerTrade
{
    public BrokerTrade(string itemTemplateId, int price, params TradeItem[] items)
    {
        this.ItemTemplateId = itemTemplateId;
        this.Price = price;
        this.Items.AddRange(items);
    }

    public string ItemTemplateId { get; set; }

    public int Price { get; set; }

    public List<TradeItem> Items { get; private set; } = new();
}
