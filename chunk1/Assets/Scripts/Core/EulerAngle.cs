using UnityEngine;

namespace Assets.Scripts.Core
{
    public static class Euler
    {
        public static float Clamp360(this float x)
        {
            while (x > 360f)
                x -= 360f;
            while (x < 0f)
                x += 360f;
            return x;
        }

        public static float Diff(float x, float y)
        {
            var diff = y - x;
            if (diff < -180f)
                diff = y + 360f - x;
            else if (diff > 180f)
                diff = y + 360f - x;

            return diff.Clamp360();
        }
    }
}
