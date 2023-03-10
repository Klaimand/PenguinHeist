using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods
{
    public static Vector2 NormalizeIfGreater(ref this Vector2 v)
    {
        v = v.sqrMagnitude > 1f ? v.normalized : v;
        return v;
    }

    public static Vector3 Flatten(this Vector2 v)
    {
        return new Vector3(v.x, 0f, v.y);
    }

    public static Vector2 ZeroIfBelow(ref this Vector2 v, float minMagnitude)
    {
        v = v.magnitude < minMagnitude ? Vector2.zero : v;
        return v;
    }

    public static void ClampAtZero(ref this int i)
    {
        i = i < 0 ? 0 : i;
    }

    public static void ClampAtZero(ref this float f)
    {
        f = f < 0 ? 0 : f;
    }
}
