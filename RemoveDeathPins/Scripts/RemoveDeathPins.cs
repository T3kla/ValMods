using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RemoveDeathPins
{

    public static class RDP
    {

        public static void AddDeathPoint(Vector3 pos)
        {

            PlayerProfile playerProfile = Game.instance.GetPlayerProfile();
            Minimap.instance.AddPin(pos, Minimap.PinType.Death, "", true, false);

        }

        public static void RemoveDeathPin(Vector3 pos)
        {

            List<Minimap.PinData> toRemove = Minimap.instance.m_pins.Where(a => a.m_type == Minimap.PinType.Death && Utils.DistanceXZ(pos, a.m_pos) < 1f).ToList();
            foreach (var pin in toRemove) Minimap.instance.RemovePin(pin);

        }

    }

}