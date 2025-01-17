using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using static UnityEditor.PlayerSettings;
public interface State      // 각 행동 상태를 나타내는 인터페이스
{
    public void EnterState();
    public void UpdateState();
    public void ExitState();
}
public class BossScript : MonoBehaviour
{
    public Rigidbody2D target;          // 플레이어 rigid
    public Rigidbody2D rigid;           // 보스 rigid
    SpriteRenderer sprite;
    public Animator anim;
    Collider2D collider;

    [Header("BossStatus")]
    public float speed;
    public float health;
    public float maxHealth;
    public bool isDie;
    bool isAttack;

    [Header("FSM")]
    [SerializeField] private RunState runState;
    [SerializeField] private AttackState attackState;
    [SerializeField] private DeadState deadState;

    private BossFSM bossFSM;
    private void Awake()
    {
        bossFSM = new BossFSM(this);
        bossFSM.ChangeState(runState);
        collider = GetComponent<Collider2D>();
        sprite = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }
    void OnEnable()
    {
        isDie = false;
        isAttack = false;
        collider.enabled = true;
        rigid.simulated = true;
        sprite.sortingOrder = 2;
        //anim.SetBool("Dead", false);
        health = maxHealth;
        GameManager.instance.enemySpawner.bossHp.SetActive(true);
    }
    void Start()
    {
        target = GameManager.instance.player.GetComponent<Rigidbody2D>();
        StartCoroutine("StateUpdate");
    }
    void Update()       // 공격중or생사 상태에 따라 유한상태패턴 전환
    {
        if(isDie)UpdateState(Estate.Dead);

        bossFSM.currentState.UpdateState();  // 현재 상태의 Update메소드 호출해,
    }                                                       // 상황에 맞게 행동
    IEnumerator StateUpdate()
    {
        if (!isAttack && !isDie) UpdateState(Estate.Run);
        if (isAttack && !isDie) UpdateState(Estate.Attack);
        yield return new WaitForSeconds(0.5f);
        StartCoroutine("StateUpdate");
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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Magnet")) isAttack = true; // 보스 공격범위가 되면 공격
        if (collision.CompareTag("Bullet") && !isDie)        // 총알에 피격시
        {
            health -= collision.GetComponent<Bullet>().damage;
            if (health > 0)                         // 살아있으면 피격판정 후 효과음
            {
                if(!isAttack) anim.SetTrigger("Hit"); // 공격 시전중이 아닐때만 경직 트리거 발동
                SoundManager.instance.PlayEffect(SoundManager.Effect.Hit);
            }else
            {                                       // 죽음
                isDie = true;
                collider.enabled = false;
                rigid.simulated = false;
                sprite.sortingOrder = 1;
                if (GameManager.instance.isLive)
                {
                    SoundManager.instance.PlayEffect(SoundManager.Effect.BossDie);
                }
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Magnet")) isAttack = false;
    }
    private void UpdateState(Estate state)
    {
        switch (state)
        {
            case Estate.Run:
                bossFSM.ChangeState(runState);
                break;
            case Estate.Attack:
                bossFSM.ChangeState(attackState);
                break;
            case Estate.Dead:
                bossFSM.ChangeState(deadState);
                break;
        }
    }
    void Dead()
    {
        gameObject.SetActive(false);
    }
}
