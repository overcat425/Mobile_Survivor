using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpUiScript : MonoBehaviour
{
    public static HpUiScript instance;
    public Enemy enemy;
    public float bossHealth;
    public float bossMaxHealth;
    private void Start()
    {
        enemy = GetComponent<Enemy>();  // Boss에 달린 enemy스크립트
    }
    void LateUpdate()
    {
        bossHealth = enemy.health;
        bossMaxHealth = enemy.maxHealth;
    }
}