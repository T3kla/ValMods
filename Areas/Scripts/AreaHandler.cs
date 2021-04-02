using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;
using Areas.TJson;
using Areas.Containers;

namespace Areas
{

    public static class AreaHandler
    {

        public static Area PlayerCurrentArea;
        public static Coroutine PlayerAreaLookupCorou;

        public static void Initialize()
        {

            PlayerCurrentArea = new Area() { id = "" };

            Globals.Areas = Serialization.DeserializeFile<List<Area>>(Globals.Path.Areas);
            Globals.Critters = JObject.Parse(File.ReadAllText(Globals.Path.Critters));
            Globals.SS_Data = JObject.Parse(File.ReadAllText(Globals.Path.SS_Data));

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

                yield return new WaitForSecondsRealtime(5f);
            }

        }

        public static Area GetArea(Vector3 pos3D)
        {

            Vector2 pos = new Vector2(pos3D.x, pos3D.z);

            int areaIndex = -1;
            int areaLayer = -1;

            for (int i = 0; i < Globals.Areas.Count; i++)
            {
                Area a = Globals.Areas[i];

                float dis = Vector2.Distance(pos, a.centre);
                if (dis > a.inner_radious && dis < a.outter_radious && a.layer > areaLayer)
                {
                    areaIndex = i;
                    areaLayer = a.layer;
                }
            }

            return areaIndex > -1 ? Globals.Areas[areaIndex] : new Area() { id = "" };

        }

    }

}
