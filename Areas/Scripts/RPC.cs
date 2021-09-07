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
                Globals.RawLocalData.Areas,
                Globals.RawLocalData.CTData,
                Globals.RawLocalData.VAData,
                Globals.RawLocalData.SSData,
                Globals.RawLocalData.CSData,
                Globals.RawLocalData.SAData,
            }, ref zpg);

            zpg.SetPos(0);

            ZNet.instance.m_routedRpc.InvokeRoutedRPC(peerID, "Areas.SetDataValues", new object[] { zpg });

        }

        public static void SetDataValues(long sender, ZPackage zpg)
        {

            var type = ZNet.instance.GetInstanceType();
            if (type == ZNetExtension.ZNetInstanceType.Server) { return; }
            if (type == ZNetExtension.ZNetInstanceType.Local) { return; }

            Globals.RawRemoteData.Areas = zpg.ReadString();
            Globals.RawRemoteData.CTData = zpg.ReadString();
            Globals.RawRemoteData.VAData = zpg.ReadString();
            Globals.RawRemoteData.SSData = zpg.ReadString();
            Globals.RawRemoteData.CSData = zpg.ReadString();
            Globals.RawRemoteData.SAData = zpg.ReadString();

            Main.GLog.LogInfo($"Received data as Areas count is \"{Globals.RawRemoteData.Areas.Length}\"");
            Main.GLog.LogInfo($"Received data as CTData count is \"{Globals.RawRemoteData.CTData.Length}\"");
            Main.GLog.LogInfo($"Received data as VAData count is \"{Globals.RawRemoteData.VAData.Length}\"");
            Main.GLog.LogInfo($"Received data as SSData count is \"{Globals.RawRemoteData.SSData.Length}\"");
            Main.GLog.LogInfo($"Received data as CSData count is \"{Globals.RawRemoteData.CSData.Length}\"");
            Main.GLog.LogInfo($"Received data as SAData count is \"{Globals.RawRemoteData.SAData.Length}\"");

            Main.LoadData(EDS.Remote);

        }

    }

}
