using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Player player;
    public static GameManager instance;
    public EnemySpawnPool enemySpawnPool;
    public LevelUp lvupUi;
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
    public bool isLive;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        health = maxHealth;
        lvupUi.InitAttack(0);
    }
    void Update()
    {
        if (isLive == true)
        {
            gameTime += Time.deltaTime;

            if (gameTime > maxGameTime)
            {
                gameTime = maxGameTime;
            }
        }

    }
    public void GetExp()
    {
        exp++;
        if(exp == nextExp[Mathf.Min(level, nextExp.Length-1)])
        {
            level++;
            exp = 0;
            lvupUi.Show();
        }
    }
    public void Stop()
    {
        isLive = false;
        Time.timeScale = 0;
    }
    public void Resume()
    {
        isLive = true;
        Time.timeScale = 1;
    }
}
