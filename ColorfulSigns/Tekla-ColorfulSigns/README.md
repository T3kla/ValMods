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

- Remember that the last element doesn't have a comma at the end
- Don't use special characters, try to stick to alphanumerical values.

## Issues & Suggestions

Please report issues and suggestions [here](https://github.com/T3kla/ValMods/issues) or tag me as `@Tekla` in [Valheim Modding](https://discord.gg/RBq2mzeu4z) Discord server. (use the correct channel or Jessica will get mad)

## Installation (manual)

If you are installing this manually, do the following

1. Extract the archive into a folder. **Do not extract into the game folder.**
2. Move the contents of `plugins` folder into `<GameDirectory>\Bepinex\plugins`.
3. Run the game.

![Logo64](https://user-images.githubusercontent.com/23636548/112306898-a1ac1f00-8ca0-11eb-8b3e-90e73dc7bad2.png)

## Changelog

#### 5.4.1500

- BepInEx 5.4.1500

#### 5.4.1100

- Updated BepInEx version

#### 5.4.902

- Changed mod Icon

#### 5.4.901

- Dumping dependencies into plugin folder because having separate folders break the mod for some users

#### 5.4.900

- Updated Bepinex version

#### 5.4.801

- New Config setting to set default color
- New Config setting to enable or disable Color Library
- New Config setting to stablish max font size
- Color Library functionality

#### 5.4.800

- Stuff works
