using BepInEx;
using SwiftXP.SPT.Common.Loggers;
using SwiftXP.SPT.ShowMeTheMoney.QuickSell.Client.Configuration;
using SwiftXP.SPT.ShowMeTheMoney.QuickSell.Client.Patches;

namespace SwiftXP.SPT.ShowMeTheMoney.QuickSell.Client;

[BepInPlugin("com.swiftxp.spt.showmethemoney.quicksell", MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
[BepInDependency("com.SPT.custom", "3.11.0")]
[BepInDependency("com.swiftxp.spt.showmethemoney", "1.9.0")]
[BepInProcess("EscapeFromTarkov.exe")]
public class Plugin : BaseUnityPlugin
{
    private void Awake()
    {
        InitLogger();
        BindBepInExConfiguration();
        EnablePatches();
    }

    private void InitLogger()
    {
        SptLogger = new SimpleSptLogger(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_VERSION);
    }

    private void BindBepInExConfiguration()
    {
        SptLogger!.LogInfo("Bind configuration...");

        Configuration = new PluginConfiguration(Config);
    }

    private void EnablePatches()
    {
        SptLogger!.LogInfo("Enable patches...");

        new GridItemViewOnClickPatch().Enable();
        new InventoryScreenClosePatch().Enable();
        new InventoryScreenShowPatch().Enable();
    }

    public static PluginConfiguration? Configuration;

    public static SimpleSptLogger? SptLogger;

    public static bool IsInInventoryScreen;
}