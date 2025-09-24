# Show Me The Money - Quick-Sell SPT-mod

A BepInEx plugin for SPT (Single Player Tarkov).
**<span style="color:red">Please note that this plug-in only works in conjunction with the [Show Me The Money SPT-mod](https://github.com/swiftxp-hub/spt-show-me-the-money).</span>**

## What does it do?

It allows you to quickly sell items from your inventory to traders or on the flea market using a configurable keyboard shortcut and the left or right mouse button. By default...

- Q + Left-mouse click => Sell to trader
- Q + Right-mouse click => Sell on flea market

The configuration settings from the "Show Me The Money" SPT-mod are taken into account by this quick-sale companion mod.

Please note that this is an early version of this mod and it is always advisable to back up your SPT profiles before installing new mods.

## Requirements

- [Show Me The Money SPT-mod](github.com/swiftxp-hub/spt-show-me-the-money) v1.5.3

## Installation

Extract the contents of the .zip file into your SPT directory. 

You should end up having the following files copied to your SPT directory:
- C:\yourSPTfolder\BepInEx\plugins\SwiftXP.SPT.ShowMeTheMoney.QuickSell.dll

### If you use the Fika headless client

There is no need to install anything to your Fika headless client for this mod to work properly. I would actually recommend to NOT install my mod on your Fika headless client. This also means that I recommend to add my mod to your Exclusions.json, if you use [Corter's Mod Sync](https://github.com/c-orter/ModSync). [Please see his FAQ on how to add sync-exclusions](https://github.com/c-orter/ModSync/wiki/Configuration#exclusions).

## Configuration

Please use the BepInEx configurator to configure features of the mod (usually accessible by pressing F12 or F1 when you are in-game):

![BepInEx Plugin Configuration](https://raw.githubusercontent.com/swiftxp-hub/spt-show-me-the-money-quick-sell/refs/heads/main/Assets/plugin-configuration.png)<br />
*(Default configuration with freshly installed mod)*

## Remarks

- Quick-selling is only possible from the inventory-screen for now.

## Known compatibility

- [UIFixes](https://github.com/tyfon7/UIFixes) v4.2.2 by Tyfon. Yes, with the help of multi-selection, several items can be sold at the same time.

## Tested environment

- SPT 3.11.4 (this mod should work with every SPT 3.11 release, but it's not tested except for version 3.11.4)
- EFT 16.1.3.35392

## Support and feature requests

Please note that I maintain all my mods in my spare time. Therefore, I can only invest a limited amount of time, especially when it comes to support requests. The following principle always applies to support requests: No logs, no support. [Please follow this link to the SPT FAQ to find your logs](https://hub.sp-tarkov.com/faq-question/64-where-can-i-find-my-log-files/).

## Features that may come in the future

- Quick-sell via context-menu (right-mouse click on items)
- What-ever comes to my mind or by feature-requests in the comments/SPT-discord