using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScanner : MonoBehaviour
{
    public float range;
    public LayerMask targetLayer;
    public RaycastHit2D[] targets;
    public Transform nearest;
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        targets = Physics2D.CircleCastAll(transform.position, range, Vector2.zero, 0, targetLayer);
        nearest = GetNearest();
    }
    Transform GetNearest()
    {
        Transform result = null;
        float distance = 100;

        foreach(RaycastHit2D target in targets)
        {
            Vector3 myPos = transform.position;
            Vector3 targetPos = target.transform.position;
            float diff = Vector3.Distance(myPos, targetPos);
            if(diff < distance)
            {
                distance = diff;
                result = target.transform;
            }
        }
        return result;
    }
}
