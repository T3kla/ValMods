‎<p align="center">[![Logo64](https://user-images.githubusercontent.com/23636548/135311233-240e15b7-73b1-4d2e-b37c-b0b527338504.png)](https://ko-fi.com/tekla)</p>

# ColorfulSigns

Enable Unity's enriched text in signs and change default color to white.

| Tag                        | Effect |
| -------------------------- | ------ |
| <b>b</b>                   | Bold   |
| <i>i</i>                   | Italic |
| <size=50>s</size>          | Size   |
| <color=#ff0000ff>c</color> | Color  |

[Source](https://docs.unity3d.com/Packages/com.unity.ugui@1.0/manual/StyledText.html)

## Color Library

This functionality allows you to use specific colors without having to remember their hexadecimal value.
Inside the plugin folder there is now a file called "color_library.json". You will see something like:

```json
{
    "red": "#a12d2d",
    "blue": "#292fcf"
}
```

This will turn "<color=red>" to "<color=#a12d2d>" when inputting a sign text value, effectively changing its color. You can edit the Json to have your own color palette!

When editing the Json:

-   Remember that the last element doesn't have a comma at the end
-   Don't use special characters, try to stick to alphanumerical values.

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

#### 5.4.1601

-   Optimisations

#### 5.4.1600

-   Updated BepInEx version
-   Updated README.md
-   Removed color_library.json so it doesn't cause accidental overrides. Now it gets generated if no library is found.

#### 5.4.1500

-   Updated BepInEx version

#### 5.4.1100

-   Updated BepInEx version

#### 5.4.902

-   Changed mod Icon

#### 5.4.901

-   Dumping dependencies into plugin folder because having separate folders break the mod for some users

#### 5.4.900

-   Updated BepInEx version

#### 5.4.801

-   New Config setting to set default color
-   New Config setting to enable or disable Color Library
-   New Config setting to stablish max font size
-   Color Library functionality

#### 5.4.800

-   Stuff works
