using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using EFT.InventoryLogic;
using EFT.UI.DragAndDrop;
using HarmonyLib;
using SPT.Reflection.Patching;
using SwiftXP.SPT.Common.ConfigurationManager;
using SwiftXP.SPT.Common.EFT;
using SwiftXP.SPT.Common.Loggers;
using SwiftXP.SPT.Common.Sessions;
using SwiftXP.SPT.ShowMeTheMoney.Client.Patches;
using SwiftXP.SPT.ShowMeTheMoney.QuickSell.Client.Services;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SwiftXP.SPT.ShowMeTheMoney.QuickSell.Client.Patches;

public class GridItemViewOnClickPatch : ModulePatch
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
                || !IsQuickSellKeyPressed()
                || EFTHelper.IsInRaid)
            {
                return true;
            }

            List<Item> itemsToSell = GetItemsToSell(__instance);

            if (itemsToSell.Any())
            {
                switch (button)
                {
                    case PointerEventData.InputButton.Left:
                        if (Plugin.Configuration!.EnableTraderQuickSell.IsEnabled())
                        {
                            BrokerService.Instance.Trade(Enums.BrokerTradeTypeEnum.Trader, [.. itemsToSell]);
                            result = false;
                        }

                        break;

                    case PointerEventData.InputButton.Right:
                        if (Plugin.Configuration!.EnableFleaQuickSell.IsEnabled())
                        {
                            BrokerService.Instance.Trade(Enums.BrokerTradeTypeEnum.Flea, [.. itemsToSell]);
                            result = false;
                        }

                        break;

                    case PointerEventData.InputButton.Middle:
                        if (Plugin.Configuration!.EnableBestTradeQuickSell.IsEnabled())
                        {
                            BrokerService.Instance.Trade(Enums.BrokerTradeTypeEnum.Best, [.. itemsToSell]);
                            result = false;
                        }

                        break;

                    default:
                        break;
                }
            }

            if (!result)
            {
                ShowMeTheMoney.Client.Plugin.HoveredItem = null;
                SimpleTooltipShowPatch.Instance?.Close();
            }
        }
        catch (Exception exception)
        {
            Plugin.SptLogger!.LogException(exception);
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
            && ItemIsFirAndAllowToBeSold(item)
            && ItemsIsNotFirAndAllowToBeSold(item)
            && !(item.Owner.OwnerType != EOwnerType.Profile && item.Owner.GetType() == typeof(TraderControllerClass));
    }

    private static bool ItemIsFirAndAllowToBeSold(Item item)
    {
        if (Plugin.Configuration!.DoNotSellFoundInRaidItems.IsEnabled()
            && item.MarkedAsSpawnedInSession
            && !Plugin.Configuration!.ForceSellFoundInRaidItemsKey.Value.IsPressed())
        {
            return false;
        }

        return true;
    }

    private static bool ItemsIsNotFirAndAllowToBeSold(Item item)
    {
        if (!item.MarkedAsSpawnedInSession && RagFairClass.Settings.isOnlyFoundInRaidAllowed)
        {
            return false;
        }

        return true;
    }

    private static bool IsQuickSellKeyPressed()
    {
        if (Plugin.Configuration!.QuickSellKey.Value.IsPressed())
            return true;

        if (Plugin.Configuration!.DoNotSellFoundInRaidItems.IsEnabled() && Plugin.Configuration!.ForceSellFoundInRaidItemsKey.Value.IsPressed())
            return true;

        return false;
    }
}