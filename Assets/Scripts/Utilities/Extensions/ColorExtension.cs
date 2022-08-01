using UnityEngine;

namespace Utilities.Extensions
{
    public static class ColorExtension
    {
        ///The color with alpha
        public static Color WithAlpha(this Color color, float alpha) 
        {
            color.a = alpha;
            return color;
        }

        public static Color Opacity(this Color color, float opacity)
        {
            return new Color(color.r, color.g, color.b, opacity);
        }
    }
}