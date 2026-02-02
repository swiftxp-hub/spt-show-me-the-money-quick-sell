using SwiftXP.SPT.Common.Loggers;
using SwiftXP.SPT.ShowMeTheMoney.QuickSell.Client.Configuration;

namespace SwiftXP.SPT.ShowMeTheMoney.QuickSell.Client.Models;

public static class PluginContextDataHolder
{
    private static volatile PluginContextData s_currentData = new(null, null, false);

    public static PluginContextData Current
    {
        get { return s_currentData; }
    }

    public static void SetIsInInventoryScreen(bool isInInventoryScreen)
    {
        PluginContextData pluginContextData = new(Current?.SptLogger, Current?.Configuration, isInInventoryScreen);
        System.Threading.Interlocked.Exchange(ref s_currentData, pluginContextData);
    }

    public static void SetContextInstances(SimpleSptLogger simpleSptLogger, PluginConfiguration pluginConfiguration)
    {
        PluginContextData pluginContextData = new(simpleSptLogger, pluginConfiguration, Current?.IsInInventoryScreen ?? false);
        System.Threading.Interlocked.Exchange(ref s_currentData, pluginContextData);
    }
}