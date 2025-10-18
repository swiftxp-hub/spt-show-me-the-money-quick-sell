# Show Me The Money - Quick-Sell SPT-mod

A client modification for SPT (Single Player Tarkov).

**Please note that this mod only works in conjunction with the [Show Me The Money SPT-mod](https://forge.sp-tarkov.com/mod/2299/show-me-the-money).**

**Warning:** There is a bug in version 1.0.0 and older versions with stackable items (bullets) and selling them on the flea-market. The wrong (total instead of per bullet) price is used to sell it on the flea-market. A hot-fix is available (version 1.0.1 or newer).

## What does it do?

It allows you to quickly sell items from your inventory to traders or on the flea market using a configurable keyboard shortcut and the left, middle or right mouse button. By default...

- Q + Left-mouse click => Sell to trader
- Q + Right-mouse click => Sell on flea market
- Q + Middle-mouse click => Sell at the best price (trader or flea market)

The configuration settings from the "Show Me The Money" SPT-mod are taken into account by this quick-sale companion mod.

[UIFixes](https://forge.sp-tarkov.com/mod/1342/ui-fixes) v4.2.2 or later by Tyfon is supported and with the help of multi-selection, several items can be sold at the same time. Items of the same type are bundled automatically when selling on the flea market.

Please note that it is always advisable to back up your SPT profiles before installing new mods.

## Requirements

- [Show Me The Money SPT-mod](https://forge.sp-tarkov.com/mod/2299/show-me-the-money) v1.6.1 or newer

## Installation

Extract the contents of the .zip file into your SPT directory. 

You should end up having the following files copied to your SPT directory:
- C:\yourSPTfolder\BepInEx\plugins\SwiftXP.SPT.ShowMeTheMoney.QuickSell.Client.dll

### If you use the Fika headless client

There is no need to install anything to your Fika headless client for this mod to work properly. I would actually recommend to NOT install my mod on your Fika headless client. This also means that I recommend to add my mod to your Exclusions.json, if you use [Corter's Mod Sync](https://github.com/c-orter/ModSync). [Please see his FAQ on how to add sync-exclusions](https://github.com/c-orter/ModSync/wiki/Configuration#exclusions).

## Configuration

Please use the BepInEx configurator to configure features of the mod (usually accessible by pressing F12 or F1 when you are in-game):

![BepInEx Plugin Configuration](https://raw.githubusercontent.com/swiftxp-hub/spt-show-me-the-money-quick-sell/refs/heads/main/Assets/plugin-configuration.png)

## Remarks

- Quick-selling is only possible from the inventory-screen for now.

## Known compatibility

- [UIFixes](https://forge.sp-tarkov.com/mod/1342/ui-fixes) v4.2.2 or later by Tyfon. 

## Tested environment

- SPT 3.11.4 (this mod should work with every SPT 3.11 release, but it's not tested except for version 3.11.4)
- EFT 16.1.3.35392

## Support and feature requests

Please note that I maintain all my mods in my spare time. Therefore, I can only invest a limited amount of time, especially when it comes to support requests.

## Features that may come in the future

- More info, like if items could not be sold. In particular, selling using the middle mouse button currently provides little information.
- Quick-sell via context-menu (right-mouse click on items)
- What-ever comes to my mind or by feature-requests in the comments/SPT-discord