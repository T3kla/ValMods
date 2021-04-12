using System;
using System.IO;
using UnityEngine;

namespace Areas.NetCode
{

    public class RPC
    {

        public static void SendDataToClient(long peerID)
        {

            if (ZNet.instance.GetInstanceType() == ZNetType.Client) { return; }

            ZPackage zpg = new ZPackage();

            ZRpc.Serialize(new object[] {
                Globals.LocalRaw.Areas,
                Globals.LocalRaw.CTData,
                Globals.LocalRaw.SSData,
                Globals.LocalRaw.CSData,
                Globals.LocalRaw.SAData
            }, ref zpg);

            zpg.SetPos(0);

            ZNet.instance.m_routedRpc.InvokeRoutedRPC(peerID, "Areas.SetDataValues", new object[] { zpg });

        }

        public static void SetDataValues(long sender, ZPackage zpg)
        {

            var type = ZNet.instance.GetInstanceType();
            if (type == ZNetType.Server) { return; }
            if (type == ZNetType.Local) { return; }

            Globals.RemoteRaw.Areas = zpg.ReadString();
            Globals.RemoteRaw.CTData = zpg.ReadString();
            Globals.RemoteRaw.SSData = zpg.ReadString();
            Globals.RemoteRaw.CSData = zpg.ReadString();
            Globals.RemoteRaw.SAData = zpg.ReadString();

            Main.Log.LogInfo($"Recieved data as Areas count is \"{Globals.RemoteRaw.Areas.Length}\"");
            Main.Log.LogInfo($"Recieved data as CTData count is \"{Globals.RemoteRaw.CTData.Length}\"");
            Main.Log.LogInfo($"Recieved data as SSData count is \"{Globals.RemoteRaw.SSData.Length}\"");
            Main.Log.LogInfo($"Recieved data as CSData count is \"{Globals.RemoteRaw.CSData.Length}\"");
            Main.Log.LogInfo($"Recieved data as SAData count is \"{Globals.RemoteRaw.SAData.Length}\"");

            Main.Remote_LoadData();

        }

    }

}
