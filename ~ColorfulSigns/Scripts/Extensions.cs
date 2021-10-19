using System;
using System.Text;
using UnityEngine;

namespace ColorfulSigns
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
            sb.Clear();

            return new Color(
                ((hex & 0xff000000) >> 24) / 255f,
                ((hex & 0x00ff0000) >> 16) / 255f,
                ((hex & 0x0000ff00) >> 8) / 255f,
                ((hex & 0x000000ff)) / 255f);
        }
    }
}
