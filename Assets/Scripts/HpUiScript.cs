using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpUiScript : MonoBehaviour
{
    public static HpUiScript instance;
    public Enemy enemy;
    public float eliteHealth;
    public float eliteMaxHealth;
    private void Start()
    {
        enemy = GetComponent<Enemy>();  // elite에 달린 enemy스크립트
    }
    void LateUpdate()
    {
        eliteHealth = enemy.health;
        eliteMaxHealth = enemy.maxHealth;
        if(eliteHealth <= 0)
        {
            StartCoroutine("EliteDie");
        }
    }
    IEnumerator EliteDie()
    {
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }
}