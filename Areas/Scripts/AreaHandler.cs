using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DifficultyAreas.TJson;
using DifficultyAreas.Containers;

namespace DifficultyAreas
{

    public static class AreaHandler
    {

        public static Area PlayerCurrentArea;
        public static Coroutine PlayerZoneLookupCorou;

        public static void Initialize()
        {
            PlayerCurrentArea = new Area() { id = "" };
            Global.Areas = Serialization.DeserializeFile<List<Area>>(Global.Path.AreaMap);
            Global.AreaCfgs = Serialization.DeserializeFile<List<AreaCfg>>(Global.Path.AreaCfgs);
            foreach (var item in Global.Areas)
                item.cfg = GetAreaCfg(item.cfg_id);
        }

        public static IEnumerator ZoneLookup(Player player)
        {
            while (true)
            {
                if (player == null) break;
                
                Area newArea = GetArea(player.transform.position);

                if (newArea.id != PlayerCurrentArea.id)
                {
                    string msg = newArea.id == "" ? $"Exiting {PlayerCurrentArea.display_name}" : $"Entering {newArea.display_name}";

                    Player.m_localPlayer.Message(MessageHud.MessageType.Center, msg, 0, null);
                    PlayerCurrentArea = newArea;

                    Debug.Log($"[Areas] ZoneLookup newArea: {(PlayerCurrentArea != null ? newArea.display_name : "None")}");
                }

                yield return new WaitForSecondsRealtime(4f);
            }
        }

        private static Area GetArea(Vector3 playerPos)
        {
            Vector2 playerPos2D = new Vector2(playerPos.x, playerPos.z);
            int areaIndex = -1;
            int areaLayer = -1;

            for (int i = 0; i < Global.Areas.Count; i++)
            {
                Area a = Global.Areas[i];
                if (a.cfg == null) continue;

                float dis = Vector2.Distance(playerPos2D, a.center);
                if (dis > a.inner_radious && dis < a.outter_radious && a.cfg.layer > areaLayer)
                {
                    areaIndex = i;
                    areaLayer = a.cfg.layer;
                }
            }

            return areaIndex > -1 ? Global.Areas[areaIndex] : new Area() { id = "" };
        }

        private static AreaCfg GetAreaCfg(string configId)
        {
            return Global.AreaCfgs.Find(a => a.id == configId) ?? null;
        }

    }

}
