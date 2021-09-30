using System.Collections.Generic;
using System.Globalization;
using Areas.Containers;
using Jotunn.Entities;
using Jotunn.Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Areas
{
    public static class Commands
    {
        public static void Awake()
        {
            CommandManager.Instance.AddConsoleCommand(new SpawnCritter());
            CommandManager.Instance.AddConsoleCommand(new ListSpawners());
            CommandManager.Instance.AddConsoleCommand(new CreateSpawner());
            CommandManager.Instance.AddConsoleCommand(new RemoveSpawner());
        }
    }

    public class SpawnCritter : ConsoleCommand
    {
        public override string Name => "asc";
        public override string Help => "Spawn Critter as: asc [critter_name] -c [cfg] -p [x,y,z]";

        public override void Run(string[] args)
        {
            if (!SynchronizationManager.Instance.PlayerIsAdmin) { Console.instance.Print($"You are not admin"); return; }

            if (args.Length < 1) return;
            string ctName = args[0];
            GameObject prefab;

            if (Global.CurrentData.VAMods.TryGetValue(ctName, out var vaData))
                prefab = PrefabManager.Instance.GetPrefab(vaData.original);
            else
                prefab = PrefabManager.Instance.GetPrefab(ctName);

            if (prefab == null || prefab.GetComponent<Character>() == null || prefab.GetComponent<Character>()?.IsPlayer() == true)
            {
                Console.instance.Print($"Couldn't find critter with name \"{ctName}\"");
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

            Character newCritter = Critters.Spawn(prefab, pos.Value, Quaternion.identity);
            Critters.ProcessSpawnCommand(newCritter, ctName, cfg);
        }
    }


    public class ListSpawners : ConsoleCommand
    {
        public override string Name => "als";
        public override string Help => "List Custom Spawners as: command";

        public override void Run(string[] args)
        {
            Console.instance.Print($" ---- Starting search for custom spawners ---- ");

            foreach (var obj in SceneManager.GetActiveScene().GetRootGameObjects())
            {
                var dis = Vector3.Distance(obj.transform.position, Player.m_localPlayer.transform.position);

                CreatureSpawner cs = obj.GetComponent<CreatureSpawner>();
                if (cs == null) continue;

                var ctName = cs.GetCtName();
                if (string.IsNullOrEmpty(ctName)) continue;

                var data = cs.GetData();
                if (data == null) continue;

                Console.instance.Print($"Found custom spawner for critter \"{ctName}\" at position \"{obj.GetCleanPos()}\" and distance \"{dis:F2}\"");
            }

            Console.instance.Print($" ---- Finished search for custom spawners ---- ");
        }
    }

    public class CreateSpawner : ConsoleCommand
    {
        public override string Name => "acs";
        public override string Help => "Create Spawner as: command [critterName] -p [x:float, y:float, z:float] -t [respawn_time:float] -d [respawn_at_day:bool] -n [respawn_at_night:bool]";

        public override void Run(string[] args)
        {

            if (!SynchronizationManager.Instance.PlayerIsAdmin) { Console.instance.Print($"You are not admin"); return; }

            CSData DefaultCSData = new()
            {
                respawn_time_minutes = 1f,
                trigger_distance = 60f,
                trigger_noise = 0f,
                spawn_at_day = true,
                spawn_at_night = true,
                require_spawn_area = false,
                spawn_in_player_base = true,
                set_patrol_spawn_point = true,
            };

            if (args.Length < 1) return;

            string ctName = args[0];

            GameObject p_spawner = PrefabManager.Instance.GetPrefab("Spawner_Boar");
            GameObject p_critter = PrefabManager.Instance.GetPrefab(Variants.FindOriginal(ctName) ?? ctName);
            if (p_spawner == null || p_spawner.GetComponent<CreatureSpawner>() == null ||
            p_critter == null || p_critter.GetComponent<Character>() == null)
                return;

            Vector3? pos = Player.m_localPlayer.transform.position;
            float respawnTime = 1f;
            bool respawnDay = true;
            bool respawnNight = true;

            if (args.Length > 1)
                for (int i = 1; i < args.Length; i++)
                    switch (args[i])
                    {
                        case "-t":
                            float.TryParse(args[i + 1], NumberStyles.Any, CultureInfo.InvariantCulture, out respawnTime);
                            break;
                        case "-d":
                            bool.TryParse(args[i + 1], out respawnDay);
                            break;
                        case "-n":
                            bool.TryParse(args[i + 1], out respawnNight);
                            break;
                        case "-p":
                            var array = args[i + 1].Split(',');
                            pos = new Vector3(int.Parse(array[0]), int.Parse(array[1]), int.Parse(array[2]));
                            break;
                        default: break;
                    }

            GameObject newSpawner = GameObject.Instantiate(p_spawner, pos.Value, Quaternion.identity);
            CreatureSpawner cs = newSpawner.GetComponent<CreatureSpawner>();

            CSData data = new(DefaultCSData);
            data.respawn_time_minutes = Mathf.Clamp(respawnTime, 0.1f, 1000000f);
            data.spawn_at_day = respawnDay;
            data.spawn_at_night = respawnNight;

            cs.SetCtName(ctName);
            cs.SetData(data);
            cs.m_creaturePrefab = p_critter;

            Spawners.ApplyCSData(cs, data);
            Console.instance.Print($"Creating spawner for critter \"{cs.m_creaturePrefab?.name}\" at \"{Player.m_localPlayer.transform.position}\"");
        }
    }

    public class RemoveSpawner : ConsoleCommand
    {
        public override string Name => "ars";
        public override string Help => "Remove Spawner as: command -r [radios:float] -c [critter_name:string]";

        public override void Run(string[] args)
        {
            if (!SynchronizationManager.Instance.PlayerIsAdmin) { Console.instance.Print($"You are not admin"); return; }

            float radios = 10f;
            string critter = "";

            if (args.Length > 1)
                for (int i = 0; i < args.Length; i++)
                    switch (args[i])
                    {
                        case "-r":
                            float.TryParse(args[i + 1], NumberStyles.Any, CultureInfo.InvariantCulture, out radios);
                            break;
                        case "-c":
                            critter = args[i + 1];
                            break;
                        default: break;
                    }

            List<CreatureSpawner> spawnersToDestroy = new();

            foreach (var obj in SceneManager.GetActiveScene().GetRootGameObjects())
            {
                CreatureSpawner cs = obj.GetComponent<CreatureSpawner>();
                if (cs == null) continue;

                ZDO zDO = cs.m_nview?.GetZDO();
                if (zDO == null) continue;

                var str = zDO.GetString("Areas CustomCS");
                if (string.IsNullOrEmpty(str)) continue;

                if (!string.IsNullOrEmpty(critter)) if (critter != str) continue;
                if (Vector3.Distance(obj.transform.position, Player.m_localPlayer.transform.position) > radios) continue;

                spawnersToDestroy.Add(cs);
            }

            foreach (var cs in spawnersToDestroy)
            {
                var dis = Vector3.Distance(cs.transform.position, Player.m_localPlayer.transform.position);
                Console.instance.Print($"Destroying spawner for critter \"{cs.m_creaturePrefab?.name}\" at a distance of \"{dis:F2}\"");
                ZNetScene.instance.Destroy(cs.gameObject);
            }
        }
    }
}
