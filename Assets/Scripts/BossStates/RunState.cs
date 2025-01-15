using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunState : MonoBehaviour, State
{
    Animator animator;
    public Rigidbody2D target;
    
    private BossScript bossScript;

    public void EnterState()
    {
        if(!animator) animator = GetComponent<Animator>();
        if(!bossScript) bossScript = GetComponent<BossScript>();
        if (!target) target = GameManager.instance.player.GetComponent<Rigidbody2D>();
        animator.SetBool("Run", true);
    }
    public void UpdateState()
    {
        if (GameManager.instance.isLive == true)
        {
            if (!bossScript.isDie && !bossScript.anim.GetCurrentAnimatorStateInfo(0).IsName("Enemy Hit"))
            {                           // 살아있고 경직상태가 아닐때 적 추적
                Vector2 dirVec = target.position - bossScript.rigid.position;
                Vector2 chasingDir = dirVec.normalized * bossScript.speed * Time.fixedDeltaTime;
                bossScript.rigid.MovePosition(bossScript.rigid.position + chasingDir);
                bossScript.rigid.velocity = Vector2.zero;
            }
        }
        Debug.Log("RunState");
    }
    public void ExitState()
    {
        animator.SetBool("Run", false);
    }
}
