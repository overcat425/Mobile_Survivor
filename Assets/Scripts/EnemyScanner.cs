using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScanner : MonoBehaviour
{                      // 플레이어가 가장 가까운 적을 탐색하는 스크립트
    public float range;
    public LayerMask targetLayer;
    public RaycastHit2D[] targets;
    public Transform nearest;

    void FixedUpdate()
    {                               // 원형으로 레이캐스트해서 적 오브젝트 탐색
        targets = Physics2D.CircleCastAll(transform.position, range, Vector2.zero, 0, targetLayer);
        nearest = GetNearest();
    }
    Transform GetNearest()
    {
        Transform result = null;
        float distance = 100;           // 사거리

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
