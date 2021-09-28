using System.Collections.Generic;
using System.Linq;
using Areas.Containers;

namespace Areas
{

    public struct LocBlock
    {
        public string language, token, value;

        public LocBlock(string language, string token, string value)
        {
            this.language = language;
            this.token = token;
            this.value = value;
        }
    }

    public static class VariantsHandler
    {
        private static HashSet<LocBlock> LocalizationData = new();

        public static void OnDataLoaded() => GenerateVariants();
        public static void OnDataReset() => ClearVariants();

        private static void GenerateVariants()
        {
            if (Global.CurrentData.VAMods == null) return;
            if (Global.CurrentData.VAMods.Count < 1) return;

            Main.Log.LogInfo($"Generating Variants");

            // Generate Variants area
            Area area = new()
            {
                name = "variants",
                cfg = "variants",
                layer = 0,
                passthrough = true,
                centre = new float[] { 0, 0 },
                radius = new float[] { 0, 1000000 },
            };
            Global.CurrentData.Areas.Add("variants", area);

            // Add Tokens
            foreach (var variant in Global.CurrentData.VAMods)
                if (variant.Value.localization != null)
                    variant.Value.localization?.ForEach(a => LocalizationData.Add(new LocBlock(a[0], $"enemy_{variant.Key}", a[1])));

            Global.CurrentData.CTMods.Add("variants", Global.CurrentData.VAMods.ToDictionary(a => a.Key, a => a.Value as CTData));
            PushLocalizationData(Localization.instance.GetSelectedLanguage());
        }

        private static void ClearVariants()
            => PullLocalizationData(Localization.instance.GetSelectedLanguage());

        public static void PushLocalizationData(string language)
        {
            var stuff = from a in LocalizationData
                        where a.language == language
                        select a;

            foreach (var item in stuff)
                if (Localization.instance.m_translations.ContainsKey(item.token))
                    Localization.instance.m_translations[item.token] = item.value;
                else
                    Localization.instance.m_translations.Add(item.token, item.value);
        }

        public static void PullLocalizationData(string language)
        {
            var stuff = from a in LocalizationData
                        where a.language == language
                        select a.token;

            Localization.instance.m_translations.RemoveAll(stuff);
        }

        public static void OnLanguageChanged(string language)
            => PushLocalizationData(language);

        public static string FindOriginal(string ctName)
            => Global.CurrentData.VAMods.TryGetValue(ctName, out var vaData) ? vaData.original : null;
    }
}
