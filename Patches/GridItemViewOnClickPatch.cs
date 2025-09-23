using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Comfort.Common;
using EFT;
using EFT.InventoryLogic;
using EFT.Trading;
using EFT.UI;
using EFT.UI.DragAndDrop;
using HarmonyLib;
using SPT.Reflection.Patching;
using SwiftXP.SPT.Common.ConfigurationManager;
using SwiftXP.SPT.Common.Constants;
using SwiftXP.SPT.Common.Loggers;
using SwiftXP.SPT.Common.Notifications;
using SwiftXP.SPT.Common.Sessions;
using SwiftXP.SPT.ShowMeTheMoney.Models;
using SwiftXP.SPT.ShowMeTheMoney.Patches;
using SwiftXP.SPT.ShowMeTheMoney.QuickSell.Services;
using SwiftXP.SPT.ShowMeTheMoney.Services;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SwiftXP.SPT.ShowMeTheMoney.QuickSell.Patches;

public class ItemViewOnClickPatch : ModulePatch
{
    protected override MethodBase GetTargetMethod() =>
        AccessTools.FirstMethod(typeof(GridItemView), x => x.Name == "OnClick");

    [PatchPrefix]
    public static bool Prefix(GridItemView __instance, PointerEventData.InputButton button, Vector2 position, bool doubleClick)
    {
        bool result = true;

        if (__instance == null
            || __instance.Item == null
            || !Plugin.Configuration!.EnablePlugin.IsEnabled()
            || !ShowMeTheMoney.Plugin.Configuration!.EnableTraderPrices.IsEnabled()
            || !Plugin.Configuration!.QuickSellKey.Value.IsPressed()
            || IsInRaid())
        {
            return true;
        }

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

        if (sellableItems.Any())
        {
            ShowMeTheMoney.Plugin.HoveredItem = null;
            SimpleTooltipShowPatch.Instance?.Close();

            switch (button)
            {
                case PointerEventData.InputButton.Left:
                    BrokerService.Instance.Trade(Enums.BrokerTradeEnum.Trader, [.. sellableItems]);
                    result = false;

                    break;

                case PointerEventData.InputButton.Right:
                    BrokerService.Instance.Trade(Enums.BrokerTradeEnum.Flea, [.. sellableItems]);
                    result = false;

                    break;

                default:
                    break;
            }
        }

        return result;
    }

    private static bool IsInRaid()
    {
        bool? inRaid = Singleton<AbstractGame>.Instance?.InRaid;

        return inRaid.HasValue && inRaid.Value;
    }

    private static bool ItemMeetsRequirements(Item item)
    {
        return SptSession.Session.Profile.Examined(item)
            && (!item.IsContainer || (item.IsContainer && item.IsEmpty()))
            && !(item.Owner.OwnerType != EOwnerType.Profile && item.Owner.GetType() == typeof(TraderControllerClass));
    }
}