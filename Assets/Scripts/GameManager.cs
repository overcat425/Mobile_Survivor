using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Player player;
    public static GameManager instance;
    public EnemySpawnPool enemySpawnPool;
    public ExpItemPool expItemPool;
    public LevelUp lvupUi;
    [Header("PlayerInfo")]
    public int level;
    public int kills;
    public int exp;
    public int[] nextExp = { 10, 30, 60, 100, 150, 210, 280, 360, 450, 600 };
    public float health;
    public float maxHealth = 100;
    public int playerId;

    [Header("Control")]
    public float gameTime;
    public float maxGameTime;
    public bool isLive;
    public GameResult resultUi;
    public GameObject enemyCleaner;
    public GameObject mainBtn;
    public GameObject characters;
    public Transform joyStick;

    [Header("Exp")]
    public GameObject expPrefab;
    public Transform expParent;
    public Transform expStart;
    public Transform expEnd;
    public int particleAmount;
    public float particleDelay;
    public float moveDuration;
    public Ease moveEase;
    private void Awake()
    {
        instance = this;
        Application.targetFrameRate = 60;
    }
    private void Start()
    {
        SoundManager.instance.PlayBgm(false);
    }
    public void GameStart(int id)
    {
        playerId = id;
        health = maxHealth;

        player.gameObject.SetActive(true);
        lvupUi.InitAttack(playerId % 2);
        Resume();

        SoundManager.instance.PlayBgm(true);
        SoundManager.instance.PlayEffect(SoundManager.Effect.Select);
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
        StartCoroutine("GoMenu");
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
            for(int i = 0; i < particleAmount; i++)
            {
                var targetDelay = i * particleDelay;
                GetExpItem(targetDelay);
            }
            exp++;
            if(exp == nextExp[Mathf.Min(level, nextExp.Length-1)])
            {
                level++;
                exp = 0;
                lvupUi.Show();
            }
        }
    }
    public void GetExpItem(float delay)
    {
        var expItem = expItemPool.GetExpItem(0);
        var randomPos = new Vector3(Random.Range(-50f, 50f), Random.Range(-50f, 50f), 0f);
        Vector3 startPos = randomPos + expStart.transform.position;
        expItem.transform.position = startPos;
        expItem.transform.DOMove(expEnd.position, moveDuration).SetEase(moveEase).SetDelay(delay);
    }
    public void Stop()
    {
        isLive = false;
        Time.timeScale = 0;
        joyStick.localScale = Vector3.zero;
    }
    public void Resume()
    {
        isLive = true;
        Time.timeScale = 1;
        joyStick.localScale = Vector3.one;
    }
    IEnumerator DyingAnim()
    {
        isLive = false;
        yield return new WaitForSeconds(0.5f);
        resultUi.gameObject.SetActive(true);
        resultUi.Defeat();
        Stop();
        SoundManager.instance.StopBgm(true);
        SoundManager.instance.PlayEffect(SoundManager.Effect.GameOver);
    }
    IEnumerator ClearAnim()
    {
        isLive = false;
        enemyCleaner.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        resultUi.gameObject.SetActive(true);
        resultUi.Clear();
        Stop();
        SoundManager.instance.StopBgm(true);
        SoundManager.instance.PlayEffect(SoundManager.Effect.Win);
    }
    IEnumerator GoMenu()
    {       // 이미 Gameover나 Win으로 timescale이 0이므로 Realtime사용
        yield return new WaitForSecondsRealtime(0.4f);
        SceneManager.LoadScene(0);
    }
    public void SelectChar()
    {
        mainBtn.SetActive(false);
        characters.SetActive(true);
    }
    public void GoBack()
    {
        characters.SetActive(false);
        mainBtn.SetActive(true);

    }
    public void QuitGame()
    {
        Application.Quit();
    }
}