using System.Collections.Generic;

namespace Areas.Containers
{
    public class DataVariant : DataCritter
    {
        public string original { get; set; }
        public List<string[]> localization { get; set; }
    }
}
