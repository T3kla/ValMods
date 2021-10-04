‎<p align="center">[![Logo64](https://user-images.githubusercontent.com/23636548/135311233-240e15b7-73b1-4d2e-b37c-b0b527338504.png)](https://ko-fi.com/tekla)</p>

# DungeonReset

Allows manual and automatic reset of dungeons.

The first time you add this mod it will reset any dungeon that gets loaded because they lack date information.
This info gets added after the initial reset and will be used alongside the "Interval" config to calculate the next reset.

Player structures inside dungeons should not get deleted.

Resources, monsters and spawners should reappear.

All time calculations are done with real time, not game time.

## Commands

-   **dungeonresetclosest**: reset closest dungeon

-   **dungeonresetloaded**: reset all loaded dungeons

## Configs

-   **Interval**: reset cooldown, defaults to 23h

-   **AllowedThemes**: types of dungeon that are allowed to be reset

-   **PlayerProtection**: interrupts reset if players are inside the dungeon

-   **PlayerProtectionInterval**: reset cooldown when reset was interrupted via PlayerProtection, defaults to 10m

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

-   Fix: NullReferenceException when exiting game from play scene

#### 5.4.1603

-   Updated README.md

#### 5.4.1602

-   Fix: command "drforce" not working
-   Renamed command "drforce" to "dungeonresetclosest"
-   Added command "dungeonresetloaded" to reset all loaded dungeons

#### 5.4.1601

-   Set Jotunn has hard dependency for BepInEx
-   Enforce mod ownership and version

#### 5.4.1600

-   Automatic reset by intervals
-   Player protection
-   Force reset command "drforce"
