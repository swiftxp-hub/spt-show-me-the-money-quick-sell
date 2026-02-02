using System.Reflection;
using EFT.UI;
using HarmonyLib;
using SPT.Reflection.Patching;
using SwiftXP.SPT.Common.EFT;
using SwiftXP.SPT.ShowMeTheMoney.QuickSell.Client.Models;

namespace SwiftXP.SPT.ShowMeTheMoney.QuickSell.Client.Patches;

public class InventoryScreenShowPatch : ModulePatch
{
    protected override MethodBase GetTargetMethod() =>
        AccessTools.FirstMethod(typeof(InventoryScreen), x => x.Name == nameof(InventoryScreen.Show));

    [PatchPostfix]
#pragma warning disable CA1707 // Identifiers should not contain underscores

    public static void PatchPostfix(InventoryScreen __instance)
#pragma warning restore CA1707 // Identifiers should not contain underscores

    {
        if (!EFTHelper.IsInRaid)
            PluginContextDataHolder.SetIsInInventoryScreen(true);
    }
}