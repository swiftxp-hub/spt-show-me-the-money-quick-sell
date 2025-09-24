using BepInEx;
using SwiftXP.SPT.ShowMeTheMoney.QuickSell.Configuration;
using SwiftXP.SPT.Common.Loggers;
using SwiftXP.SPT.ShowMeTheMoney.QuickSell.Patches;

namespace SwiftXP.SPT.ShowMeTheMoney.QuickSell;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
[BepInDependency("SwiftXP.SPT.ShowMeTheMoney", "1.5.3")]
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
        SimpleSptLogger.Instance.Init(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_VERSION);
    }

    private void BindBepInExConfiguration()
    {
        SimpleSptLogger.Instance.LogInfo("Bind configuration...");

        Configuration = new PluginConfiguration(Config);
    }

    private void EnablePatches()
    {
        SimpleSptLogger.Instance.LogInfo("Enable patches...");

        new ItemViewOnClickPatch().Enable();
        new InventoryScreenClosePatch().Enable();
        new InventoryScreenShowPatch().Enable();
    }

    public static PluginConfiguration? Configuration;

    public static bool IsInInventoryScreen;
}