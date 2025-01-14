using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BossState { Idle = 0, Chase, Attack }
public class BossScript : MonoBehaviour
{
    public Rigidbody2D target;
    Rigidbody2D rigid;
    SpriteRenderer sprite;
    Animator anim;
    //Collider collider;

    [Header("BossStatus")]
    public float speed;
    public float health;
    public float maxHealth;
    bool isDie;

    private void Awake()
    {
        //collider = GetComponent<Collider2D>();
        sprite = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void LateUpdate()
    {
        if (GameManager.instance.isLive == true)
        {
            if (!isDie)
            {               // 플레이어 바라보기
                sprite.flipX = target.position.x < rigid.position.x;
            }
        }
    }
    void Dead()
    {
        gameObject.SetActive(false);
    }
}
