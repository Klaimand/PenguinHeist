using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GameOverButton : MonoBehaviour
{
    private RectTransform rect;
    private Vector3 basePos;
    
    // Start is called before the first frame update
    void Start()
    {
        rect = GetComponent<RectTransform>();
        basePos = rect.transform.position;
        
        UIAnimation.DoMove(rect, 0.0001f, Vector3.left * 1000);
        UIAnimation.DoMove(rect, 1.2f, basePos);
    }
}
