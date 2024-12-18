using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public Vector2 inputVec;
    public float speed;
    public EnemyScanner scanner;
    Rigidbody2D rigid;
    SpriteRenderer sprite;
    Animator anim;

    public WeaponFlip[] hand;
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        scanner = GetComponent<EnemyScanner>();
        hand = GetComponentsInChildren<WeaponFlip>(true);
    }
    void OnMove(InputValue value)
    {
        inputVec = value.Get<Vector2>();
    }

    private void FixedUpdate()
    {
        if(GameManager.instance.isLive == true)
        {
            Vector2 nextVec = inputVec.normalized * speed * Time.fixedDeltaTime;
            rigid.MovePosition(rigid.position + nextVec);
        }
    }
    private void LateUpdate()
    {
        if (GameManager.instance.isLive == true)
        {
            anim.SetFloat("Speed", inputVec.magnitude);
            if (inputVec.x != 0)
            {
                sprite.flipX = inputVec.x < 0;
            }
        }
    }
}
