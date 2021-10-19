namespace Areas.Containers
{
    public class CustomCritterData
    {
        public int? level_min { get; set; }
        public int? level_max { get; set; }
        public float? level_chance { get; set; }
        public int? level_fixed { get; set; }

        public float? health_multi { get; set; }
        public float? damage_multi { get; set; }

        public Dictionary<int[], Stage> evolution { get; set; }         // Not in Wiki
        public Dictionary<string, ByDay> scale_by_day { get; set; }     // Not in Wiki
        public Dictionary<string, Dictionary<string, float>> scale_by_boss { get; set; }
    }
}
