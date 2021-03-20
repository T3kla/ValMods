using UnityEngine;
using HarmonyLib;

namespace ColorfulSigns
{

    [HarmonyPatch]
    public static class Patches
    {

        [HarmonyPrefix]
        [HarmonyPatch(typeof(Sign), nameof(Sign.Awake))]
        public static bool Sing_Awake(Sign __instance)
        {
            Color def, custom; ColorUtility.TryParseHtmlString("#ededed", out def);

            __instance.m_textWidget.supportRichText = true;
            __instance.m_textWidget.material = null;
            __instance.m_characterLimit = 999;
            __instance.m_textWidget.color = ColorUtility.TryParseHtmlString(Globals.configDefColor.Value, out custom) ? custom : def;
            __instance.m_textWidget.resizeTextMaxSize = Globals.configMaxFontSize.Value;

            return true;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(Sign), nameof(Sign.SetText))]
        public static bool Sing_SetText(Sign __instance, ref string text)
        {
            if (Globals.configEnableColorLibrary.Value)
                foreach (var item in Globals.colorLibrary.Keys)
                    if (text.Contains($"<color={item}>"))
                    {
                        string newStr = text.Replace($"<color={item}>", $"<color={Globals.colorLibrary[item]}>");
                        text = newStr;
                    }

            return true;
        }

    }

}