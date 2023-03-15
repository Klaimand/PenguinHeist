using DG.Tweening;
using UnityEngine;

public static class UIAnimation
{
    public static void DoPUnchScale(RectTransform transform, float time, float scale)
    {
        if (transform.localScale != Vector3.one) return;
        transform.DOPunchScale( Vector3.one * scale, time);
    }
}
