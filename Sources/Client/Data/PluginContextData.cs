using SwiftXP.SPT.Common.Loggers;
using SwiftXP.SPT.ShowMeTheMoney.QuickSell.Client.Configuration;

namespace SwiftXP.SPT.ShowMeTheMoney.QuickSell.Client.Data;

public record PluginContextData
{
    public PluginContextData(SimpleSptLogger? simpleSptLogger, PluginConfiguration? pluginConfiguration,
        bool isInInventoryScreen)
    {
        SptLogger = simpleSptLogger;
        Configuration = pluginConfiguration;
        IsInInventoryScreen = isInInventoryScreen;
    }

    public SimpleSptLogger? SptLogger { get; }

    public PluginConfiguration? Configuration { get; set; }

    public bool IsInInventoryScreen { get; set; }
}