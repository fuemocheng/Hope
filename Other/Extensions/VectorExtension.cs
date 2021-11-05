using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VectorExtension
{
    //topdown 2d view to 3d
    public static Vector3 To3D(this Vector2 vec, Transform reference = null)
    {
        return new Vector3(vec.x, reference ? reference.localPosition.y : 0, vec.y);
    }

    public static Vector2 To2D(this Vector3 vec)
    {
        return new Vector2(vec.x, vec.z);
    }

    public static Vector3 SetX(this Vector3 vec, float x)
    {
        return new Vector3(x, vec.y, vec.z);
    }

    public static Vector3 SetY(this Vector3 vec, float y)
    {
        return new Vector3(vec.x, y, vec.z);
    }

    public static Vector3 SetZ(this Vector3 vec, float z)
    {
        return new Vector3(vec.x, vec.y, z);
    }

    public static bool Approximate(this Vector3 a, Vector3 b)
    {
        return Mathf.Abs(a.x - b.x) <= 0.01f && Mathf.Abs(a.y - b.y) <= 0.01f && Mathf.Abs(a.z - b.z) <= 0.01f;
    }
}
