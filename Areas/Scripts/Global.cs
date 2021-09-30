using Areas.Containers;

namespace Areas
{
    public static class Global
    {
        public static class Path
        {
            public static string ModFolder;
            public static string Areas => $@"{ModFolder}\areas.yaml";
            public static string CTData => $@"{ModFolder}\critters.yaml";
            public static string VAData => $@"{ModFolder}\variants.yaml";
            public static string SSData => $@"{ModFolder}\ss_data.yaml";
            public static string CSData => $@"{ModFolder}\cs_data.yaml";
            public static string SAData => $@"{ModFolder}\sa_data.yaml";
            public static string Assets => $@"{ModFolder}\areasbundle";
        }
    }
}
