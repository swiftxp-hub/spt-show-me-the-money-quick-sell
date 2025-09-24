using System.Reflection;
using EFT.UI;
using HarmonyLib;
using SPT.Reflection.Patching;

namespace SwiftXP.SPT.ShowMeTheMoney.QuickSell.Patches;

public class InventoryScreenShowPatch : ModulePatch
{
    protected override MethodBase GetTargetMethod() =>
        AccessTools.FirstMethod(typeof(InventoryScreen), x => x.Name == nameof(InventoryScreen.Show));

    [PatchPostfix]
    public static void PatchPostfix(InventoryScreen __instance)
    {
        Plugin.IsInInventoryScreen = true;
    }
}