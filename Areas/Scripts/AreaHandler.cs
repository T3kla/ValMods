using System.Collections;
using UnityEngine;
using Areas.Containers;
using System.Linq;
using System.Collections.Generic;

namespace Areas
{

    public static class AreaHandler
    {

        public static Area PlayerCurrentArea = new Area { name = "" };
        public static Coroutine PlayerAreaLookupCorou;

        public static void ZoneLookup_Start()
        {

            ZoneLookup_Stop();
            PlayerAreaLookupCorou = Main.Instance.StartCoroutine(ZoneLookup());

        }

        public static void ZoneLookup_Stop()
        {

            if (PlayerAreaLookupCorou == null) return;
            Main.Instance.StopCoroutine(PlayerAreaLookupCorou);
            PlayerAreaLookupCorou = null;

        }

        public static IEnumerator ZoneLookup()
        {

            yield return new WaitForSecondsRealtime(4f);

            Player player = Player.m_localPlayer;

            while (true)
            {

                if (player == null) { Main.GLog.LogWarning($"ZoneLookup Break because player == null"); break; }

                Area newArea = GetArea(player.transform.position.ToXZ()) ?? new Area { name = "" };

                if (newArea.name != PlayerCurrentArea.name)
                {
                    string msg = newArea.name == "" ? $"Exiting {PlayerCurrentArea.name}" : $"Entering {newArea.name}";
                    player.Message(MessageHud.MessageType.Center, msg, 0, null);
                    PlayerCurrentArea = newArea;

                    Main.GLog.LogInfo($"ZoneLookup newArea: {(PlayerCurrentArea != null ? newArea.name : "None")}");
                }

                yield return new WaitForSecondsRealtime(4f);

            }

            PlayerAreaLookupCorou = null;

        }

        public static Area GetArea(Vector2 pos)
        {

            var areas = GetAreas(pos);
            return areas.Count() > 1 ? areas.ElementAt(0) : null;

        }

        public static IEnumerable<Area> GetAreas(Vector2 pos)
        {

            bool CheckDis(float dis, Vector2 rad) => (dis > rad.x && dis < rad.y) ? true : false;

            return from a in Globals.CurrentData.Areas
                   where CheckDis(Vector2.Distance(pos, a.Value.centre.ToVector2()), a.Value.radius.ToVector2())
                   orderby a.Value.layer descending
                   select a.Value;

        }

        public static CTData GetCTDataFromPos(string name, Vector2 pos, out string area, out string cfg)
        {

            area = "";
            cfg = "";

            var areas = GetAreas(pos).ToList();
            if (areas.Count < 0) return null;

            CTData data = null;
            foreach (var a in areas)
                if (Globals.CurrentData.CTMods.ContainsKey(a.cfg))
                    if (Globals.CurrentData.CTMods[a.cfg].TryGetValue(name, out data) || !a.passthrough)
                    { area = a.name; cfg = a.cfg; break; }

            return data;

        }

        public static CTData GetCTDataFromCfg(string name, string cfg)
        {

            CTData data = null;
            foreach (var a in Globals.CurrentData.CTMods)
                if (Globals.CurrentData.CTMods[cfg]?.TryGetValue(name, out data) == true)
                    break;

            return data;

        }

        public static SSData GetSSDataFromPos(int index, Vector2 pos, out string area, out string cfg)
        {

            SSData data = null; area = ""; cfg = "";

            foreach (var a in GetAreas(pos))
                if (Globals.CurrentData.RetrieveSSData(a.cfg, index, out data) || !a.passthrough)
                { area = a.name; cfg = a.cfg; break; }

            return data;

        }

        public static CSData GetCSDataFromPos(string name, Vector2 pos, out string area, out string cfg)
        {

            CSData data = null; area = ""; cfg = "";

            foreach (var a in GetAreas(pos))
                if (Globals.CurrentData.RetrieveCSData(a.cfg, name, out data) || !a.passthrough)
                { area = a.name; cfg = a.cfg; break; }

            return data;

        }

        public static SAData GetSADataFromPos(string name, Vector2 pos, out string area, out string cfg)
        {

            SAData data = null; area = ""; cfg = "";

            foreach (var a in GetAreas(pos))
                if (Globals.CurrentData.RetrieveSAData(a.cfg, name, out data) || !a.passthrough)
                { area = a.name; cfg = a.cfg; break; }

            return data;

        }

    }

}
