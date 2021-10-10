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
            => (this.type, this.name) = (PinType.None, null);
        public AutoPin(PinType type, string name)
            => (this.type, this.name) = (type, name);
    }

    public static class PinAuto
    {
        public static AutoPin TinData = new(PinType.Icon3, "Tin");
        public static AutoPin CopData = new(PinType.Icon3, "Copper");
        public static AutoPin SilData = new(PinType.Icon3, "Silver");

        public static Vector3 a = new Vector3();

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
        }

        private static void Add(Vector3 pos, PinType type, string name = null)
            => Minimap.instance.AddPin(pos, type, name ?? "", true, false, 0L);

        public static bool AddPin(Vector3 pos, AutoPin ap)
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

        public static string PinTypeToStr(PinType type) => type switch
        {
            PinType.Icon0 => "Icon0",
            PinType.Icon1 => "Icon1",
            PinType.Icon2 => "Icon2",
            PinType.Icon3 => "Icon3",
            PinType.Icon4 => "Icon4",
            PinType.Death => "Death",
            PinType.Bed => "Bed",
            PinType.Shout => "Shout",
            PinType.None => "None",
            PinType.Boss => "Boss",
            PinType.Player => "Player",
            PinType.RandomEvent => "RandomEvent",
            PinType.Ping => "Ping",
            PinType.EventArea => "EventArea",
            _ => ""
        };

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
