using System;
using System.Collections.Generic;
using System.Reflection;
using EFT.InventoryLogic;
using EFT.UI.DragAndDrop;
using HarmonyLib;
using SPT.Reflection.Patching;
using SwiftXP.SPT.Common.ConfigurationManager;
using SwiftXP.SPT.Common.EFT;
using SwiftXP.SPT.Common.Sessions;
using SwiftXP.SPT.ShowMeTheMoney.Client.Patches;
using SwiftXP.SPT.ShowMeTheMoney.QuickSell.Client.Models;
using SwiftXP.SPT.ShowMeTheMoney.QuickSell.Client.Services;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SwiftXP.SPT.ShowMeTheMoney.QuickSell.Client.Patches;

public class GridItemViewOnClickPatch : ModulePatch
{
    protected override MethodBase GetTargetMethod() =>
        AccessTools.FirstMethod(typeof(GridItemView), x => x.Name == nameof(GridItemView.OnClick));

    [PatchPrefix]
#pragma warning disable CA1707 // Identifiers should not contain underscores

    public static bool Prefix(GridItemView __instance, PointerEventData.InputButton button, Vector2 position, bool doubleClick)
#pragma warning restore CA1707 // Identifiers should not contain underscores

    {
        bool result = true;

        try
        {
            if (!PluginContextDataHolder.Current.IsInInventoryScreen
                || __instance == null
                || __instance.Item == null
                || !PluginContextDataHolder.Current.Configuration!.EnablePlugin.IsEnabled()
                || !IsQuickSellKeyPressed()
                || EFTHelper.IsInRaid)
            {
                return true;
            }

            List<Item> itemsToSell = GetItemsToSell(__instance);

            if (itemsToSell.Count != 0)
            {
                switch (button)
                {
                    case PointerEventData.InputButton.Left:
                        if (PluginContextDataHolder.Current.Configuration!.EnableTraderQuickSell.IsEnabled())
                        {
                            BrokerService.Trade(Enums.BrokerTradeType.Trader, [.. itemsToSell]);
                            result = false;
                        }

                        break;

                    case PointerEventData.InputButton.Right:
                        if (PluginContextDataHolder.Current.Configuration!.EnableFleaQuickSell.IsEnabled())
                        {
                            BrokerService.Trade(Enums.BrokerTradeType.Flea, [.. itemsToSell]);
                            result = false;
                        }

                        break;

                    case PointerEventData.InputButton.Middle:
                        if (PluginContextDataHolder.Current.Configuration!.EnableBestTradeQuickSell.IsEnabled())
                        {
                            BrokerService.Trade(Enums.BrokerTradeType.Best, [.. itemsToSell]);
                            result = false;
                        }

                        break;

                    default:
                        break;
                }
            }

            if (!result)
            {
                ShowMeTheMoney.Client.Models.PluginContextDataHolder.SetHoveredItem(null);
                SimpleTooltipShowPatch.Instance?.Close();
            }
        }
        catch (Exception exception)
        {
            PluginContextDataHolder.Current.SptLogger?
                .LogException(exception);
        }

        return result;
    }

    private static List<Item> GetItemsToSell(GridItemView __instance)
    {
        List<Item> itemsToSell = [];

        if (UIFixesInterop.MultiSelect.Count > 0)
            itemsToSell.AddRange([.. UIFixesInterop.MultiSelect.Items]);

        else if (__instance?.Item is not null)
            itemsToSell.Add(__instance!.Item);

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

    private static bool ItemMeetsRequirements(Item item)
    {
        return SptSession.Session.Profile.Examined(item)
            && (!item.IsContainer || (item.IsContainer && item.IsEmpty()))
            && item.PinLockState != EItemPinLockState.Locked
            && ItemIsFirAndAllowedToBeSold(item)
            && !(item.Owner.OwnerType != EOwnerType.Profile && item.Owner.GetType() == typeof(TraderControllerClass));
    }

    private static bool ItemIsFirAndAllowedToBeSold(Item item)
    {
        if (PluginContextDataHolder.Current.Configuration!.DoNotSellFoundInRaidItems.IsEnabled()
            && item.MarkedAsSpawnedInSession
            && !PluginContextDataHolder.Current.Configuration!.ForceSellFoundInRaidItemsKey.Value.IsPressed())
        {
            return false;
        }

        return true;
    }

    private static bool IsQuickSellKeyPressed()
    {
        if (PluginContextDataHolder.Current.Configuration!.QuickSellKey.Value.IsPressed())
            return true;

        if (PluginContextDataHolder.Current.Configuration!.DoNotSellFoundInRaidItems.IsEnabled() && PluginContextDataHolder.Current.Configuration!.ForceSellFoundInRaidItemsKey.Value.IsPressed())
            return true;

        return false;
    }
}