using System.Collections.Generic;
using System.Linq;
using Jotunn.Entities;
using Jotunn.Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DungeonReset
{
    public static class CommandHandler
    {
        public static void Awake()
        {
            CommandManager.Instance.AddConsoleCommand(new SpawnCritter());
        }
    }

    public class SpawnCritter : ConsoleCommand
    {
        public override string Name => "rdforce";
        public override string Help => "Force reset closest dungeon in (X,Z) distance.";

        public override void Run(string[] args)
        {
            if (!SynchronizationManager.Instance.PlayerIsAdmin) { Console.instance.Print($"You are not admin!"); return; }

            List<DungeonGenerator> dungeons = new();

            foreach (var go in SceneManager.GetActiveScene().GetRootGameObjects())
            {
                var dungeon = go.GetComponent<DungeonGenerator>();
                if (dungeon)
                    dungeons.Add(dungeon);
            }

            dungeons.OrderByDescending(ctx => Utils.DistanceXZ(Player.m_localPlayer.transform.position, ctx.transform.position));

            var closest = dungeons.FirstOrDefault();
            if (closest == null) { Console.instance.Print($"No dungeon found!"); return; }

            Dungeons.Reset(closest);
        }
    }
}
