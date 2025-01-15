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
        if (canAttack)
        {
            //Vector3 dirVec = (target.position - bossScript.rigid.position)*0.2f;
            rand = Random.Range(0, 3);
            switch (rand)
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
            }Particle(rand);
            canAttack = false;
            SoundManager.instance.PlayEffect(SoundManager.Effect.BossAttack);
        }
    }
    public void ExitState()
    {
    }
    public void Particle(int rand)  // 공격 오브젝트도 오브젝트풀링으로 관리
    {
        Vector3 dirVec = (target.position - bossScript.rigid.position) * 0.2f;
        GameObject enemy = GameManager.instance.enemySpawnPool.Spawn(rand+4);
        enemy.transform.position = target.transform.position - dirVec;
    }
}
