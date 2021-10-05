using System;
using System.Text;
using UnityEngine;

namespace QoLPins
{
    public static class Extensions
    {
        public static StringBuilder Str = new StringBuilder(9);

        public static Color ToColor(this string color)
        {
            Str.Append(color);

            if (color.StartsWith("#", StringComparison.InvariantCulture))
                Str.Remove(0, 1);

            if (Str.Length == 6)
                Str.Append("FF");

            var hex = Convert.ToUInt32(Str.ToString(), 16);
            var r = ((hex & 0xff000000) >> 0x18) / 255f;
            var g = ((hex & 0xff0000) >> 0x10) / 255f;
            var b = ((hex & 0xff00) >> 8) / 255f;
            var a = ((hex & 0xff)) / 255f;

            Str.Clear();

            return new Color(r, g, b, a);
        }
    }
}
