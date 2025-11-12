A client modification for SPT (Single Player Tarkov).

**Please note that this mod only works in conjunction with the [Show Me The Money SPT-mod](https://forge.sp-tarkov.com/mod/2299/show-me-the-money).**

___

## Tabs {.tabset}

### SPT 4.0.x
#### What does it do?

This mod allows you to quickly sell items from your inventory to traders or on the flea market using a configurable keyboard shortcut combined with the left, middle, or right mouse button. By default:

- Q + Left-mouse click → Sell to trader  
- Q + Right-mouse click → Sell on flea market  
- Q + Middle-mouse click → Sell at the best price (trader or flea market)  

The configuration settings from the "Show Me The Money" SPT-mod are applied automatically by this quick-sale addon mod.

With [UIFixes](https://forge.sp-tarkov.com/mod/1342/ui-fixes) v5.0.2 or later by Tyfon, multiple items can be selected and sold simultaneously. Items of the same type are automatically bundled when selling on the flea market.  

#### Requirements

- [Show Me The Money SPT-mod](https://forge.sp-tarkov.com/mod/2299/show-me-the-money) v2.2.0 or newer

#### Installation

Extract the contents of the `.zip` or `.7z` file into your SPT directory.  

You should end up with the following file in your SPT directory:
```
- C:\yourSPTfolder\BepInEx\plugins\com.swiftxp.spt.showmethemoney.quicksell\SwiftXP.SPT.ShowMeTheMoney.QuickSell.Client.dll
```

##### If you use the Fika headless client

There is no need to install this mod on your Fika headless client. In fact, I recommend **not** installing it there.  

If you use [Corter's Mod Sync](https://github.com/c-orter/ModSync), it is recommended to add this mod to your `Exclusions.json`. [See his FAQ on adding sync exclusions](https://github.com/c-orter/ModSync/wiki/Configuration#exclusions).

#### Configuration

Use the BepInEx configurator to adjust mod settings (usually accessible by pressing F12 or F1 in-game):

![BepInEx Plugin Configuration](https://raw.githubusercontent.com/swiftxp-hub/spt-show-me-the-money-quick-sell/refs/heads/main/Assets/plugin-configuration.png)

#### Remarks

- Quick-selling is currently only possible from the inventory screen.

#### Known compatibility

- [UIFixes](https://forge.sp-tarkov.com/mod/1342/ui-fixes) v5.0.2 or later for SPT 4.0.x by Tyfon  

---

### SPT 3.11.x
#### What does it do?

This mod allows you to quickly sell items from your inventory to traders or on the flea market using a configurable keyboard shortcut combined with the left, middle, or right mouse button. By default:

- Q + Left-mouse click → Sell to trader  
- Q + Right-mouse click → Sell on flea market  
- Q + Middle-mouse click → Sell at the best price (trader or flea market)  

The configuration settings from the "Show Me The Money" SPT-mod are applied automatically.  

With [UIFixes](https://forge.sp-tarkov.com/mod/1342/ui-fixes) v4.2.2 or later by Tyfon, multiple items can be selected and sold simultaneously. Items of the same type are automatically bundled when selling on the flea market.  

**Note:** Always back up your SPT profiles before installing new mods.

#### Requirements

- [Show Me The Money SPT-mod](https://forge.sp-tarkov.com/mod/2299/show-me-the-money) v1.6.1 or newer for SPT 3.11.x

#### Installation

Extract the contents of the `.zip` or `.7z` file into your SPT directory.  

You should end up with the following file in your SPT directory:
```
- C:\yourSPTfolder\BepInEx\plugins\SwiftXP.SPT.ShowMeTheMoney.QuickSell.dll
```

##### If you use the Fika headless client

There is no need to install this mod on your Fika headless client. In fact, I recommend **not** installing it there.  

If you use [Corter's Mod Sync](https://github.com/c-orter/ModSync), it is recommended to add this mod to your `Exclusions.json`. [See his FAQ on adding sync exclusions](https://github.com/c-orter/ModSync/wiki/Configuration#exclusions).

#### Configuration

Use the BepInEx configurator to adjust mod settings (usually accessible by pressing F12 or F1 in-game):

![BepInEx Plugin Configuration](https://raw.githubusercontent.com/swiftxp-hub/spt-show-me-the-money-quick-sell/refs/heads/main-spt311/Assets/plugin-configuration.png)

#### Remarks

- Quick-selling is currently only possible from the inventory screen.

#### Known compatibility

- [UIFixes](https://forge.sp-tarkov.com/mod/1342/ui-fixes) v4.2.2 or later for SPT 3.11.x by Tyfon  

#### Known issues

- When using [Stash Search](https://forge.sp-tarkov.com/mod/2148/stash-search) by ArchangelWTF, if a search filter is active and you sell items to a trader, the money from the sale may not appear in your stash. To resolve this, clear the search, sell another item to a trader, and the money will then appear.

#### Support for 3.11.x

I will try to continue supporting SPT 3.11.x as long as the SPT team also supports this version. However, support cannot be guaranteed. Bug fixes and minor adjustments are the only guaranteed updates.

{.endtabset}

___

##### Support and feature requests

Please note that all my mods are maintained in my spare time, so support requests may be handled with limited availability.