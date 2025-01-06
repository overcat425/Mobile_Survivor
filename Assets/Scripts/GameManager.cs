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
    public bool isHitable;      // 레벨업시 강화창 열리는  0.7초동안 무적
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
        Application.targetFrameRate = 60;           // 프레임 60 고정
        isHitable = true;
    }
    private void Start()
    {
        volume.profile.TryGet(out vignette);
        SoundManager.instance.PlayBgm(false);       // 게임 시작시 평화로운 브금
    }
    public void GameStart(int id)   // 캐릭터 id 받아서 그 캐릭터로 시작
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
    void Update()               // 게임 진행시간
    {
        if (isLive == true)
        {
            gameTime += Time.deltaTime;
            StartCoroutine("Pause");
            if (gameTime > maxGameTime)     // 생존 성공
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
            for(int i = 0; i < particleAmount; i++)     // 경험치구슬이 n개로 쪼개짐
            {
                var targetDelay = i * particleDelay;
                GetExpItem(targetDelay);
            }
            StartCoroutine("ExpUpGlow");
            exp++;
            if(exp == nextExp[Mathf.Min(level, nextExp.Length-1)])
            {                                   // 레벨업 로직
                level++;
                exp = 0;
                lvupUi.Show();
            }
        }
    }
    public void GetExpItem(float delay)
    {
        var expItem = expItemPool.GetExpItem(0);   // 오브젝트 풀 리스트에서 활성화
        var randomPos = new Vector3(Random.Range(-50f, 50f), Random.Range(-50f, 50f), 0f);  // 플레이어 근처 랜덤좌표
        Vector3 startPos = randomPos + expStart.transform.position; // 경험치 파편 생성지점에서 랜덤생성한 좌표를 더해 무작위로 튀어나오는 효과
        expItem.transform.position = startPos;
        expItem.transform.DOMove(expEnd.position, moveDuration).SetEase(moveEase).SetDelay(delay);
    }                       // DoTween으로 목적지까지 추적효과
    public void Stop()                  // 게임 정지
    {
        isLive = false;
        Time.timeScale = 0;
        joyStick.localScale = Vector3.zero;
    }
    public void Resume()                // 게임 재개
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
    IEnumerator DyingAnim()             // 게임오버 코루틴
    {
        isLive = false;
        resultUi.gameObject.SetActive(true);
        resultUi.Defeat();
        yield return new WaitForSeconds(1.5f);
        Stop();
        SoundManager.instance.StopBgm(true);
        SoundManager.instance.PlayEffect(SoundManager.Effect.GameOver);
    }
    IEnumerator ClearAnim()             // 게임 클리어 코루틴
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
    IEnumerator ExpUpGlow()     // 경험치 획득시 경험치바 변색
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
    IEnumerator DoubleClickQuit()           // 뒤로가기 더블터치시 종료
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                clickCount++;
                if (!IsInvoking("DoubleClick")) Invoke("DoubleClick", 0.3f);    // 더블클릭 제한시간
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