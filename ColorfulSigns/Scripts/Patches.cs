using HarmonyLib;
using UnityEngine;

namespace ColorfulSigns
{
    [HarmonyPatch]
    public static class Patches
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(Sign), nameof(Sign.Awake))]
        public static bool Sing_Awake(Sign __instance)
        {
            Color mDef = "#ededed".ToColor();
            Color cDef = Configs.DefaultColor.Value.ToColor();

            if (__instance == null)
                return true;

            __instance.m_textWidget.supportRichText = true;
            __instance.m_textWidget.material = null;
            __instance.m_characterLimit = 999;
            __instance.m_textWidget.color = string.IsNullOrEmpty(Configs.DefaultColor.Value) ? mDef : cDef;
            __instance.m_textWidget.resizeTextMaxSize = Configs.MaxFontSize.Value;

            return true;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(Sign), nameof(Sign.SetText))]
        public static bool Sing_SetText(Sign __instance, ref string text)
        {
            if (__instance == null)
                return true;

            var str = text;

            if (string.IsNullOrEmpty(str))
                str = "";

            if (Configs.EnableColorLibrary.Value)
                foreach (var item in ColorfulSigns.Library.Keys)
                    if (str.Contains($"<color={item}>"))
                        str = str.Replace($"<color={item}>", $"<color={ColorfulSigns.Library[item]}>");

            text = str;

            return true;
        }
    }
}
