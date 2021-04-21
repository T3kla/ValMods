using System.Collections;
using UnityEngine;
using Areas.Containers;

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

                Area newArea = GetArea(player.transform.position) ?? new Area { name = "" };

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

        public static Area GetArea(Vector3 pos3D)
        {

            Vector2 pos = new Vector2(pos3D.x, pos3D.z);
            string area = null;
            int layer = -1;

            foreach (var a in Globals.Areas)
            {
                float dis = Vector2.Distance(pos, a.Value.centre);

                if (dis < a.Value.radius.x || dis > a.Value.radius.z) continue;
                if (a.Value.layer <= layer) continue;

                area = a.Key;
                layer = a.Value.layer;
            }

            return area != null ? Globals.Areas[area] : null;

        }

    }

}
