using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods
{
    public static Vector2 NormalizeIfGreater(this Vector2 v)
    {
        return v.sqrMagnitude > 1f ? v.normalized : v;
    }
}
