using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage;        // 총알 데미지
    public int per;                 // 총알 관통력
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
        if (!collision.CompareTag("Enemy") || per == -10)   // per -10은 enemyCleaner
            return;
        per--;          // 충돌체가 Enemy면 관통횟수 -1
        if(per <0)      // 관통횟수 다떨어지면 오브젝트 비활성화
        {
            rigid.velocity = Vector2.zero;
            gameObject.SetActive(false);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {       // 총알이 일정거리 이상 벗어나면 비활성화
        if (!collision.CompareTag("Area") || per == -10)
            return;
        gameObject.SetActive(false);
    }
}
