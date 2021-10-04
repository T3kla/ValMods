# ColorfulSigns for Valheim

Enable enriched text in signs and change default color to white.

| Tag | Effect |
| ----------- | ----------- |
| <b>b</b> | Bold |
| <i>i</i> | Italic |
| <size=50>s</size> | Size |
| <color=#ff0000ff>c</color> | Color |

[List Source](https://docs.unity3d.com/Packages/com.unity.ugui@1.0/manual/StyledText.html)

## Color Library

This functionality allows you to use specific colours without having to remember their hexadecimal value.
Inside the plugin folder there is now a file called "color_library.json". You will see something like:

```json
{
    "red": "#a12d2d",
    "blue": "#292fcf"
}
```

This will turn "<color=red>" to "<color=#a12d2d>" when inputing a sign text value, efectively changing it's color.
You can edit the Json to have your own color palette!

When editing the Json:
- Remember that the last element doesn't have a , at the end
- Don't use special characters, try to stick to alphanumerical values.

## Installation (manual)

If you are installing this manually, do the following

1. Extract the archive into a folder. **Do not extract into the game folder.**
2. Move the contents of `plugins` folder into `<GameDirectory>\Bepinex\plugins`.
3. Run the game.

## Changelog
#### 5.4.801
- New Config setting to set default color
- New Config setting to enable or disable Color Library
- New Config setting to stablish max font size
- Color Library functionality
#### 5.4.800
- Stuff works