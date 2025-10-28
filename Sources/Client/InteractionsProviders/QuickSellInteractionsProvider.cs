using System.Collections.Generic;
using EFT.InventoryLogic;
using EFT.UI;
using IcyClawz.CustomInteractions;
using SwiftXP.SPT.Common.ConfigurationManager;
using SwiftXP.SPT.Common.Constants;
using SwiftXP.SPT.Common.Sessions;
using Plugin = SwiftXP.SPT.ShowMeTheMoney.QuickSell.Client.Plugin;

public class QuickSellInteractionsProvider : ICustomInteractionsProvider
{
    public IEnumerable<CustomInteraction> GetCustomInteractions(ItemUiContext context, EItemViewType viewType, Item item)
    {
        if (Plugin.IsInInventoryScreen && viewType is EItemViewType.Inventory)
        {
            List<Item> itemsToSell = GetItemsToSell(item);

            return GetInteractions(context, itemsToSell);
        }

        return [];
    }

    private IEnumerable<CustomInteraction> GetInteractions(ItemUiContext context, List<Item> itemsToSell)
    {
        yield return new(context)
        {
            Caption = () => "Sell quickly",
            Icon = () => EFTHardSettings.Instance.StaticIcons.GetBigCurrencySign(SptConstants.CurrencyIds.Roubles),
            SubMenu = () => GetSubInteractions(context, itemsToSell)
        };
    }

    private IList<CustomInteraction> GetSubInteractions(ItemUiContext context, List<Item> itemsToSell)
    {
        List<CustomInteraction> result =
        [
            new(context)
            {
                Enabled = () => SwiftXP.SPT.ShowMeTheMoney.Client.Plugin.Configuration!.EnableTraderPrices.IsEnabled(),
                Caption = () => "To Trader(s)",
                Action = () =>
                {
                    return;
                }
            },

            new(context)
            {
                Enabled = () => SwiftXP.SPT.ShowMeTheMoney.Client.Plugin.Configuration!.EnableFleaPrices.IsEnabled(),
                Caption = () => "On Flea",
                Action = () =>
                {
                    return;
                }
            },

            new(context)
            {
                Caption = () => "Best Deal(s)",
                Action = () =>
                {
                    return;
                }
            }
        ];

        return result;
    }

    private List<Item> GetItemsToSell(Item item)
    {
        List<Item> itemsToSell = [];

        if (UIFixesInterop.MultiSelect.Count > 0)
            itemsToSell.AddRange([.. UIFixesInterop.MultiSelect.Items]);

        else if (item is not null)
            itemsToSell.Add(item);

        List<Item> sellableItems = [];

        foreach (Item itemToSell in itemsToSell)
        {
            if (ItemMeetsRequirements(itemToSell))
            {
                sellableItems.Add(itemToSell);
            }
        }

        return sellableItems;
    }

    private bool ItemMeetsRequirements(Item item)
    {
        return SptSession.Session.Profile.Examined(item)
            && (!item.IsContainer || (item.IsContainer && item.IsEmpty()))
            && item.PinLockState != EItemPinLockState.Locked
            && !(item.Owner.OwnerType != EOwnerType.Profile && item.Owner.GetType() == typeof(TraderControllerClass));
    }
}