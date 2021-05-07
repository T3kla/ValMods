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

        public override string Name => "a.spawn";
        public override string Help => "Spawn critter as: a.spawn [critterName] -c [cfg] -p [x,y,z]";

        public override void Run(string[] args)
        {

            if (args.Length < 1) return;
            string ctrName = args[0];

            GameObject prefab = VariantsHandler.GetVariant(ctrName) ?? PrefabManager.Instance.GetPrefab(ctrName);
            if (prefab == null || prefab.GetComponent<Character>() == null)
            {
                Console.instance.Print($"Couldn't find critter with name \"{ctrName}\"");
                return;
            }

            string cfg = null;
            Vector3? pos = Player.m_localPlayer.transform.position + Player.m_localPlayer.transform.forward * 2;

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

            Character newCritter = CritterHandler.Spawn(prefab, pos.Value, Quaternion.identity, false);
            if (cfg != "none") CritterHandler.ModifyGiven(newCritter, cfg);

        }

    }

    // public class SpawnSpawner : ConsoleCommand
    // {

    //     public override string Name => "a.spawner";
    //     public override string Help => "Spawn a creature spawner as: a.spawner [critterName] -p [x,y,z]";

    //     public override void Run(string[] args)
    //     {

    //         if (args.Length < 1) return;
    //         string ctrName = args[0];

    //         PrefabManager.Instance.GetPrefab
    //         GameObject prefab = VariantsHandler.GetVariant(ctrName) ?? PrefabManager.Instance.GetPrefab(ctrName);
    //         if (prefab == null || prefab.GetComponent<Character>() == null)
    //         {
    //             Console.instance.Print($"Couldn't find critter with name \"{ctrName}\"");
    //             return;
    //         }

    //         string cfg = null;
    //         Vector3? pos = Player.m_localPlayer.transform.position + Player.m_localPlayer.transform.forward * 2;

    //         if (args.Length > 1)
    //             for (int i = 1; i < args.Length; i++)
    //                 switch (args[i])
    //                 {
    //                     case "-c": cfg = args[i + 1]; break;
    //                     case "-p":
    //                         var array = args[i + 1].Split(',');
    //                         pos = new Vector3(int.Parse(array[0]), int.Parse(array[1]), int.Parse(array[2]));
    //                         break;
    //                     default: break;
    //                 }

    //         Character newCritter = CritterHandler.Spawn(prefab, pos.Value, Quaternion.identity, false);
    //         if (cfg != "none") CritterHandler.ModifyGiven(newCritter, cfg);

    //     }

    // }

}
