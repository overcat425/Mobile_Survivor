using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Processors;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public RuntimeAnimatorController[] animCtrl;
    public float speed;
    public float health;
    public float maxHealth;
    public Transform pos;

    public Rigidbody2D target;
    bool isDie;
    Rigidbody2D rigid;
    Collider2D collider;  //
    SpriteRenderer sprite;
    Animator anim;
    WaitForFixedUpdate wait;

    public GameObject expItem;
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        wait = new WaitForFixedUpdate();
    }
    private void FixedUpdate()
    {
        if (GameManager.instance.isLive == true)
        {
            if (!isDie && !anim.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
            {
                Vector2 dirVec = target.position - rigid.position;
                Vector2 chasingDir = dirVec.normalized * speed * Time.fixedDeltaTime;
                rigid.MovePosition(rigid.position + chasingDir);
                rigid.velocity = Vector2.zero;
            }
        }
    }
    private void LateUpdate()
    {
        if (GameManager.instance.isLive == true)
        {
            if (!isDie)
            {
                sprite.flipX = target.position.x < rigid.position.x;
            }
        }
    }
    void OnEnable()
    {
        target = GameManager.instance.player.GetComponent<Rigidbody2D>();
        isDie = false;
        collider.enabled = true;
        rigid.simulated = true;
        sprite.sortingOrder = 2;
        anim.SetBool("Dead", false);
        health = maxHealth;
    }
    public void GetInfo(SpawnData data)     // 적 생성시 상태 초기화
    {
        anim.runtimeAnimatorController = animCtrl[data.Type];
        speed = data.speed;
        maxHealth = data.health;
        health = data.health;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet")&& !isDie)
        {
            health -= collision.GetComponent<Bullet>().damage;
            StartCoroutine("KnockBack");
            if (health > 0)
            {
                anim.SetTrigger("Hit");
                SoundManager.instance.PlayEffect(SoundManager.Effect.Hit);
            }
            else
            {
                isDie = true;
                collider.enabled = false;
                rigid.simulated = false;
                sprite.sortingOrder = 1;
                anim.SetBool("Dead", true);
                //Dead();     // 애니메이터에서 이벤트함수로 넣음
                GameManager.instance.kills++;
                Drop();
                if (GameManager.instance.isLive)
                {
                    SoundManager.instance.PlayEffect(SoundManager.Effect.Die);
                }
            }
        }
    }
    IEnumerator KnockBack()
    {
        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 knockbackDir = transform.position - playerPos;
        rigid.AddForce(knockbackDir.normalized * 3, ForceMode2D.Impulse);
        yield return wait;
    }
    void Dead()
    {
        gameObject.SetActive(false);
    }
    void Drop()
    {
        GameObject coin = GameManager.instance.enemySpawnPool.Spawn(3);
        coin.transform.position = pos.position;
    }
}
