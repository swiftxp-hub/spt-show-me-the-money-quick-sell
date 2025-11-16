___
As many of you have seen in the recent announcements from the SPT developers on Discord about the changes with Tarkov 1.0, the long-term outlook for SPT and FIKA is uncertain. If you haven’t read their message yet, you can find it here:

https://discord.com/channels/875684761291599922/875706629260197908/1439239841895158022

FIKA will no longer be adding major new features, focusing instead on support and bug fixes for as long as SPT remains viable.

For my mod, I’ll be taking a similar approach: I’ll continue providing maintenance updates and fixing bugs, but I don’t currently plan to develop new large-scale features. The project has grown far beyond what I expected when I started it, and it has genuinely been great to read your feedback and turn it into improvements. Thank you for every comment and conversation on Discord.

The mod’s source code is available on GitHub, so if anyone wants to fork it and build new features on top of it, feel free - you have my blessing.
___

#### What does it do?

In the grand tradition of making life marginally easier (and infinitely more entertaining), this mod allows you to **instantly sell items** from your inventory to traders or the flea market using a configurable keyboard shortcut combined with whichever mouse button you feel most emotionally connected to.  

By default, the universe is arranged thus:

- **Q + Left-click** → Sell to trader (the financially sensible option)  
- **Q + Right-click** → Sell on flea market (the entrepreneurial-but-risky option)  
- **Q + Middle-click** → Sell at *the best price*, as determined by arcane calculation and a faint whiff of magic  

All relevant configuration settings from the venerable **Show Me The Money** mod are automatically applied, as if by an invisible accountant lurking in the background.

If you’re using [UIFixes](https://forge.sp-tarkov.com/mod/1342/ui-fixes) v5.0.2 or later (Tyfon’s delightful gift to humanity), you can even select **multiple items** and sell them all at once. When selling on the flea market, items of the same type are automatically bundled. Like socks in a dryer, except these actually stay together.

#### Requirements

- You must possess the mighty **[Show Me The Money](https://forge.sp-tarkov.com/mod/2299/show-me-the-money)** SPT-mod, version **2.2.0 or newer**, without which this quick-sell addon would spend its days staring listlessly into the void.

#### Installation

Extract the contents of the `.zip` or `.7z` file into your SPT directory with all the elegance and grace of a caffeinated space hamster.

After that, you should find exactly this file in your SPT installation:
```
- C:\yourSPTfolder\BepInEx\plugins\com.swiftxp.spt.showmethemoney.quicksell\SwiftXP.SPT.ShowMeTheMoney.QuickSell.Client.dll
```

##### If you use the Fika headless client

Splendidly simple advice: **do not install this mod there.** Nothing good will come from doing so. The client will ignore it, you’ll think it’s broken, and the universe will sigh. If you’re using [Corter’s Mod Sync](https://github.com/c-orter/ModSync), please add this mod to your `Exclusions.json`.

#### Configuration

Adjust all settings through the BepInEx configurator (summonable through **F12** or **F1**, assuming the stars are aligned and your keyboard cooperates):

![BepInEx Plugin Configuration](https://raw.githubusercontent.com/swiftxp-hub/spt-show-me-the-money-quick-sell/refs/heads/main/Assets/plugin-configuration.png)

#### Remarks

- Quick-selling is currently possible **only from the inventory screen**, likely because attempting to do it mid-raid would tear a hole in the fabric of space-time (and also be wildly unbalanced).

#### Known compatibility

- [UIFixes](https://forge.sp-tarkov.com/mod/1342/ui-fixes) v5.0.2 or later for SPT 4.0.x by Tyfon  

---

##### Support and feature requests

All support and requests for new features will be handled as time permits, often between cups of tea and existential reflection. Please be patient. The universe is large, confusing, and full of bugs.