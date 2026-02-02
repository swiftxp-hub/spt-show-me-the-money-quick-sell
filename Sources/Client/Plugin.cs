using BepInEx;
using SwiftXP.SPT.Common.Loggers;
using SwiftXP.SPT.ShowMeTheMoney.QuickSell.Client.Configuration;
using SwiftXP.SPT.ShowMeTheMoney.QuickSell.Client.Models;
using SwiftXP.SPT.ShowMeTheMoney.QuickSell.Client.Patches;

namespace SwiftXP.SPT.ShowMeTheMoney.QuickSell.Client;

[BepInPlugin("com.swiftxp.spt.showmethemoney.quicksell", MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
[BepInDependency("com.SPT.custom", "4.0.11")]
[BepInDependency("com.swiftxp.spt.showmethemoney", "2.6.0")]
[BepInProcess("EscapeFromTarkov.exe")]
public class Plugin : BaseUnityPlugin
{
    private void Awake()
    {
        SimpleSptLogger simpleSptLogger = new(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_VERSION);
        PluginConfiguration pluginConfiguration = new(Config);

        PluginContextDataHolder.SetContextInstances(simpleSptLogger, pluginConfiguration);

        EnablePatches();
    }

    private static void EnablePatches()
    {
        new GridItemViewOnClickPatch().Enable();
        new InventoryScreenClosePatch().Enable();
        new InventoryScreenShowPatch().Enable();
    }
}