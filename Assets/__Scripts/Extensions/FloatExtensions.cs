using UnityEngine;

namespace Tetris.Extensions
{
    public static class FloatExtensions
    {
        public static float Difference(this float a, float b)
        {
            return Mathf.Abs(a - b);
        }

        public static float Round(this float f, float roundFactor)
        {
            bool isNegative = f < 0;
            f = Mathf.Abs(f);
            bool roundUp = f % roundFactor >= roundFactor / 2;

            float result;
            if (roundUp)
            {
                result = (f - f % roundFactor) + roundFactor;
            }
            else
            {
                result = f - f % roundFactor;
            }

            if (isNegative) result *= -1;

            return result;
        }
    }
}
