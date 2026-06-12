using UnityEngine;
using ParrotCode.Native.Shared;

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
    }
}
