using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AttackState : MonoBehaviour, State
{
    Animator animator;
    [SerializeField] GameObject[] particle;
    public Rigidbody2D target;
    private BossScript bossScript;
    float coolTime;
    public int rand = 0;
    bool canAttack;
    public void EnterState()
    {
        if (!animator) animator = GetComponent<Animator>();
        if (!bossScript) bossScript = GetComponent<BossScript>();
    }
    public void UpdateState()
    {
        if (!target) target = GameManager.instance.player.GetComponent<Rigidbody2D>();
        coolTime += Time.deltaTime;
        if (coolTime > 1f)
        {
            canAttack = true;
            coolTime = 0;
        }
        if (canAttack)          // 공격 가능하면
        {
            rand = Random.Range(0, 3);
            switch (rand)        // 3가지 공격패턴 중 랜덤으로 마법공격
            {
                case 0:
                    animator.SetTrigger("Attack");
                    break;
                case 1:
                    animator.SetTrigger("Attack 2");
                    break;
                case 2:
                    animator.SetTrigger("Attack 3");
                    break;
            }Particle(rand);        // 공격 마법 오브젝트
            canAttack = false;
            SoundManager.instance.PlayEffect(SoundManager.Effect.BossAttack);
        }
    }
    public void Particle(int rand)  // 공격 오브젝트도 오브젝트풀링으로 관리
    {
        Vector3 dirVec = (target.position - bossScript.rigid.position) * 0.2f;
        GameObject enemy = GameManager.instance.enemySpawnPool.Spawn(rand+4);
        enemy.transform.position = target.transform.position - dirVec;
    }       // 플레이어보다 약간 안쪽으로 공격해서 바깥쪽으로 밀어내도록 설계
    public void ExitState()
    {
    }

}
