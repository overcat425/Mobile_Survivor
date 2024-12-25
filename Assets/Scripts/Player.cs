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
    public RuntimeAnimatorController[] animCon;
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        scanner = GetComponent<EnemyScanner>();
        hand = GetComponentsInChildren<WeaponFlip>(true);
    }
    private void OnEnable()
    {
        speed *= Character.Speed;       // 캐릭터별 이동속도 부여
        anim.runtimeAnimatorController = animCon[GameManager.instance.playerId];
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
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (GameManager.instance.isLive)            // 피격시 프레임수준에서 체력감소
        {
            GameManager.instance.health -= Time.deltaTime * 10;
        }
        if (GameManager.instance.health < 0)            // 사망시 무기 비활성화 후 묘비애니메이션
        {
            for (int i = 2; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
            anim.SetTrigger("Dead");
            GameManager.instance.GameOver();
        }
    }
}
