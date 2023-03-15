using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public static class UIAnimation
{
    public static void DoPUnchScale(RectTransform transform, float time, float scale)
    {
        if (transform.localScale != Vector3.one) return;
        transform.DOPunchScale( Vector3.one * scale, time);
    }

    public static void DoFade(Image image, float time, float alpha)
    {
        DOTween.ToAlpha( () => image.color, x => image.color = x, alpha, time);
    }
    
    public static void DoMove(RectTransform transform, float time, Vector3 position)
    {
        transform.DOMove(position, time).SetEase(Ease.OutBack);
    }
}
