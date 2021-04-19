using System;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;
using Areas.Containers;
using HarmonyLib;
using System.Reflection;
using System.Linq;
using System.Collections;

namespace Areas
{

    public static class DungeonHandler
    {

        public class CoroutineBox { public IEnumerator task; }

        public static List<IEnumerator> Tasks = new List<IEnumerator>();

        public static void Regenerate(DungeonGenerator dg)
        {

            string msg = dg != null ? RegenerateInternal(dg) : "OutOfBounds";
            long? regenAtSecond = null;

            switch (msg)
            {
                case "RegenSuccess":
                    Main.GLog.LogInfo($"Regen output: Dungeon \"{dg.GetCleanName()}\" was regenerated successfully!");
                    regenAtSecond = (long)ZNet.instance.GetTimeSeconds() + Globals.Config.DungeonRegenCooldown.Value * 60;
                    break;
                case "PlayerInside":
                    Main.GLog.LogWarning($"Regen output: Dungeon \"{dg.GetCleanName()}\" coudln't regenerate because a player was found inside!");
                    regenAtSecond = (long)ZNet.instance.GetTimeSeconds() + Globals.Config.DungeonRegenCooldown.Value * 60;
                    break;
                case "AlreadyRegenerated":
                    Main.GLog.LogInfo($"Regen output: Dungeon \"{dg.GetCleanName()}\" was already regnerated by another instance!");
                    regenAtSecond = dg.GetRegenAtSecond();
                    break;
                case "OutOfBounds":
                    Main.GLog.LogWarning($"Regen output: Dungeon \"null\" coudln't regenerate because it was out of bounds!");
                    regenAtSecond = null;
                    break;
                default:
                    Main.GLog.LogWarning($"Regen output: Dungeon \"null\" coudln't regenerate because it was out of bounds!");
                    break;
            }

            if (regenAtSecond.HasValue && dg != null)
            {
                ZDO dungeonZDO = dg.GetComponent<ZNetView>().GetZDO();
                dungeonZDO.Set("Areas RegenAtSecond", regenAtSecond.Value);

                Task_Schedule(dg);
            }

        }

        private static string RegenerateInternal(DungeonGenerator dg)
        {

            if (dg == null) { return "OutOfBounds"; }
            if (dg.GetRegenRemainder() > 0L) { return "AlreadyRegenerated"; }

            Bounds bounds = new Bounds(dg.transform.position, dg.m_zoneSize * 2f);
            if (ZNet.instance.GetPlayerList().Any(a => bounds.Contains(a.m_position))) { return "PlayerInside"; }
            if (bounds.Contains(Player.m_localPlayer.transform.position)) { return "PlayerInside"; }

            GameObject netSceneRoot = ZNetScene.instance.m_netSceneRoot;

            for (int i = 0; i < netSceneRoot.transform.childCount; i++)
            {
                GameObject obj = netSceneRoot.transform.GetChild(i).gameObject;

                if (obj.transform.position.y < 4000f) continue;
                if (!bounds.Contains(obj.transform.position)) continue;
                if (obj == dg.gameObject) continue;
                if (obj.GetComponent<Player>()) continue;
                if (obj.GetComponent<TombStone>()) continue;

                Piece piece = obj.GetComponent<Piece>();
                if ((piece != null ? piece.GetCreator() : 0L) != 0L) continue;

                ZNetView netView = obj.GetComponent<ZNetView>();
                if (netView != null)
                {
                    netView.GetZDO()?.SetOwner(ZDOMan.instance.GetMyID());
                    netView.Destroy();
                }
                else
                {
                    UnityEngine.Object.Destroy(obj);
                }
            }

            dg.Generate(ZoneSystem.SpawnMode.Full);

            return "RegenSuccess";

        }

        public static void Task_Schedule(DungeonGenerator dungeon, long remainder = 0L)
        {

            if (dungeon == null) return;

            long trueRem = remainder > 0 ? remainder : dungeon.GetRegenRemainder();

            string trueRemStr = string.Format("{0}:{1}", Mathf.Floor(trueRem / 60).ToString("00"), (trueRem % 60).ToString("00"));
            Main.GLog.LogInfo($"Sheduling Regen Task \"{dungeon.GetCleanName()}\" regeneration in \"{trueRemStr}\"");

            CoroutineBox box = new CoroutineBox();
            IEnumerator task = Task(dungeon, trueRem, box);
            box.task = task;
            Tasks.Add(task);
            Main.Instance.StartCoroutine(task);

        }

        public static void Task_Deschedule(IEnumerator task)
        {

            if (task != null)
            {
                Tasks.Remove(task);
                Main.Instance.StopCoroutine(task);
            }

        }

        public static void Task_DescheduleAll()
        {

            Main.GLog.LogInfo($"Descheduling all Dungeon Regeneration tasks");

            foreach (var task in Tasks)
                if (task != null)
                    Main.Instance.StopCoroutine(task);

            Tasks.Clear();

        }

        public static IEnumerator Task(DungeonGenerator dungeon, long remainder, CoroutineBox box)
        {

            float wait = remainder + UnityEngine.Random.Range(1f, 5f);
            yield return new WaitForSeconds(wait);

            Regenerate(dungeon);
            Task_Deschedule(box.task);

        }

    }

}
