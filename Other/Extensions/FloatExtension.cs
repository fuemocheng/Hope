using UnityEngine;

public static class FloatExtension
{
    public static bool EqualZero(this float input)
    {
        return Mathf.Abs(input) <= 0.00001f;
    }

    public static bool EqualTo(this float input, float other)
    {
        return Mathf.Abs(input - other) <= 0.00001f;
    }
}
