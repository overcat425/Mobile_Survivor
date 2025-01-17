using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using static UnityEditor.PlayerSettings;
public interface State      // �� �ൿ ���¸� ��Ÿ���� �������̽�
{
    public void EnterState();
    public void UpdateState();
    public void ExitState();
}
public class BossScript : MonoBehaviour
{
    public Rigidbody2D target;          // �÷��̾� rigid
    public Rigidbody2D rigid;           // ���� rigid
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
    void Update()       // ������or���� ���¿� ���� ���ѻ������� ��ȯ
    {
        if(isDie)UpdateState(Estate.Dead);

        bossFSM.currentState.UpdateState();  // ���� ������ Update�޼ҵ� ȣ����,
    }                                                       // ��Ȳ�� �°� �ൿ
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
            {               // �÷��̾� �ٶ󺸱�
                sprite.flipX = target.position.x < rigid.position.x;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Magnet")) isAttack = true; // ���� ���ݹ����� �Ǹ� ����
        if (collision.CompareTag("Bullet") && !isDie)        // �Ѿ˿� �ǰݽ�
        {
            health -= collision.GetComponent<Bullet>().damage;
            if (health > 0)                         // ��������� �ǰ����� �� ȿ����
            {
                if(!isAttack) anim.SetTrigger("Hit"); // ���� �������� �ƴҶ��� ���� Ʈ���� �ߵ�
                SoundManager.instance.PlayEffect(SoundManager.Effect.Hit);
            }else
            {                                       // ����
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
