using Jotunn;

namespace Areas.NetCode
{

    public class RPC
    {

        public static void SendDataToClient(long peerID)
        {

            if (ZNet.instance.GetInstanceType() == ZNetExtension.ZNetInstanceType.Client) { return; }

            ZPackage zpg = new();

            ZRpc.Serialize(new object[] {
                Global.RawLocalData.Areas,
                Global.RawLocalData.CTData,
                Global.RawLocalData.VAData,
                Global.RawLocalData.SSData,
                Global.RawLocalData.CSData,
                Global.RawLocalData.SAData,
            }, ref zpg);

            zpg.SetPos(0);

            ZNet.instance.m_routedRpc.InvokeRoutedRPC(peerID, "Areas.SetDataValues", new object[] { zpg });

        }

        public static void SetDataValues(long sender, ZPackage zpg)
        {

            var type = ZNet.instance.GetInstanceType();
            if (type == ZNetExtension.ZNetInstanceType.Server) { return; }
            if (type == ZNetExtension.ZNetInstanceType.Local) { return; }

            Global.RawRemoteData.Areas = zpg.ReadString();
            Global.RawRemoteData.CTData = zpg.ReadString();
            Global.RawRemoteData.VAData = zpg.ReadString();
            Global.RawRemoteData.SSData = zpg.ReadString();
            Global.RawRemoteData.CSData = zpg.ReadString();
            Global.RawRemoteData.SAData = zpg.ReadString();

            Main.Log.LogInfo($"Received data as Areas count is \"{Global.RawRemoteData.Areas.Length}\"");
            Main.Log.LogInfo($"Received data as CTData count is \"{Global.RawRemoteData.CTData.Length}\"");
            Main.Log.LogInfo($"Received data as VAData count is \"{Global.RawRemoteData.VAData.Length}\"");
            Main.Log.LogInfo($"Received data as SSData count is \"{Global.RawRemoteData.SSData.Length}\"");
            Main.Log.LogInfo($"Received data as CSData count is \"{Global.RawRemoteData.CSData.Length}\"");
            Main.Log.LogInfo($"Received data as SAData count is \"{Global.RawRemoteData.SAData.Length}\"");

            Main.LoadData(EDS.Remote);

        }

    }

}
