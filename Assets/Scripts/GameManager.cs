using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public float health;
    public float maxHealth = 100;
    [Header("Control")]
    public float gameTime;
    public float maxGameTime = 2 * 10f;
    public bool isLive;
    public GameResult resultUi;
    public GameObject enemyCleaner;
    private void Awake()
    {
        instance = this;
    }
    public void GameStart()
    {
        health = maxHealth;
        lvupUi.InitAttack(0);
        Resume();
    }
    public void GameOver()
    {
        StartCoroutine("DyingAnim");
    }
    public void GameClear()
    {
        StartCoroutine("ClearAnim");
    }
    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
    void Update()
    {
        if (isLive == true)
        {
            gameTime += Time.deltaTime;

            if (gameTime > maxGameTime)     // 생존 성공
            {
                gameTime = maxGameTime;
                GameClear();
            }
        }

    }
    public void GetExp()
    {
        if (isLive == true)
        {
            exp++;
            if(exp == nextExp[Mathf.Min(level, nextExp.Length-1)])
            {
                level++;
                exp = 0;
                lvupUi.Show();
            }
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
    IEnumerator DyingAnim()
    {
        isLive = false;
        yield return new WaitForSeconds(0.5f);
        resultUi.gameObject.SetActive(true);
        resultUi.Defeat();
        Stop();
    }
    IEnumerator ClearAnim()
    {
        isLive = false;
        enemyCleaner.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        resultUi.gameObject.SetActive(true);
        resultUi.Clear();
        Stop();
    }
}
