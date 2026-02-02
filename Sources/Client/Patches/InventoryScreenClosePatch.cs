using System.Reflection;
using EFT.UI;
using HarmonyLib;
using SPT.Reflection.Patching;
using SwiftXP.SPT.Common.EFT;
using SwiftXP.SPT.ShowMeTheMoney.QuickSell.Client.Data;

namespace SwiftXP.SPT.ShowMeTheMoney.QuickSell.Client.Patches;

public class InventoryScreenClosePatch : ModulePatch
{
    protected override MethodBase GetTargetMethod() =>
        AccessTools.FirstMethod(typeof(InventoryScreen), x => x.Name == nameof(InventoryScreen.Close));

    [PatchPostfix]
#pragma warning disable CA1707 // Identifiers should not contain underscores

    public static void PatchPostfix(InventoryScreen __instance)
#pragma warning restore CA1707 // Identifiers should not contain underscores

    {
        if (!EFTHelper.IsInRaid)
            PluginContextDataHolder.SetIsInInventoryScreen(false);
    }
}