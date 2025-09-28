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

        this.QuickSellKey = configFile.BindConfiguration("1. Main settings", "Quick-sell key", new KeyboardShortcut(KeyCode.Q), $"Defines which key or key combination needs to be pressed in conjunction with the left or right mouse button to sell items quickly.{Environment.NewLine}{Environment.NewLine}(Default: Q)", 0);

        this.AllowAnyNumberOfFleaOffers = configFile.BindConfiguration("2. Flea market", "Allow any number of offers at the flea market", false, $"Allows you to sell any number of items at the flea market and ignores the limitations normally imposed by Tarkov.{Environment.NewLine}{Environment.NewLine}(Default: Disabled)", 2);

        this.DoNotSellFoundInRaidItems = configFile.BindConfiguration("2. Flea market", "Do not sell items which are found in raid", false, $"Prevents items found in the raid from being sold. Selling items found in raid can still be forced using the configured key.{Environment.NewLine}{Environment.NewLine}(Default: Disabled)", 1);

        this.ForceSellFoundInRaidItemsKey = configFile.BindConfiguration("2. Flea market", "Force sell found in raid items key", new KeyboardShortcut(KeyCode.LeftAlt), $"Forces the sale of items found in the raid.{Environment.NewLine}{Environment.NewLine}(Default: Left alt)", 0);

        configFile.SaveOnConfigSet = true;
    }

    #region Main settings 

    public ConfigEntry<bool> EnablePlugin { get; set; }

    public ConfigEntry<KeyboardShortcut> QuickSellKey { get; set; }

    public ConfigEntry<bool> AllowAnyNumberOfFleaOffers { get; set; }

    public ConfigEntry<bool> DoNotSellFoundInRaidItems { get; set; }

    public ConfigEntry<KeyboardShortcut> ForceSellFoundInRaidItemsKey { get; set; }

    #endregion
}