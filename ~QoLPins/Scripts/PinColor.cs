using System.Collections.Generic;
using UnityEngine;
using static Minimap;

namespace QoLPins
{
    public static class PinColor
    {
        public static Dictionary<PinType, Color> ColorLib = new();

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
            if (!Configs.EnableColors.Value)
                return;

            Main.Log.LogInfo($"Updating ColorLib\n");

            ColorLib.Clear();
            ColorLib[PinType.Death] = Configs.ColorDeath.Value.ToColor();
            ColorLib[PinType.Bed] = Configs.ColorBed.Value.ToColor();
            ColorLib[PinType.Icon0] = Configs.ColorFireplace.Value.ToColor();
            ColorLib[PinType.Icon1] = Configs.ColorHouse.Value.ToColor();
            ColorLib[PinType.Icon2] = Configs.ColorHammer.Value.ToColor();
            ColorLib[PinType.Icon3] = Configs.ColorBall.Value.ToColor();
            ColorLib[PinType.Icon4] = Configs.ColorCave.Value.ToColor();
            ColorLib[PinType.Boss] = Configs.ColorBoss.Value.ToColor();
            ColorLib[PinType.Player] = Configs.ColorPlayer.Value.ToColor();
            ColorLib[PinType.Shout] = Configs.ColorShout.Value.ToColor();
            ColorLib[PinType.RandomEvent] = Configs.ColorRandomEvent.Value.ToColor();
            ColorLib[PinType.Ping] = Configs.ColorPing.Value.ToColor();
            ColorLib[PinType.EventArea] = Configs.ColorEventArea.Value.ToColor();
        }
    }
}
