using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods
{
    public static Vector2 NormalizeIfGreater(this Vector2 v)
    {
        return v.sqrMagnitude > 1f ? v.normalized : v;
    }

    public static Vector3 Flatten(this Vector2 v)
    {
        return new Vector3(v.x, 0f, v.y);
    }

    public static Vector2 ZeroIfBelow(this Vector2 v, float minMagnitude)
    {
        return v.magnitude < minMagnitude ? Vector2.zero : v;
    }
}
