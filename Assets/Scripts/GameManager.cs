using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Player player;
    public static GameManager instance;
    public EnemySpawnPool enemySpawnPool;
    [Header("PlaterInfo")]
    public int level;
    public int kills;
    public int exp;
    public int[] nextExp = { 10, 30, 60, 100, 150, 210, 280, 360, 450, 600 };
    public int health;
    public int maxHealth = 100;
    [Header("Control")]
    public float gameTime;
    public float maxGameTime = 2 * 10f;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        health = maxHealth;
    }
    void Update()
    {
        gameTime += Time.deltaTime;

        if (gameTime > maxGameTime)
        {
            gameTime = maxGameTime;
        }
    }
    public void GetExp()
    {
        exp++;
        if(exp == nextExp[level])
        {
            level++;
            exp = 0;
        }
    }
}
