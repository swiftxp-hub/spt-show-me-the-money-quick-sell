using EFT.InventoryLogic;
using SwiftXP.SPT.ShowMeTheMoney.QuickSell.Enums;

namespace SwiftXP.SPT.ShowMeTheMoney.QuickSell.Models;

public class BrokerTrade
{
    public BrokerTrade(BrokerTradeEnum trade, Item item)
    {
        this.Trade = trade;
        this.Item = item;
    }

    public BrokerTradeEnum Trade { get; private set; }

    public Item Item { get; private set; }
}
