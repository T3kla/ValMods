using System.Collections.Generic;
using UnityEngine;
using static Minimap;

namespace QoLPins
{
    public class AutoPin
    {
        public PinType type;
        public string name;
        public AutoPin()
            => (type, name) = (PinType.None, null);
        public AutoPin(PinType _type, string _name)
            => (type, name) = (_type, _name);
    }

    public static class PinAuto
    {
        public static AutoPin TinData = new(PinType.Icon3, "Tin");
        public static AutoPin CopData = new(PinType.Icon3, "Copper");
        public static AutoPin SilData = new(PinType.Icon3, "Silver");
        public static AutoPin DunData = new(PinType.Icon4, "");

        public static void UpdateAutoPinData()
        {
            if (!Configs.EnableAutoPin.Value)
                return;

            var tin = Configs.AutoTin.Value.Split(':');
            var cop = Configs.AutoCopper.Value.Split(':');
            var sil = Configs.AutoSilver.Value.Split(':');
            TinData = tin.Length == 2 ? new AutoPin(StrToPinType(tin[0]), tin[1]) : new AutoPin();
            CopData = cop.Length == 2 ? new AutoPin(StrToPinType(cop[0]), cop[1]) : new AutoPin();
            SilData = sil.Length == 2 ? new AutoPin(StrToPinType(sil[0]), sil[1]) : new AutoPin();

            var dun = Configs.AutoDungeon.Value.Split(':');
            DunData = dun.Length == 2 ? new AutoPin(StrToPinType(dun[0]), dun[1]) : new AutoPin();
        }

        private static void Add(Vector3 pos, PinType type, string name = null)
            => Minimap.instance.AddPin(pos, type, name ?? "", true, false, 0L);

        public static bool AddSafe(Vector3 pos, AutoPin ap)
        {
            if (ap.type == PinType.None || FindPin(pos, ap) != null)
                return false;

            Add(pos, ap.type, ap.name);
            return true;
        }

        public static bool RemovePin(Vector3 pos, AutoPin ap)
            => RemovePin(pos, ap.type, ap.name);
        public static bool RemovePin(Vector3 pos, PinType type = PinType.None, string name = null)
        {
            if (FindPin(pos, type, name) is not PinData pin)
                return false;

            Minimap.instance.RemovePin(pin);

            return true;
        }

        public static PinData FindPin(Vector3 pos, AutoPin ap)
            => FindPin(pos, ap.type, ap.name);
        public static PinData FindPin(Vector3 pos, PinType type = PinType.None, string name = null)
        {
            List<(PinData pin, float dis)> pins = new();

            foreach (var pin in Minimap.instance.m_pins)
                if (type == PinType.None || pin.m_type == type)
                    pins.Add((pin, Utils.DistanceXZ(pos, pin.m_pos)));

            PinData closest = null;
            float closestDis = float.MaxValue;

            foreach (var pairs in pins)
                if (closest == null || pairs.dis < closestDis)
                {
                    closest = pairs.pin;
                    closestDis = pairs.dis;
                }

            if (closestDis > 1f)
                return null;

            if (!string.IsNullOrEmpty(name) && closest.m_name != name)
                return null;

            return closest;
        }

        public static PinType StrToPinType(string str) => str switch
        {
            "Fireplace" => PinType.Icon0,
            "House" => PinType.Icon1,
            "Hammer" => PinType.Icon2,
            "Ball" => PinType.Icon3,
            "Cave" => PinType.Icon4,
            _ => PinType.None
        };
    }
}
