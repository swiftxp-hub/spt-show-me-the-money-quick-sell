using System.Reflection;
using EFT.UI;
using HarmonyLib;
using SPT.Reflection.Patching;

namespace SwiftXP.SPT.ShowMeTheMoney.QuickSell.Patches;

public class InventoryScreenClosePatch : ModulePatch
{
    protected override MethodBase GetTargetMethod() =>
        AccessTools.FirstMethod(typeof(InventoryScreen), x => x.Name == nameof(InventoryScreen.Close));

    [PatchPostfix]
    public static void PatchPostfix(InventoryScreen __instance)
    {
        Plugin.IsInInventoryScreen = false;
    }
}