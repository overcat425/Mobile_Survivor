using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public float speed;
    public Rigidbody2D target;
    bool isDie = false;
    Rigidbody2D rigid;
    SpriteRenderer sprite;
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
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
}
