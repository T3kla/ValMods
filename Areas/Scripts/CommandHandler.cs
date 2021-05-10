using UnityEngine;
using Jotunn.Managers;
using Jotunn.Entities;

namespace Areas
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

        public override string Name => "a_spawn_critter";
        public override string Help => "Spawn critter as: a_spawn_critter [critterName] -c [cfg] -p [x,y,z]";

        public override void Run(string[] args)
        {

            if (args.Length < 1) return;
            string ctrName = args[0];

            GameObject prefab = PrefabManager.Instance.GetPrefab(ctrName);
            if (prefab == null || prefab.GetComponent<Character>() == null || prefab.GetComponent<Character>()?.IsPlayer() == true)
            {
                Console.instance.Print($"Couldn't find critter with name \"{ctrName}\"");
                return;
            }

            string cfg = null;
            Vector3? pos = Player.m_localPlayer.transform.position + Player.m_localPlayer.transform.forward * 5;

            if (args.Length > 1)
                for (int i = 1; i < args.Length; i++)
                    switch (args[i])
                    {
                        case "-c": cfg = args[i + 1]; break;
                        case "-p":
                            var array = args[i + 1].Split(',');
                            pos = new Vector3(int.Parse(array[0]), int.Parse(array[1]), int.Parse(array[2]));
                            break;
                        default: break;
                    }

            Character newCritter = CritterHandler.Spawn(prefab, pos.Value, Quaternion.identity);
            CritterHandler.ModifyGiven(newCritter, cfg);

        }

    }

    public class SpawnSpawner : ConsoleCommand
    {

        public override string Name => "a_set_spawner";
        public override string Help => "Spawn a creature spawner as: a_set_spawner [critterName] -p [x,y,z]";

        public override void Run(string[] args)
        {

            if (args.Length < 1) return;
            string ctrName = args[0];

            GameObject prefab = PrefabManager.Instance.GetPrefab("Spawner_Boar");
            if (prefab == null || prefab.GetComponent<CreatureSpawner>() == null) return;

            Vector3? pos = Player.m_localPlayer.transform.position;

            if (args.Length > 1)
                for (int i = 1; i < args.Length; i++)
                    switch (args[i])
                    {
                        case "-p":
                            var array = args[i + 1].Split(',');
                            pos = new Vector3(int.Parse(array[0]), int.Parse(array[1]), int.Parse(array[2]));
                            break;
                        default: break;
                    }

            GameObject newSpawner = GameObject.Instantiate(prefab, pos.Value, Quaternion.identity);
            CreatureSpawner spawner = newSpawner.GetComponent<CreatureSpawner>();

            spawner.m_respawnTimeMinuts




            Character newCritter = CritterHandler.Spawn(prefab, pos.Value, Quaternion.identity, false);
            if (cfg != "none") CritterHandler.ModifyGiven(newCritter, cfg);

        }

    }

}
