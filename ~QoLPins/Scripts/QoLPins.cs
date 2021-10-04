using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static Minimap;

namespace QoLPins
{
    public static class Pins
    {
        public static void RemoveDeathPin(Vector3 pos)
        {
            var pin = (from a in Minimap.instance.m_pins
                       where a.m_type == PinType.Death
                       orderby Utils.DistanceXZ(pos, a.m_pos) descending
                       select a).FirstOrDefault();

            if (pin == null) return;

            Minimap.instance.RemovePin(pin);
        }

        public static void ColorPin(PinData pin)
        {
            if (!Configs.EnableColors.Value)
                return;

            var img = pin.m_uiElement.GetComponent<Image>();
            img.color = GetPinColor(pin.m_type);
        }

        public static Color GetPinColor(PinType type) => type switch
        {
            PinType.Icon0 => Configs.ColorFireplace.Value.ToColor(),
            PinType.Icon1 => Configs.ColorHouse.Value.ToColor(),
            PinType.Icon2 => Configs.ColorHammer.Value.ToColor(),
            PinType.Icon3 => Configs.ColorBall.Value.ToColor(),
            PinType.Icon4 => Configs.ColorPortal.Value.ToColor(),
            _ => "ffffff".ToColor()
        };
    }
}
