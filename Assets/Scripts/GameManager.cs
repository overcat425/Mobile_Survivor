using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GameManager : MonoBehaviour
{
    public CinemachineVirtualCamera vCam;

    public Player player;
    public static GameManager instance;
    public EnemySpawnPool enemySpawnPool;
    public EnemySpawner enemySpawner;
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
    public bool isHitable;      // �������� ��ȭâ ������  0.7�ʵ��� ����
    public GameObject pauseUi;
    public GameResult resultUi;
    public GameObject enemyCleaner;
    public GameObject mainBtn;
    public GameObject characters;
    public Transform joyStick;
    public int clickCount;

    [Header("Exp")]
    public GameObject expPrefab;
    public Transform expParent;
    public Transform expStart;
    public Transform expEnd;
    public int particleAmount;
    public float particleDelay;
    public float moveDuration;
    public Ease moveEase;
    public Image expImage;
    public AnimationCurve expGlow;

    public Volume volume;
    public Vignette vignette;

    private void Awake()
    {
        instance = this;
        Application.targetFrameRate = 60;           // ������ 60 ����
        isHitable = true;
    }
    private void Start()
    {
        volume.profile.TryGet(out vignette);
        SoundManager.instance.PlayBgm(false);       // ���� ���۽� ��ȭ�ο� ���
    }
    public void GameStart(int id)   // ĳ���� id �޾Ƽ� �� ĳ���ͷ� ����
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
    void Update()               // ���� ����ð�
    {
        if (isLive == true)
        {
            gameTime += Time.deltaTime;
            StartCoroutine("Pause");
            if (gameTime > maxGameTime)     // ���� ����
            {
                gameTime = maxGameTime;
                GameClear();
            }
        }
        else if (isLive == false && pauseUi.activeSelf == false)
        {
            StartCoroutine("DoubleClickQuit");
        }
    }
    public void GetExp()
    {
        if (isLive == true)
        {
            for(int i = 0; i < particleAmount; i++)     // ����ġ������ n���� �ɰ���
            {
                var targetDelay = i * particleDelay;
                GetExpItem(targetDelay);
            }
            StartCoroutine("ExpUpGlow");
            exp++;
            if(exp == nextExp[Mathf.Min(level, nextExp.Length-1)])
            {                                   // ������ ����
                level++;
                exp = 0;
                lvupUi.Show();
            }
        }
    }
    public void GetExpItem(float delay)
    {
        var expItem = expItemPool.GetExpItem(0);   // ������Ʈ Ǯ ����Ʈ���� Ȱ��ȭ
        var randomPos = new Vector3(Random.Range(-50f, 50f), Random.Range(-50f, 50f), 0f);  // �÷��̾� ��ó ������ǥ
        Vector3 startPos = randomPos + expStart.transform.position; // ����ġ ���� ������������ ���������� ��ǥ�� ���� �������� Ƣ����� ȿ��
        expItem.transform.position = startPos;
        expItem.transform.DOMove(expEnd.position, moveDuration).SetEase(moveEase).SetDelay(delay);
    }                       // DoTween���� ���������� ����ȿ��
    public void Stop()                  // ���� ����
    {
        isLive = false;
        Time.timeScale = 0;
        joyStick.localScale = Vector3.zero;
    }
    public void Resume()                // ���� �簳
    {
        isLive = true;
        Time.timeScale = 1;
        joyStick.localScale = Vector3.one;
        SoundManager.instance.StopBgm(false);
        pauseUi.SetActive(false);
    }
    IEnumerator Pause()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                pauseUi.SetActive(true);
                SoundManager.instance.StopBgm(true);
                joyStick.localScale = Vector3.zero;
                Stop();
            }
        }
        yield return null;
    }
    IEnumerator DyingAnim()             // ���ӿ��� �ڷ�ƾ
    {
        isLive = false;
        resultUi.gameObject.SetActive(true);
        resultUi.Defeat();
        yield return new WaitForSeconds(1.5f);
        Stop();
        SoundManager.instance.StopBgm(true);
        SoundManager.instance.PlayEffect(SoundManager.Effect.GameOver);
    }
    IEnumerator ClearAnim()             // ���� Ŭ���� �ڷ�ƾ
    {
        isLive = false;
        enemyCleaner.SetActive(true);
        resultUi.gameObject.SetActive(true);
        resultUi.Clear();
        yield return new WaitForSeconds(1.5f);
        Stop();
        SoundManager.instance.StopBgm(true);
        SoundManager.instance.PlayEffect(SoundManager.Effect.Win);
    }
    IEnumerator GoMenu()
    {       // �̹� Gameover�� Win���� timescale�� 0�̹Ƿ� Realtime���
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
    IEnumerator ExpUpGlow()     // ����ġ ȹ��� ����ġ�� ����
    {
        float percent = 0;
        while (percent < 1)
        {
            percent += Time.deltaTime;
            Color color = expImage.color;
            color.g = Mathf.Lerp(0f, 1f, expGlow.Evaluate(percent));
            expImage.color = color;
            yield return null;
        }
    }
    IEnumerator DoubleClickQuit()           // �ڷΰ��� ������ġ�� ����
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                clickCount++;
                if (!IsInvoking("DoubleClick")) Invoke("DoubleClick", 0.3f);    // ����Ŭ�� ���ѽð�
            }else if(clickCount == 2)
            {
                CancelInvoke("DoubleClick");
                QuitGame();
            }
        }
        yield return null;
    }
    public void DoubleClick()
    {
        clickCount = 0;
    }
}