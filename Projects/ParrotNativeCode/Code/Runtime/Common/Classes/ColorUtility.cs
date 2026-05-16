using System;
using UnityEngine;

namespace ParrotCode.Native.Common
{
    public static class ColorUtility
    {
        public static Color GetColor(ColorID colorID)
        {
            switch(colorID)
            {
                case ColorID.Black:
                    return Color.black;
                case ColorID.White:
                    return Color.white;
                case ColorID.Red:
                    return Color.red;
                case ColorID.Green:
                    return Color.green;
                case ColorID.Blue:
                    return Color.blue;
                case ColorID.Cyan:
                    return Color.cyan;
                case ColorID.Magenta:
                    return Color.magenta;
                case ColorID.Gray:
                    return Color.grey;
                case ColorID.Grey:
                    return Color.grey;
                default: 
                    return Color.clear;
            }
        }

        public static string SetStringColor(string text, ColorID colorID)
            => $"<color={colorID.ToString().ToLower()}>{text}</color>";

        public static Color GetColor(string htmlString)
        {   
            try
            {
                Color color;
                UnityEngine.ColorUtility.TryParseHtmlString(htmlString, out color);
                return color;
            }
            catch(Exception e)
            {
                throw new Exception($"Get color using html string: {htmlString} threw an exception.");
            }
        }
    }
}
