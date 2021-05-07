namespace Areas.NetCode
{

    public class RPC
    {

        public static void SendDataToClient(long peerID)
        {

            if (ZNet.instance.GetInstanceType() == ZNetType.Client) { return; }

            ZPackage zpg = new ZPackage();

            ZRpc.Serialize(new object[] {
                Globals.RawLocalData.Areas,
                Globals.RawLocalData.CTData,
                Globals.RawLocalData.SSData,
                Globals.RawLocalData.CSData,
                Globals.RawLocalData.SAData
            }, ref zpg);

            zpg.SetPos(0);

            ZNet.instance.m_routedRpc.InvokeRoutedRPC(peerID, "Areas.SetDataValues", new object[] { zpg });

        }

        public static void SetDataValues(long sender, ZPackage zpg)
        {

            var type = ZNet.instance.GetInstanceType();
            if (type == ZNetType.Server) { return; }
            if (type == ZNetType.Local) { return; }

            Globals.RawRemoteData.Areas = zpg.ReadString();
            Globals.RawRemoteData.CTData = zpg.ReadString();
            Globals.RawRemoteData.SSData = zpg.ReadString();
            Globals.RawRemoteData.CSData = zpg.ReadString();
            Globals.RawRemoteData.SAData = zpg.ReadString();

            Main.GLog.LogInfo($"Received data as Areas count is \"{Globals.RawRemoteData.Areas.Length}\"");
            Main.GLog.LogInfo($"Received data as CTData count is \"{Globals.RawRemoteData.CTData.Length}\"");
            Main.GLog.LogInfo($"Received data as SSData count is \"{Globals.RawRemoteData.SSData.Length}\"");
            Main.GLog.LogInfo($"Received data as CSData count is \"{Globals.RawRemoteData.CSData.Length}\"");
            Main.GLog.LogInfo($"Received data as SAData count is \"{Globals.RawRemoteData.SAData.Length}\"");

            Main.LoadData(EDS.Remote);

        }

    }

}
