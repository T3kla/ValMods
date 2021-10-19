using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DungeonReset
{
    public static class Dungeons
    {
        private static float interval => Configs.Interval.Value;
        private static float playerProtectionInterval => Configs.PlayerProtectionInterval.Value;

        public static void OnDungeonLoad(DungeonGenerator dungeon)
        {
            Main.Log.LogInfo($"Dungeon '{dungeon.GetCleanName()}' was captured!\n");
            UnscheduleAllNotLoaded();
            Schedule(dungeon);
        }

        public static void Schedule(DungeonGenerator dungeon, float delay = 0f)
        {
            var rand = UnityEngine.Random.Range(1f, 5f);
            var remaining = dungeon.Remaining();
            var finalDelay = delay + remaining + rand;
            new Timer(dungeon, finalDelay, interval);
            Main.Log.LogInfo($"Dungeon '{dungeon.GetCleanName()}' was scheduled for reset in {finalDelay:F0} seconds!\n");
        }

        public static void UnscheduleAllNotLoaded()
        {
            var copy = new List<Timer>(Timer.Each);
            foreach (var timer in copy)
                if (timer.dungeon == null)
                    timer.Destroy();
        }

        public static void UnscheduleAll()
        {
            var copy = new List<Timer>(Timer.Each);
            foreach (var timer in copy)
                timer.Destroy();
        }

        public static bool Validate(Timer timer)
            => ValidateOutOfRange(timer)
            && ValidateTooEarly(timer)
            && ValidateOverdue(timer)
            && ValidatePlayerProtection(timer);

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
            Main.Log.LogInfo($"Dungeon '{dungeon.GetCleanName()}' was regenerated successfully!\n");
        }

        #region Validators
        private static bool ValidateOutOfRange(Timer timer)
        {
            if (timer.dungeon != null)
                return true;

            Main.Log.LogWarning($"Dungeon 'null' couldn't regenerate because it was out of bounds!\n");
            timer.Destroy();
            return false;
        }

        private static bool ValidateTooEarly(Timer timer)
        {
            if (Player.m_localPlayer != null)
                return true;

            Main.Log.LogWarning($"Dungeon '{timer.dungeon.GetCleanName()}' wanted to reset too early!\n");
            Schedule(timer.dungeon, 10f);
            return false;
        }

        private static bool ValidateOverdue(Timer timer)
        {
            if (timer.dungeon.IsOverdue())
                return true;

            Main.Log.LogWarning($"Dungeon '{timer.dungeon.GetCleanName()}' has already been reset!\n");
            Schedule(timer.dungeon);
            return false;
        }

        private static bool ValidatePlayerProtection(Timer timer)
        {
            if (!Configs.PlayerProtection.Value)
                return true;

            Bounds bounds = new(timer.dungeon.transform.position, timer.dungeon.m_zoneSize * 2f);
            foreach (var player in Player.GetAllPlayers())
                if (bounds.Contains(player.transform.position))
                {
                    Main.Log.LogWarning($"Dungeon '{timer.dungeon.GetCleanName()}' couldn't regenerate because a player was found inside!\n");
                    Schedule(timer.dungeon, playerProtectionInterval);
                    return false;
                }

            return true;
        }
        #endregion
    }
}
