using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySpawner : MonoBehaviour
{
    public GameObject bossPrefab;
    public GameObject boss;
    public GameObject bossHp;

    public RectTransform[] bossTextrect;
    public Transform[] eliteTrans;             // 0은 엘리트, 1은 보스
    public GameObject eliteHp;
    public GameObject eliteObject;       // 엘리트 소환시에 태그를 달아놓고 그 보스오브젝트에서 체력값을 빼옴

    public SpawnData[] spawnData;
    public Transform[] spawnPoint;
    float timer;
    int level;
    public Rigidbody2D target;

    void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>();
        target = GameManager.instance.player.GetComponent<Rigidbody2D>();
    }
    private void OnEnable()
    {
        Invoke("EliteSpawn", 299f);      // 5분째에 엘리트출현
        Invoke("BossSpawn", 422f);      // 7분째에 보스출현
    }
    void Update()
    {
        if (GameManager.instance.isLive == true)
        {                                       // level : 적 난이도 조절용 웨이브레벨
            timer += Time.deltaTime;
            level = Mathf.FloorToInt(5 - ((GameManager.instance.maxGameTime - GameManager.instance.gameTime) / 120));
            if(timer > spawnData[level].spawnTime)
            {                           // 아직 진행중이면 적 오브젝트 소환
                Spawn();
                timer = 0f;
            }
        }
    }

    void Spawn()
    {
        switch (level)  // 웨이브레벨이 1이면 기본 몬스터만 소환하고, 2부터는 몬스터가 더 많이 출몰(DoubleSpawn())
        {
            case 0:
                GameObject enemy = GameManager.instance.enemySpawnPool.Spawn(0);
                enemy.transform.position = spawnPoint[Random.Range(1,spawnPoint.Length)].position;
                enemy.GetComponent<Enemy>().GetInfo(spawnData[level]);
                break;
            default:
                DoubleSpawn();
                break;
        }
    }
    void DoubleSpawn()
    {
        for (int i = 0; i < 2; i++)     // 2가지 몬스터 + 2배로 출몰
        {
            GameObject enemy = GameManager.instance.enemySpawnPool.Spawn(0);
            enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;
            enemy.GetComponent<Enemy>().GetInfo(spawnData[level - i]);
        }
    }
    void BossSpawn()
    {
        boss = Instantiate(bossPrefab, spawnPoint[Random.Range(1, spawnPoint.Length)]);
        eliteTrans[1] = boss.transform;
        StartCoroutine(Elite(1));
    }
    void EliteSpawn()
    {
        GameObject enemy = GameManager.instance.enemySpawnPool.Spawn(0);
        enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;
        enemy.GetComponent<Enemy>().GetInfo(spawnData[5]);
        eliteTrans[0] = enemy.transform;
        eliteObject = enemy;
        enemy.AddComponent<HpUiScript>();
        eliteHp.SetActive(true);
        StartCoroutine(Elite(0));
    }
    IEnumerator Elite(int i)          // 엘리트 및 보스 등장 씬
    {
        GameManager.instance.vignette.intensity.value = 1f;  // 초점 포커스 연출
        StartCoroutine(EliteText(i));     // 텍스트액션
        GameManager.instance.isLive = false;
        GameManager.instance.vCam.Follow = eliteTrans[i].transform; // 카메라->몬스터
        SoundManager.instance.PlayEffect(SoundManager.Effect.Elite); // 등장 이펙트사운드
        yield return new WaitForSecondsRealtime(2.5f);              // 2.5초동안 보여주기
        GameManager.instance.vCam.Follow = target.transform;    // 다시 카메라->플레이어
        GameManager.instance.vignette.intensity.value = 0.44f;      // 초점 포커스 풀기
        GameManager.instance.isLive = true;
    }
    IEnumerator EliteText(int i)          // 엘리트 및 보스 등장시 텍스트액션
    {
        bossTextrect[i].gameObject.SetActive(true);      // 텍스트 ON
        yield return new WaitForSeconds(0.5f);
        bossTextrect[i].DOAnchorPosX(0, 0.7f).SetEase(Ease.OutSine);
        yield return new WaitForSeconds(1.5f);      // DoTween으로 지나가는 효과
        bossTextrect[i].DOAnchorPosX(140, 0.6f).SetEase(Ease.InSine);
        yield return new WaitForSeconds(0.6f);
        bossTextrect[i].gameObject.SetActive(false); // 텍스트 OFF
    }
}
[System.Serializable]
public class SpawnData              // 적 오브젝트 속성타입 직렬화클래스
{
    public int Type;
    public float spawnTime;
    public int health;
    public float speed;
}