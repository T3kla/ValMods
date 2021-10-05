using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Minimap;

namespace QoLPins
{
    public static class Pins
    {
        public static Dictionary<PinType, Color> ColorLib = new();

        public static void RemoveDeathPin(Vector3 pos, bool showDebug = true)
        {
            var pin = (from a in Minimap.instance.m_pins
                       where a.m_type == PinType.Death
                       orderby Utils.DistanceXZ(pos, a.m_pos) descending
                       select a).FirstOrDefault();

            if (pin == null)
                return;

            if (showDebug)
                Main.Log.LogInfo($"Removing death pin at '{pos.ToString("F0")}'\n");

            Minimap.instance.RemovePin(pin);
        }

        public static void UpdatePinsColor()
        {
            if (Configs.EnableColors.Value)
                foreach (var pin in Minimap.instance.m_pins)
                    if (pin.m_iconElement != null)
                        if (ColorLib.TryGetValue(pin.m_type, out var color))
                            pin.m_iconElement.color *= color;
        }

        public static void UpdateColorLib()
        {
            Main.Log.LogInfo($"Updating ColorLib\n");
            ColorLib.Clear();
            ColorLib[PinType.Death] = Configs.ColorDeath.Value.ToColor();
            ColorLib[PinType.Bed] = Configs.ColorBed.Value.ToColor();
            ColorLib[PinType.Icon0] = Configs.ColorFireplace.Value.ToColor();
            ColorLib[PinType.Icon1] = Configs.ColorHouse.Value.ToColor();
            ColorLib[PinType.Icon2] = Configs.ColorHammer.Value.ToColor();
            ColorLib[PinType.Icon3] = Configs.ColorBall.Value.ToColor();
            ColorLib[PinType.Icon4] = Configs.ColorPortal.Value.ToColor();
            ColorLib[PinType.Boss] = Configs.ColorBoss.Value.ToColor();
            ColorLib[PinType.Player] = Configs.ColorPlayer.Value.ToColor();
            ColorLib[PinType.Shout] = Configs.ColorShout.Value.ToColor();
            ColorLib[PinType.RandomEvent] = Configs.ColorRandomEvent.Value.ToColor();
            ColorLib[PinType.Ping] = Configs.ColorPing.Value.ToColor();
            ColorLib[PinType.EventArea] = Configs.ColorEventArea.Value.ToColor();
        }
    }
}
