using System.Collections.Generic;
using UnityEngine;

namespace Areas.Containers
{

    public class Area
    {

        public string id;
        public string cfg;
        public int layer;
        public string display_name;

        public Vector2 centre;
        public int inner_radious;
        public int outter_radious;

    }

    public class CTMods
    {

        public int? level_min;
        public int? level_max;
        public int? level_lvlUpChance;
        public int? level_fixed;

        // Weird stuff
        // public string name = "";
        // public Faction faction = Faction.AnimalsVeg;
        // public bool boss;
        // public string bossEvent = "";
        // public string defeatSetGlobalKey = "";

        // Movement & Physics
        public float? crouch_speed = 2f;
        public float? walk_speed = 5f;
        public float? speed = 10f;
        public float? turn_speed = 300f;
        public float? run_speed = 20f;
        public float? run_turn_speed = 300f;
        public float? fly_slow_speed = 5f;
        public float? fly_fast_speed = 12f;
        public float? fly_turn_speed = 12f;
        public float? acceleration = 1f;
        public float? jump_force = 10f;
        public float? jump_force_forward;
        public float? jump_force_tired_factor = 0.7f;
        public float? air_control = 0.1f;
        public bool? can_swim = true;
        public float? swim_depth = 2f;
        public float? swim_speed = 2f;
        public float? swim_turn_speed = 100f;
        public float? swim_acceleration = 0.05f;
        // public GroundTiltType m_groundTilt;
        public bool? flying;
        public float? jump_stamina_usage = 10f;

        // Health & Damage
        public bool? tolerate_water = true;
        public bool? tolerate_fire;
        public bool? tolerate_smoke = true;
        public float? health = 10f;
        public bool? stagger_when_blocked = true;
        public float? stagger_damage_factor;

    }

    public class SSMods
    {

        public bool? enabled;
        // public Heightmap.Biome? biome;
        // public Heightmap.BiomeArea? biome_area;

        public int? max_spawned;
        public float? spawn_interval; // How often do it spawn
        public float? spawn_chance; // Chanse to spawn each spawn interval
        public float? spawn_distance; // Minimum distance to another instance

        public float? spawn_radius_min; // 0 for global settings
        public float? spawn_radius_max;

        public string required_global_key; // only spawn if this key is set

        // public List<string> requiredEnvironments = new List<string>(); // only spawn if this environment is active

        public int? group_size_min;
        public int? group_size_max;
        public int? group_radius;

        public bool? spawn_at_day;
        public bool? spawn_at_night;

        public int? min_altitude;
        public int? max_altitude;

        public float? min_tilt;
        public float? max_tilt;

        public bool? in_forest;
        public bool? outside_forest;

        public float? min_ocean_depth;
        public float? max_ocean_depth;

        public bool? hunt_player;
        public float? ground_offset;

    }

    public class CSMods
    {

        public float? respawn_time_minutes;

        public float? trigger_distance;
        public float? trigger_noise;

        public bool? spawn_at_day;
        public bool? spawn_at_night;
        public bool? require_spawn_area;
        public bool? spawn_in_player_base;
        public bool? set_patrol_spawn_point;

    }

    public class SAMods
    {

        public float? spawn_interval_sec;
        public float? trigger_distance;
        public bool? set_patrol_spawn_point;
        public float? spawn_radius;
        public float? near_radius;
        public float? far_radius;
        public int? max_near;
        public int? max_total;
        public bool? on_ground_only;

    }

}
