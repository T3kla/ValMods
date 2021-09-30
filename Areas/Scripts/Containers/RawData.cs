namespace Areas.Containers
{
    public static class RawData
    {
        public class Wrap
        {
            public string Areas = "";
            public string CTData = "";
            public string VAData = "";
            public string SSData = "";
            public string CSData = "";
            public string SAData = "";
        }

        public static Wrap Local = new();
        public static Wrap Remote = new();
    }
}
