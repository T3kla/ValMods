using Jotunn.Entities;
using Jotunn.Managers;

namespace DungeonReset
{
    public static class Commands
    {
        public static void Awake()
        {
            CommandManager.Instance.AddConsoleCommand(new ResetClosest());
            CommandManager.Instance.AddConsoleCommand(new ResetLoaded());
        }
    }

    public class ResetClosest : ConsoleCommand
    {
        public override string Name => "dungeonresetclosest";
        public override string Help => "Force reset closest dungeon in (X,Z) distance.";

        public override void Run(string[] args)
        {
            if (!SynchronizationManager.Instance.PlayerIsAdmin)
            {
                Console.instance.Print($"You are not an admin!");
                return;
            }

            var closest = GetClosest();

            if (closest == null)
            {
                Console.instance.Print($"No dungeon found!");
                return;
            }

            Console.instance.Print($"Forcing regeneration of Dungeon '{closest.GetCleanName()}', resetting now!");
            Dungeons.Reset(closest);
        }

        private static DungeonGenerator GetClosest()
        {
            DungeonGenerator closest = null;
            float closestDis = float.MaxValue;

            foreach (var timer in Timer.Each)
            {
                if (Player.m_localPlayer == null || timer.dungeon == null)
                    continue;

                var dis = Utils.DistanceXZ(Player.m_localPlayer.transform.position, timer.dungeon.transform.position);
                if (closest == null || dis < closestDis)
                {
                    closest = timer.dungeon;
                    closestDis = dis;
                }
            }

            return closest;
        }
    }

    public class ResetLoaded : ConsoleCommand
    {
        public override string Name => "dungeonresetloaded";
        public override string Help => "Force reset all loaded dungeons.";

        public override void Run(string[] args)
        {
            if (!SynchronizationManager.Instance.PlayerIsAdmin)
            {
                Console.instance.Print($"You are not an admin!");
                return;
            }

            int count = 0;
            foreach (var timer in Timer.Each)
            {
                if (Player.m_localPlayer == null || timer.dungeon == null)
                    continue;

                ++count;
                Console.instance.Print($"Forcing regeneration of Dungeon '{timer.dungeon.GetCleanName()}', resetting now!");
                Dungeons.Reset(timer.dungeon);
            }

            if (count < 1)
                Console.instance.Print($"Could't find loaded dungeons!");
            else
                Console.instance.Print($"Forced a total of {count} dungeons to reset!");
        }
    }
}
