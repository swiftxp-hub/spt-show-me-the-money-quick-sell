using BepInEx.Configuration;
using SwiftXP.SPT.Common.ConfigurationManager;
using System;
using UnityEngine;

namespace SwiftXP.SPT.ShowMeTheMoney.QuickSell.Client.Configuration;

public class PluginConfiguration
{
    public PluginConfiguration(ConfigFile configFile)
    {
        // --- 1. Main settings
        this.EnablePlugin = configFile.BindConfiguration("1. Main settings", "Enable plug-in", true, $"Enable or disable the plug-in.{Environment.NewLine}{Environment.NewLine}(Default: Enabled)", 4);
        this.QuickSellKey = configFile.BindConfiguration("1. Main settings", "Quick-sell key", new KeyboardShortcut(KeyCode.Q), $"Specifies which key or key combination must be pressed in conjunction with the left, middle or right mouse button to quickly sell items.{Environment.NewLine}{Environment.NewLine}(Default: Q)", 3);

        this.EnableTraderQuickSell = configFile.BindConfiguration("1. Main settings", "Enable trader quick-sell", true, $"Enables the quick-sell to trader(s) feature (Left mouse button).{Environment.NewLine}{Environment.NewLine}(Default: Enabled)", 2);
        this.EnableFleaQuickSell = configFile.BindConfiguration("1. Main settings", "Enable flea market quick-sell", true, $"Enables the quick-sell to flea market feature (Right mouse button).{Environment.NewLine}{Environment.NewLine}(Default: Enabled)", 1);
        this.EnableBestTradeQuickSell = configFile.BindConfiguration("1. Main settings", "Enable best trade quick-sell", true, $"Enables the quick-sell for best trade feature (Middle mouse button).{Environment.NewLine}{Environment.NewLine}(Default: Enabled)", 0);

        // --- 2. Flea market
        this.SellToTraderIfFleaSlotsFull = configFile.BindConfiguration("2. Flea market", "Sell to trader(s) if no flea slots left", false, $"If there are no more slots available at the flea market, the items will be sold to traders instead.{Environment.NewLine}{Environment.NewLine}(Default: Disabled)", 3);
        this.AllowAnyNumberOfFleaOffers = configFile.BindConfiguration("2. Flea market", "Allow any number of offers at the flea market", false, $"Allows you to sell any number of items at the flea market and ignores the limitations normally imposed by Tarkov.{Environment.NewLine}{Environment.NewLine}(Default: Disabled)", 2);
        this.DoNotSellFoundInRaidItems = configFile.BindConfiguration("2. Flea market", "Do not sell items which are found in raid", false, $"Prevents items found in the raid from being sold. Selling items found in raid can still be forced using the configured key.{Environment.NewLine}{Environment.NewLine}(Default: Disabled)", 1);
        this.ForceSellFoundInRaidItemsKey = configFile.BindConfiguration("2. Flea market", "Force sell found in raid items key", new KeyboardShortcut(KeyCode.Q, KeyCode.LeftAlt), $"Specifies which key or key combination must be pressed in conjunction with the left, middle or right mouse button to quickly sell items, ignoring the \"Found in Raid\" status. Can only be used if the option \"Do not sell items which are found in raid\" is enabled.{Environment.NewLine}{Environment.NewLine}(Default: Q + Left alt)", 0);

        configFile.SaveOnConfigSet = true;
    }

    #region Main settings 

    public ConfigEntry<bool> EnablePlugin { get; set; }

    public ConfigEntry<KeyboardShortcut> QuickSellKey { get; set; }

    public ConfigEntry<bool> EnableTraderQuickSell { get; set; }

    public ConfigEntry<bool> EnableFleaQuickSell { get; set; }

    public ConfigEntry<bool> EnableBestTradeQuickSell { get; set; }

    #endregion

    #region Flea market

    public ConfigEntry<bool> SellToTraderIfFleaSlotsFull { get; set; }

    public ConfigEntry<bool> AllowAnyNumberOfFleaOffers { get; set; }

    public ConfigEntry<bool> DoNotSellFoundInRaidItems { get; set; }

    public ConfigEntry<KeyboardShortcut> ForceSellFoundInRaidItemsKey { get; set; }

    #endregion
}