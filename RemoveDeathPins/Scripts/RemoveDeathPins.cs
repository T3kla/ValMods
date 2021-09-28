using UnityEngine;
using PinType = Minimap.PinType;

namespace RemoveDeathPins
{
    public static class RDP
    {
        public static void AddDeathPoint(Vector3 pos)
        {
            Minimap.instance.AddPin(pos, PinType.Death, "", true, false);
        }

        public static void RemoveDeathPin(Vector3 pos)
        {
            foreach (var pin in Minimap.instance.m_pins)
            {
                if (pin.m_type != PinType.Death) continue;
                if (Utils.DistanceXZ(pos, pin.m_pos) > 1f) continue;
                Minimap.instance.RemovePin(pin);
                return;
            }
        }
    }
}