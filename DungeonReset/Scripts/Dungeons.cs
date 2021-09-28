using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DungeonReset
{
    public static class Dungeons
    {
        private class Timer
        {
            public static readonly List<Timer> Each = new();

            public DungeonGenerator dungeon;
            public float delay;
            public float interval;
            private Coroutine task = null;

            private void Create()
            {
                task = Main.Instance.StartCoroutine(Cycle(this));
                Each.Find(ctx => ctx.dungeon == this.dungeon)?.Destroy();
                Each.Add(this);
            }

            public void Destroy()
            {
                Main.Instance.StopCoroutine(task);
                Each.Remove(this);
            }

            public Timer(DungeonGenerator dungeon, float delay, float interval)
            {
                this.dungeon = dungeon;
                this.delay = delay;
                this.interval = interval;
                Create();
            }
        }

        public static void OnDungeonLoad(DungeonGenerator dungeon)
        {
            Main.Log.LogInfo($"Dungeon '{dungeon.GetCleanName()}' was captured!");
            Schedule(dungeon);
        }

        private static void Schedule(DungeonGenerator dungeon, float delay = 0f)
        {
            var rand = UnityEngine.Random.Range(1f, 5f);
            var remaining = dungeon.Remaining();
            new Timer(dungeon, delay + remaining + rand, Global.Config.DungeonResetInterval.Value);
        }

        private static IEnumerator Cycle(Timer timer)
        {
            yield return new WaitForSeconds(timer.delay);
            while (true)
            {
                if (ValidateReset(timer))
                    Reset(timer.dungeon);
                yield return null;
                yield return new WaitForSeconds(timer.interval);
            }
        }

        private static bool ValidateReset(Timer timer)
        {
            if (timer.dungeon == null)
            {
                Main.Log.LogWarning($"Dungeon 'null' couldn't regenerate because it was out of bounds!");
                timer.Destroy();
                return false;
            }

            if (!timer.dungeon.IsOverdue())
            {
                Main.Log.LogWarning($"Dungeon '{timer.dungeon.GetCleanName()}' has already been reset!");
                Schedule(timer.dungeon);
                return false;
            }

            var pp = ZNet.instance.GetPlayerList();
            var pl = Player.GetAllPlayers();

            if (Global.Config.DungeonResetPlayerProtection.Value)
            {
                Bounds bounds = new(timer.dungeon.transform.position, timer.dungeon.m_zoneSize * 2f);
                if (pp.Any(a => bounds.Contains(a.m_position)) && pl.Any(a => bounds.Contains(a.transform.position)))
                {
                    Main.Log.LogWarning($"Dungeon '{timer.dungeon.GetCleanName()}' couldn't regenerate because a player was found inside!");
                    Schedule(timer.dungeon, Global.Config.DungeonResetPlayerProtectionInterval.Value);
                    return false;
                }
            }

            return true;
        }

        public static void Reset(DungeonGenerator dungeon, Bounds? bounds = null)
        {
            Bounds b = bounds ?? new Bounds(dungeon.transform.position, dungeon.m_zoneSize * 2f);

            foreach (var go in SceneManager.GetActiveScene().GetRootGameObjects())
            {
                if (go.transform.position.y < 4000f
                || !b.Contains(go.transform.position)
                || go.GetComponent<Player>()
                || go.GetComponent<TombStone>()
                || go == dungeon.gameObject)
                    continue;

                Piece piece = go.GetComponent<Piece>();
                if ((piece != null ? piece.GetCreator() : 0L) != 0L)
                    continue;

                ZNetView netView = go.GetComponent<ZNetView>();
                if (netView != null)
                {
                    netView.GetZDO()?.SetOwner(ZDOMan.instance.GetMyID());
                    netView.Destroy();
                }
                else
                {
                    ZNetScene.instance.Destroy(go);
                }
            }

            dungeon.Generate(ZoneSystem.SpawnMode.Full);
            dungeon.SetLastReset(DateTimeOffset.Now);
            Main.Log.LogInfo($"Dungeon '{dungeon.GetCleanName()}' was regenerated successfully!");
        }

        public static void ClearTimers()
        {
            var copy = new List<Timer>(Timer.Each);
            foreach (var timer in copy)
                timer.Destroy();
            copy.Clear();
        }
    }
}
