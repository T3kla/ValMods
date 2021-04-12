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

        // CUstom Variables
        public int? level_min;
        public int? level_max;
        public int? level_lvlUpChance;
        public int? level_fixed;

        public string color;
        public float? size;

        // Weird Character Stuff
        // public string name = "";
        // public Faction faction = Faction.AnimalsVeg;
        // public bool boss;
        // public string bossEvent = "";
        // public string defeatSetGlobalKey = "";

        // Movement & Physics
        public float? crouch_speed;
        public float? walk_speed;
        public float? speed;
        public float? turn_speed;
        public float? run_speed;
        public float? run_turn_speed;
        public float? fly_slow_speed;
        public float? fly_fast_speed;
        public float? fly_turn_speed;
        public float? acceleration;
        public float? jump_force;
        public float? jump_force_forward;
        public float? jump_force_tired_factor;
        public float? air_control;
        public bool? can_swim;
        public float? swim_depth;
        public float? swim_speed;
        public float? swim_turn_speed;
        public float? swim_acceleration;
        // public GroundTiltType m_groundTilt;
        public bool? flying;
        public float? jump_stamina_usage;

        // Health & Damage
        public bool? tolerate_water;
        public bool? tolerate_fire;
        public bool? tolerate_smoke;
        public float? health;
        public bool? stagger_when_blocked;
        public float? stagger_damage_factor;

        // Base AI stuff
        public string path_agent_type;
        public float? view_range;
        public float? view_angle;
        public float? hear_range;
        // public EffectList alertedEffects = new EffectList();
        // public EffectList idleSound = new EffectList();
        public float? idle_sound_interval;
        public float? idle_sound_chance;
        public float? move_min_angle;
        public bool? smooth_movement;
        public bool? serpent_movement;
        public float? serpent_turn_radius;
        public float? jump_interval;

        public float? random_circle_interval;

        public float? random_move_interval;
        public float? random_move_range;

        public bool? random_fly;
        public float? chance_to_takeoff;
        public float? chance_to_land;
        public float? ground_duration;
        public float? air_duration;
        public float? max_land_altitude;
        public float? fly_altitude_min;
        public float? fly_altitude_max;
        public float? takeoff_time;

        public bool? avoid_fire;
        public bool? afraid_of_fire;
        public bool? avoid_water;
        public string spawn_message;
        public string death_message;

        // Monster AI stuff
        public float? alert_range;
        public bool? flee_if_hurt_when_target_cant_be_reached;
        public bool? flee_if_not_alerted;
        public float? flee_if_low_health;
        public bool? circulate_while_charging;
        public bool? circulate_while_charging_flying;
        public bool? enable_hunt_player;
        public bool? attack_player_objects;
        public bool? attack_player_objects_when_alerted;
        public float? intercept_time_max;
        public float? intercept_time_min;
        public float? max_chase_distance;
        public float? min_attack_interval;

        public float? circle_target_interval;
        public float? circle_target_duration;
        public float? circle_target_distance;

        public bool? sleeping;
        public bool? noise_wakeup;
        public float? noise_range_scale;
        public float? wakeup_range;
        // public EffectList wakeupEffects = new EffectList();

        public bool? avoid_land;

        // public List<ItemDrop> consumeItems;
        public float? consume_range;
        public float? consume_search_range;
        public float? consume_search_interval;
        public float? consume_heal;

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
