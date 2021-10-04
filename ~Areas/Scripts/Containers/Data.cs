using System.Collections.Generic;
using System.Linq;

namespace Areas.Containers
{
    public static class Data
    {
        public static Dictionary<string, Area> Areas = new();
        public static Dictionary<string, Dictionary<string, DataCritter>> CTMods = new();
        public static Dictionary<string, DataVariant> VAMods = new();
        public static Dictionary<string, Dictionary<int, DataSS>> SSMods = new();
        public static Dictionary<string, Dictionary<string, DataCS>> CSMods = new();
        public static Dictionary<string, Dictionary<string, DataSA>> SAMods = new();

        public static bool RetrieveVAData(string ctName, out DataVariant data)
        {
            data = (from a in VAMods
                    where a.Key == ctName
                    select a.Value)
                    .FirstOrDefault();
            return data != null;
        }

        public static bool RetrieveSSData(string cfg, int index, out DataSS data)
        {
            data = (from a in SSMods
                    where a.Key == cfg
                    from b in a.Value
                    where b.Key == index
                    select b.Value)
                    .FirstOrDefault();
            return data != null;
        }

        public static bool RetrieveCSData(string cfg, string spawnerName, out DataCS data)
        {
            data = (from a in CSMods
                    where a.Key == cfg
                    from b in a.Value
                    where b.Key == spawnerName
                    select b.Value)
                    .FirstOrDefault();
            return data != (DataCS)default;
        }

        public static bool RetrieveSAData(string cfg, string spawnerName, out DataSA data)
        {
            data = (from a in SAMods
                    where a.Key == cfg
                    from b in a.Value
                    where b.Key == spawnerName
                    select b.Value)
                    .FirstOrDefault();
            return data != null;
        }
    }
}
