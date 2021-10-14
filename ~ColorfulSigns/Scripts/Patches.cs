using HarmonyLib;
using Jotunn;
using UnityEngine.UI;

namespace ColorfulSigns
{
    [HarmonyPatch]
    public static class Patches
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(Sign), nameof(Sign.Awake))]
        public static void Sing_Awake(Sign __instance)
        {
            var def = Configs.DefaultTextColor.Value;
            var color = (string.IsNullOrEmpty(def) ? "#ededed" : def).ToColor();

            __instance.m_textWidget.supportRichText = true;
            __instance.m_textWidget.material = null;
            __instance.m_characterLimit = 999;
            __instance.m_textWidget.color = color;
            __instance.m_textWidget.resizeTextMaxSize = Configs.MaxFontSize.Value;

            if (!Configs.EnableOutline.Value)
                return;

            var o = __instance.m_textWidget.gameObject.GetOrAddComponent<Outline>();
            o.effectColor = Configs.OutlineColor.Value.ToColor();
            o.effectDistance = new UnityEngine.Vector2(Configs.OutlineSize.Value, Configs.OutlineSize.Value);
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(Sign), nameof(Sign.SetText))]
        public static void Sing_SetText(Sign __instance, ref string text)
        {
            if (string.IsNullOrEmpty(text)
            || !Configs.UseLibrary.Value)
                return;

            foreach (var item in ColorfulSigns.Library.Keys)
                if (text.Contains($"<color={item}>"))
                    text = text.Replace($"<color={item}>", $"<color={ColorfulSigns.Library[item]}>");
        }
    }
}
