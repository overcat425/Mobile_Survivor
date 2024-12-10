using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public RuntimeAnimatorController[] animCtrl;
    public float speed;
    public float health;
    public float maxHealth;

    public Rigidbody2D target;
    bool isDie;
    Rigidbody2D rigid;
    SpriteRenderer sprite;
    Animator anim;
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }
    void Start()
    {
        
    }
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        if (!isDie)
        {
            Vector2 dirVec = target.position - rigid.position;
            Vector2 chasingDir = dirVec.normalized * speed * Time.fixedDeltaTime;
            rigid.MovePosition(rigid.position + chasingDir);
            rigid.velocity = Vector2.zero;
        }
    }
    private void LateUpdate()
    {
        if (!isDie)
        {
            sprite.flipX = target.position.x < rigid.position.x;
        }
    }
    void OnEnable()
    {
        target = GameManager.instance.player.GetComponent<Rigidbody2D>();
        isDie = false;
        health = maxHealth;
    }
    public void GetInfo(SpawnData data)     // 적 생성시 상태 초기화
    {
        anim.runtimeAnimatorController = animCtrl[data.Type];
        speed = data.speed;
        maxHealth = data.health;
        health = data.health;
    }
}
