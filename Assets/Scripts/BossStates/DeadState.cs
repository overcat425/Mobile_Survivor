using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : MonoBehaviour, State
{
    Animator animator;
    public void EnterState()
    {
        GameManager.instance.kills++;
        if (!animator) animator = GetComponent<Animator>();
        animator.SetBool("Death", true);
    }
    public void UpdateState()
    {
        StartCoroutine("DestroyBoss");
    }
    public void ExitState(){}
    IEnumerator DestroyBoss()
    {
        yield return new WaitForSeconds(2.2f);
        gameObject.SetActive(false);
    }
}