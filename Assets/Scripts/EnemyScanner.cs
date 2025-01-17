using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScanner : MonoBehaviour
{                      // �÷��̾ ���� ����� ���� Ž���ϴ� ��ũ��Ʈ
    public float range;
    public LayerMask targetLayer;
    public RaycastHit2D[] targets;
    public Transform nearest;

    void FixedUpdate()
    {                               // �������� ����ĳ��Ʈ�ؼ� �� ������Ʈ Ž��
        targets = Physics2D.CircleCastAll(transform.position, range, Vector2.zero, 0, targetLayer);
        nearest = GetNearest();
    }
    Transform GetNearest()
    {
        Transform result = null;
        float distance = 100f;           // ��Ÿ�
        foreach(RaycastHit2D target in targets)     // ����ĳ��Ʈ ���� Ÿ�� ��
        {
            Vector3 playerPos = transform.position;
            Vector3 targetPos = target.transform.position;
            float diff = Vector3.Distance(playerPos, targetPos);
            if(diff < distance)         // �� ª�� Ÿ���� ������
            {
                distance = diff;
                result = target.transform; // �� Ÿ�� ����
            }
        }
        return result;
    }
}