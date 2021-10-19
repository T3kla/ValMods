‎<p align="center">[![Logo64](https://user-images.githubusercontent.com/23636548/135311233-240e15b7-73b1-4d2e-b37c-b0b527338504.png)](https://ko-fi.com/tekla)</p>

# QoL Pins

Automations, colorization and automatic placement of minimap pins.

## Death Pin Automations

In the first section of the configurations you can find the toggles for this two features.

-   Automatic removal off death pins when retrieving Tombstone
-   Non generation of death pin when dying with an empty inventory

## Pin Colorization

In the second section of the configurations you can find options to change each ping coloration via string with hexadecimal format.

This colors will only be visible to you.

## Automatic Pin Placement

In the third section of the configurations you can find the fields corresponding to the default pin type and text that will be shown when certain automated pin is triggered. It works like this:

```
Pin types available: `Fireplace` `House` `Hammer` `Ball` `Cave`

------------------------------------------------------------------------------------
PinType:Message (Example: Tin field)
    'Ball:Tin'  => Place Ball pin with the text 'Tin'
    'Ball:T'    => Place Ball pin with the text 'T'
    'Hammer:'   => Place Hammer pin with no text
    'Hammer'    => Deactivate Tin automated pin placement
    ':Tin'      => Deactivate Tin automated pin placement
    ':'         => Deactivate Tin automated pin placement
    ''          => Deactivate Tin automated pin placement

------------------------------------------------------------------------------------
PinType:Message (Example: Dungeon field)
    'Cave:'         => Place Cave pin with automatic naming of the dungeon
    'Cave:Dungeon'  => Place Cave pin with the text 'Dungeon' for every dungeon type
```

<!-- 35e644 green - e03f3f red - e6c035 yellow -->
<!-- <span style="color:??????">**---**</span>   -->

|   Group   |            Availability             |        Add Pin         |     Remove Pin     |
| :-------: | :---------------------------------: | :--------------------: | :----------------: |
|   Ores    |         Tin, Copper, Silver         |    Ore takes damage    | Ore gets destroyed |
| Dungeons  | ForestCrypt, SunkenCrypt, TrollCave | Interact with entrance |        None        |
| Locations |               **---**               |                        |                    |
| Leviathan |               **---**               |                        |                    |
| Spawners  |               **---**               |                        |                    |

## Stuff

-   **Bugs?** Report them [here](https://github.com/T3kla/ValMods/issues).
-   **Features?** Propose them [here](https://github.com/T3kla/ValMods/issues).
-   **Contact?** Discord: Tekla#1012 or tag me as @Tekla in [Valheim Modding](https://discord.gg/RBq2mzeu4z)

## Installation (manual)

If you are installing this manually, do the following

1. Extract the archive into a folder. **Do not extract into the game folder.**
2. Move the contents of `plugins` folder into `<GameDirectory>\Bepinex\plugins`.
3. Run the game.

## Changelog

#### 5.4.1604

-   Automatic Pin Placement for dungeons: ForestCrypt, SunkenCrypt, TrollCave

#### 5.4.1603

-   Automatic Pin Placement for ores: Tin, Copper, Silver

#### 5.4.1602

-   Fix: ColorLibrary not updating when loading plugin

#### 5.4.1601

-   Jotunn implementation to allow in-game color changes through their mod settings panel
-   Refreshing ColorLibrary on config change instead of znet awake
-   Fix: naming one of the configs

#### 5.4.1600

-   Automatic removal off death pins when retrieving Tombstone
-   Not generating death pin when dying with an empty inventory
-   Configurable pin colors
