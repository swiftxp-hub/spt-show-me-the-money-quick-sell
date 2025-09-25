using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Comfort.Common;
using EFT;
using EFT.InventoryLogic;
using EFT.UI.DragAndDrop;
using HarmonyLib;
using SPT.Reflection.Patching;
using SwiftXP.SPT.Common.ConfigurationManager;
using SwiftXP.SPT.Common.Loggers;
using SwiftXP.SPT.Common.Sessions;
using SwiftXP.SPT.ShowMeTheMoney.Patches;
using SwiftXP.SPT.ShowMeTheMoney.QuickSell.Services;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SwiftXP.SPT.ShowMeTheMoney.QuickSell.Patches;

public class ItemViewOnClickPatch : ModulePatch
{
    protected override MethodBase GetTargetMethod() =>
        AccessTools.FirstMethod(typeof(GridItemView), x => x.Name == nameof(GridItemView.OnClick));

    [PatchPrefix]
    public static bool Prefix(GridItemView __instance, PointerEventData.InputButton button, Vector2 position, bool doubleClick)
    {
        bool result = true;

        try
        {
            if (!Plugin.IsInInventoryScreen
                || __instance == null
                || __instance.Item == null
                || !Plugin.Configuration!.EnablePlugin.IsEnabled()
                || !Plugin.Configuration!.QuickSellKey.Value.IsPressed()
                || IsInRaid())
            {
                return true;
            }

            List<Item> itemsToSell = GetItemsToSell(__instance);

            if (itemsToSell.Any())
            {
                switch (button)
                {
                    case PointerEventData.InputButton.Left:
                        if (ShowMeTheMoney.Plugin.Configuration!.EnableTraderPrices.IsEnabled())
                        {
                            BrokerService.Instance.Trade(Enums.BrokerTradeEnum.Trader, [.. itemsToSell]);
                            result = false;
                        }

                        break;

                    case PointerEventData.InputButton.Right:
                        if (ShowMeTheMoney.Plugin.Configuration!.EnableFleaPrices.IsEnabled())
                        {
                            BrokerService.Instance.Trade(Enums.BrokerTradeEnum.Flea, [.. itemsToSell]);
                            result = false;
                        }

                        break;

                    default:
                        break;
                }
            }

            if (!result)
            {
                ShowMeTheMoney.Plugin.HoveredItem = null;
                SimpleTooltipShowPatch.Instance?.Close();
            }
        }
        catch (Exception exception)
        {
            SimpleSptLogger.Instance.LogException(exception);
        }

        return result;
    }

    private static bool IsInRaid()
    {
        bool? inRaid = Singleton<AbstractGame>.Instance?.InRaid;

        return inRaid.HasValue && inRaid.Value;
    }

    private static List<Item> GetItemsToSell(GridItemView __instance)
    {
        List<Item> itemsToSell = [];

        if (UIFixesInterop.MultiSelect.Count > 0)
            itemsToSell.AddRange(UIFixesInterop.MultiSelect.Items);

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
            && !(item.Owner.OwnerType != EOwnerType.Profile && item.Owner.GetType() == typeof(TraderControllerClass));
    }
}