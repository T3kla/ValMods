namespace Areas.NetCode
{
    public enum ZNetType { Local, Client, Server }

    public static class ZNetExtensions
    {

        public static ZNetType GetInstanceType(this ZNet znet)
        {

            bool isServer = znet.IsServer();
            bool isDedicated = znet.IsDedicated();

            if (isServer && !isDedicated) return ZNetType.Local;
            if (!isServer && !isDedicated) return ZNetType.Client;
            if (isServer && isDedicated) return ZNetType.Server;

            return ZNetType.Server;

        }

    }

}