using UnityEngine;
using Jotunn.Managers;
using Jotunn.Entities;
using Jotunn;
using System.Collections.Generic;
using Areas.Containers;
using System.Globalization;

namespace Areas
{

    public static class CommandHandler
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
        public override string Help => "Spawn Critter: command [critter_name] -c [cfg] -p [x,y,z]";

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
            CritterHandler.ProcessSpawnCommand(newCritter, cfg);

        }

    }


    public class ListSpawners : ConsoleCommand
    {

        public override string Name => "als";
        public override string Help => "List Spawners. List loaded spawners.";

        public override void Run(string[] args)
        {

            GameObject root = ZNetScene.instance.m_netSceneRoot;

            for (int i = 0; i < root.transform.childCount; i++)
            {
                GameObject obj = root.transform.GetChild(i).gameObject;

                CreatureSpawner cs = obj.GetComponent<CreatureSpawner>();
                if (cs == null) continue;

                ZDO zDO = cs.GetComponent<ZNetView>()?.GetZDO();
                if (zDO == null) continue;

                var str = zDO.GetString("Areas CustomCS");
                if (string.IsNullOrEmpty(str)) continue;

                var dis = Vector3.Distance(obj.transform.position, Player.m_localPlayer.transform.position);

                Console.instance.Print($"Found spawner for critter \"{cs.m_creaturePrefab?.name}\" at position \"{obj.transform.position}\" and distance \"{dis.ToString("F2")}\"");
            }

        }

    }

    public class CreateSpawner : ConsoleCommand
    {

        public override string Name => "acs";
        public override string Help => "Create Spawner: command [critterName] -p [x:float, y:float, z:float] -t [respawn_time:float] -d [respawn_at_day:bool] -n [respawn_at_night:bool]";

        public override void Run(string[] args)
        {

            CSData DefaultCSData = new CSData()
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

            GameObject p_spawner = PrefabManager.Instance.GetPrefab("Spawner_Boar");
            GameObject p_critter = PrefabManager.Instance.GetPrefab(args[0]);
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

            CSData data = new CSData(DefaultCSData);
            data.respawn_time_minutes = respawnTime;
            data.spawn_at_day = respawnDay;
            data.spawn_at_night = respawnNight;

            cs.SetCustomCS(args[0], data);

            cs.m_creaturePrefab = p_critter;
            SpawnerHandler.ApplyCSData(cs, data);
            Console.instance.Print($"Creating spawner for critter \"{cs.m_creaturePrefab?.name}\" at \"{Player.m_localPlayer.transform.position}\"");

        }

    }

    public class RemoveSpawner : ConsoleCommand
    {

        public override string Name => "ars";
        public override string Help => "Remove Spawner: command -r [radios:float] -c [critter_name:string]";

        public override void Run(string[] args)
        {

            HashSet<CreatureSpawner> spawners = new HashSet<CreatureSpawner>();
            GameObject root = ZNetScene.instance.m_netSceneRoot;

            float radios = 10f;
            string critter = "";

            if (args.Length > 1)
                for (int i = 1; i < args.Length; i++)
                    switch (args[i])
                    {
                        case "-r":
                            float.TryParse(args[i + 1], out radios);
                            break;
                        case "-c":
                            critter = args[i + 1];
                            break;
                        default: break;
                    }


            for (int i = 0; i < root.transform.childCount; i++)
            {
                GameObject obj = root.transform.GetChild(i).gameObject;

                CreatureSpawner cs = obj.GetComponent<CreatureSpawner>();
                if (cs == null) continue;

                ZDO zDO = cs.GetComponent<ZNetView>()?.GetZDO();
                if (zDO == null) continue;

                var str = zDO.GetString("Areas CustomCS");
                if (string.IsNullOrEmpty(str)) continue;

                if (!string.IsNullOrEmpty(critter)) if (critter != str) continue;
                if (Vector3.Distance(obj.transform.position, Player.m_localPlayer.transform.position) > radios) continue;

                spawners.Add(cs);
            }

            foreach (var cs in spawners)
            {
                var dis = Vector3.Distance(cs.transform.position, Player.m_localPlayer.transform.position);
                Console.instance.Print($"Destroying spawner for critter \"{cs.m_creaturePrefab?.name}\" at a distance of \"{dis.ToString("F2")}\"");
                GameObject.Destroy(cs.gameObject);
            }

        }

    }

}
