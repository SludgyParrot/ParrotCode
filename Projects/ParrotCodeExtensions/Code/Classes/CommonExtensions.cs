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

#region Included Unity Assemblies
using UnityEngine;
#endregion

#region Included Parrot Code Assemblies
using ParrotCode.Native;
#endregion

namespace ParrotCode.Extensions
{
    public static class CommonExtensions
    {
        public static ScreenDirection ToScreenDirection(this Vector2 direction2D)
        {
            ScreenDirection direction = ScreenDirection.Up;
            float deadZone = 1f;

            if (direction2D.y >= deadZone && direction2D.y > direction2D.x)
                direction = ScreenDirection.Up;

            if (direction2D.y <= -deadZone && direction2D.y < direction2D.x)
                direction = ScreenDirection.Down;

            if (direction2D.x <= -deadZone && direction2D.x < direction2D.y)
                direction = ScreenDirection.Left;

            if (direction2D.x >= deadZone && direction2D.x > direction2D.y)
                direction = ScreenDirection.Right;

            return direction;
        }

        public static string Extension(this FileExtension extension)
        {
            return extension switch
            {
                FileExtension.JSON => ".json",
                FileExtension.XML => ".xml",
                FileExtension.Log => ".log",
                FileExtension.Text => ".txt",
                FileExtension.PNG => ".png",
                FileExtension.JPG => ".jpg",
                FileExtension.JPEG => ".jpeg",
                FileExtension.PDF => ".pdf",
                _ => $"Extension is currently not defined for: {extension}"
            };       
        }
    }
}
