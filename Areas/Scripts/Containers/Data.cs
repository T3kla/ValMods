using System.Collections.Generic;
using System.Linq;

namespace Areas.Containers
{
    public static class Data
    {
        public class Wrap
        {
            public Dictionary<string, Area> Areas = new();
            public Dictionary<string, Dictionary<string, CTData>> CTMods = new();
            public Dictionary<string, VAData> VAMods = new();
            public Dictionary<string, Dictionary<int, SSData>> SSMods = new();
            public Dictionary<string, Dictionary<string, CSData>> CSMods = new();
            public Dictionary<string, Dictionary<string, SAData>> SAMods = new();

            public bool RetrieveVAData(string ctName, out VAData data)
            {
                data = (from a in VAMods
                        where a.Key == ctName
                        select a.Value)
                        .FirstOrDefault();
                return data != null;
            }

            public bool RetrieveSSData(string cfg, int index, out SSData data)
            {
                data = (from a in SSMods
                        where a.Key == cfg
                        from b in a.Value
                        where b.Key == index
                        select b.Value)
                        .FirstOrDefault();
                return data != null;
            }

            public bool RetrieveCSData(string cfg, string spawnerName, out CSData data)
            {
                data = (from a in CSMods
                        where a.Key == cfg
                        from b in a.Value
                        where b.Key == spawnerName
                        select b.Value)
                        .FirstOrDefault();
                return data != null;
            }

            public bool RetrieveSAData(string cfg, string spawnerName, out SAData data)
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

        public static Wrap Current = new();
    }
}
