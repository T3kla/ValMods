namespace Areas.Containers
{
    public static class RawData
    {
        public readonly struct Box
        {
            public readonly string Areas = "";
            public readonly string CTData = "";
            public readonly string VAData = "";
            public readonly string SSData = "";
            public readonly string CSData = "";
            public readonly string SAData = "";

            public Box(string areas = "", string cTData = "", string vAData = "", string sSData = "", string cSData = "", string sAData = "")
                => (Areas, CTData, VAData, SSData, CSData, SAData)
                = (areas, cTData, vAData, sSData, cSData, sAData);
        }

        public static Box Loc = new();
        public static Box Rem = new();

        public static ref Box Get(EDS type)
        {
            if (type == EDS.Remote)
                return ref Rem;
            else
                return ref Loc;
        }
    }
}
