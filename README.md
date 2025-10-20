A client modification for SPT (Single Player Tarkov).

**Please note that this mod only works in conjunction with the [Show Me The Money SPT-mod](https://forge.sp-tarkov.com/mod/2299/show-me-the-money).**

___

## Tabs {.tabset}

### SPT 4.0.x
#### What does it do?

It allows you to quickly sell items from your inventory to traders or on the flea market using a configurable keyboard shortcut and the left, middle or right mouse button. By default...

- Q + Left-mouse click => Sell to trader
- Q + Right-mouse click => Sell on flea market
- Q + Middle-mouse click => Sell at the best price (trader or flea market)

The configuration settings from the "Show Me The Money" SPT-mod are taken into account by this quick-sale companion mod.

[UIFixes](https://forge.sp-tarkov.com/mod/1342/ui-fixes) by Tyfon is currently not fully supported, but support will come back in the near future.

#### Requirements

- [Show Me The Money SPT-mod](https://forge.sp-tarkov.com/mod/2299/show-me-the-money) v2.0.1 or newer

#### Installation

Extract the contents of the .zip or .7z file into your SPT directory. 

You should end up having the following files copied to your SPT directory:
```
- C:\yourSPTfolder\BepInEx\plugins\com.swiftxp.spt.showmethemoney.quicksell\SwiftXP.SPT.ShowMeTheMoney.QuickSell.Client.dll
```

##### If you use the Fika headless client

There is no need to install anything to your Fika headless client for this mod to work properly. I would actually recommend to NOT install my mod on your Fika headless client. This also means that I recommend to add my mod to your Exclusions.json, if you use [Corter's Mod Sync](https://github.com/c-orter/ModSync). [Please see his FAQ on how to add sync-exclusions](https://github.com/c-orter/ModSync/wiki/Configuration#exclusions).

#### Configuration

Please use the BepInEx configurator to configure features of the mod (usually accessible by pressing F12 or F1 when you are in-game):

![BepInEx Plugin Configuration](https://raw.githubusercontent.com/swiftxp-hub/spt-show-me-the-money-quick-sell/refs/heads/main/Assets/plugin-configuration.png)

#### Remarks

- Quick-selling is only possible from the inventory-screen for now.

### SPT 3.11.x
#### What does it do?

It allows you to quickly sell items from your inventory to traders or on the flea market using a configurable keyboard shortcut and the left, middle or right mouse button. By default...

- Q + Left-mouse click => Sell to trader
- Q + Right-mouse click => Sell on flea market
- Q + Middle-mouse click => Sell at the best price (trader or flea market)

The configuration settings from the "Show Me The Money" SPT-mod are taken into account by this quick-sale companion mod.

[UIFixes](https://forge.sp-tarkov.com/mod/1342/ui-fixes) v4.2.2 or later by Tyfon is supported and with the help of multi-selection, several items can be sold at the same time. Items of the same type are bundled automatically when selling on the flea market.

Please note that it is always advisable to back up your SPT profiles before installing new mods.

#### Requirements

- [Show Me The Money SPT-mod](https://forge.sp-tarkov.com/mod/2299/show-me-the-money) v1.6.1 or newer versions for SPT 3.11.x

#### Installation

Extract the contents of the .zip or .7z file into your SPT directory. 

You should end up having the following files copied to your SPT directory:
```
- C:\yourSPTfolder\BepInEx\plugins\SwiftXP.SPT.ShowMeTheMoney.QuickSell.dll
```

##### If you use the Fika headless client

There is no need to install anything to your Fika headless client for this mod to work properly. I would actually recommend to NOT install my mod on your Fika headless client. This also means that I recommend to add my mod to your Exclusions.json, if you use [Corter's Mod Sync](https://github.com/c-orter/ModSync). [Please see his FAQ on how to add sync-exclusions](https://github.com/c-orter/ModSync/wiki/Configuration#exclusions).

#### Configuration

Please use the BepInEx configurator to configure features of the mod (usually accessible by pressing F12 or F1 when you are in-game):

![BepInEx Plugin Configuration](https://raw.githubusercontent.com/swiftxp-hub/spt-show-me-the-money-quick-sell/refs/heads/main-spt311/Assets/plugin-configuration.png)

#### Remarks

- Quick-selling is only possible from the inventory-screen.

#### Known compatibility

- [UIFixes](https://forge.sp-tarkov.com/mod/1342/ui-fixes) v4.2.2 or later versions for SPT 3.11.x by Tyfon. 

#### Support for 3.11.x

I will try to continue supporting SPT 3.11.x as long as the SPT team also supports this version. However, I cannot promise this 100%. Support will be limited to bug fixes and minor adjustments.

{.endtabset}

___

##### Support and feature requests

Please note that I maintain all my mods in my spare time. Therefore, I can only invest a limited amount of time, especially when it comes to support requests.