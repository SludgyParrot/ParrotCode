/*

Parrot Code
Copyright (c) 2026 Sludgy Parrot (Pty) Ltd. All Rights Reserved.

This source code is proprietary and confidential software owned by
Sludgy Parrot (Pty) Ltd.

Parrot Code is a commercial software product developed and distributed
by Sludgy Parrot (Pty) Ltd.

Unauthorized copying, modification, distribution, sublicensing,
reverse engineering, decompilation, disclosure, or use of this
software, in whole or in part, is strictly prohibited without
prior written permission from Sludgy Parrot (Pty) Ltd.

This software is provided under the terms of a separate license
agreement. Possession of this source code does not grant any rights
to use, modify, distribute, or create derivative works unless
explicitly authorized by a valid written license.

THE SOFTWARE IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, EXCEPT AS REQUIRED BY APPLICABLE LAW.

For licensing inquiries:
licensing@sludgyparrot.com

*/

using System;
using UnityEngine;

namespace ParrotCode.Native
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
