using BepInEx.Configuration;
using SwiftXP.SPT.Common.ConfigurationManager;
using System;
using UnityEngine;

namespace SwiftXP.SPT.ShowMeTheMoney.QuickSell.Configuration;

public class PluginConfiguration
{
    public PluginConfiguration(ConfigFile configFile)
    {
        // --- 1. Main settings
        this.EnablePlugin = configFile.BindConfiguration("1. Main settings", "Enable plug-in", true, $"Enable or disable the plug-in.{Environment.NewLine}{Environment.NewLine}(Default: Enabled)", 1);

        this.QuickSellKey = configFile.BindConfiguration("1. Main settings", "Quick-Sell key", new KeyboardShortcut(KeyCode.Q), $"Defines which key or key combination needs to be pressed to sell items quickly.{Environment.NewLine}{Environment.NewLine}(Default: Q)", 0);

        this.AllowAnyNumberOfFleaOffers = configFile.BindConfiguration("2. Flea market", "Allow any number of offers at the flea market", false, $"Allows you to sell any number of items at the flea market and ignores the limitations normally imposed by Tarkov.{Environment.NewLine}{Environment.NewLine}(Default: Disabled)", 0);

        configFile.SaveOnConfigSet = true;
    }

    #region Main settings 

    public ConfigEntry<bool> EnablePlugin { get; set; }

    public ConfigEntry<KeyboardShortcut> QuickSellKey { get; set; }

    public ConfigEntry<bool> AllowAnyNumberOfFleaOffers { get; set; }

    #endregion
}