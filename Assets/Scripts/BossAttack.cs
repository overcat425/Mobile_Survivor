using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttack : MonoBehaviour
{
    Animator anim;
    void OnEnable()
    {
        StartCoroutine("Particle");
    }
    IEnumerator Particle()
    {
        yield return new WaitForSeconds(0.5f);
        StopCoroutine("Particle");
        gameObject.SetActive(false);
    }
}