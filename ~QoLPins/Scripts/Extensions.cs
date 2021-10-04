using System;
using System.Text;
using UnityEngine;

namespace QoLPins
{
    public static class Extensions
    {
        public static StringBuilder sb = new StringBuilder(9);

        public static Color ToColor(this string color)
        {
            sb.Append(color);

            if (color.StartsWith("#", StringComparison.InvariantCulture))
                sb.Remove(0, 1);

            if (sb.Length == 6)
                sb.Append("FF");

            var hex = Convert.ToUInt32(sb.ToString(), 16);
            var r = ((hex & 0xff000000) >> 0x18) / 255f;
            var g = ((hex & 0xff0000) >> 0x10) / 255f;
            var b = ((hex & 0xff00) >> 8) / 255f;
            var a = ((hex & 0xff)) / 255f;

            sb.Clear();

            return new Color(r, g, b, a);
        }
    }
}
