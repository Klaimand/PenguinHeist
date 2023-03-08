using System;
using UnityEngine;

public class InputEntry : MonoBehaviour
{
    private SplitScreen _splitScreen;
    public Transform p1Pos;
    public GameObject debugBall;
    //public Vector3 p2Pos;
    //public Vector3 offsetPlayer2Rotation;
    
    private Ray ray;
    private RaycastHit hit;

    private void Start()
    {
        _splitScreen = GetComponent<SplitScreen>();
    }

    void Update()
    {
        if (GetClickPosition() != null)
        {
            debugBall.transform.position = hit.point;
            var i = hit.point - p1Pos.transform.position; 
            i.Normalize();
            _splitScreen.player1OffSetCam = i;
        }
        else
        {
            _splitScreen.player1OffSetCam = Vector3.zero;
        }
    }
    
    public Vector3? GetClickPosition()
    {
        if (Input.GetMouseButton(0))
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log(hit.point);
                return hit.point;
            }
        }

        return null;
    }
}
