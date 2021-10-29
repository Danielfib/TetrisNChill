using UnityEngine;

public static class FloatExtensions
{
    public static float Difference(this float a, float b)
    {
        return Mathf.Abs(a - b);
    }
}
