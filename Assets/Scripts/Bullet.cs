using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage;        // �Ѿ� ������
    public int per;                 // �Ѿ� �����
    Rigidbody2D rigid;
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }
    public void Init(float damage, int per, Vector3 dir)
    {
        this.damage = damage;
        this.per = per;

        if (per >= 0)
        {
            rigid.velocity = dir * 10f;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy") || per == -10)   // per -10�� enemyCleaner
            return;
        per--;          // �浹ü�� Enemy�� ����Ƚ�� -1
        if(per <0)      // ����Ƚ�� �ٶ������� ������Ʈ ��Ȱ��ȭ
        {
            rigid.velocity = Vector2.zero;
            gameObject.SetActive(false);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {       // �Ѿ��� �����Ÿ� �̻� ����� ��Ȱ��ȭ
        if (!collision.CompareTag("Area") || per == -10)
            return;
        gameObject.SetActive(false);
    }
}
