using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Awareness : MonoBehaviour
{

    public float viewRadius = 10;
    [Range(0,360)]
    public float viewAngle = 90;

    public LayerMask targetMask;
    public LayerMask obstacleMask;
    [HideInInspector] public List<Transform> visibleTargets;

    void Start() {
        Init();
    }

    public void Init()
    {
        FindVisibleTargets ();
        StartCoroutine ("FindTargetsWithDelay", .2f);
    }


    IEnumerator FindTargetsWithDelay(float delay) {
        while (true) {
            yield return new WaitForSeconds (delay);
            FindVisibleTargets ();
        }
    }

    void FindVisibleTargets()
    {
        Collider[] targetsInViewRadius = Physics.OverlapSphere (transform.position, viewRadius, targetMask);
        targetsInViewRadius.OrderBy(x => Vector3.Distance(transform.position, x.transform.position)).ToArray().CopyTo(targetsInViewRadius, 0);
        visibleTargets = new List<Transform> ();
        for (int i = 0; i < targetsInViewRadius.Length; i++) {
            if (targetsInViewRadius[i].gameObject == gameObject)
            {
                continue;
            }
            Transform target = targetsInViewRadius [i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            if (Vector3.Angle (transform.forward, dirToTarget) < viewAngle / 2) {
                float dstToTarget = Vector3.Distance (transform.position, target.position);
                if (!Physics.Raycast (transform.position, dirToTarget,dstToTarget, obstacleMask)) {
                    visibleTargets.Add(target);
                }
            }
            
        }
    }
    
    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal) {
        if (!angleIsGlobal) {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad),0,Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
