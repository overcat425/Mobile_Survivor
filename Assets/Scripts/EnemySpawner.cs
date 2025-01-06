using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public RectTransform bossTextrect;
    Transform bossTrans;

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
        Invoke("BossSpawn", 419f);      // 7분째에 보스출현
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
        GameObject enemy = GameManager.instance.enemySpawnPool.Spawn(0);
        enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;
        enemy.GetComponent<Enemy>().GetInfo(spawnData[5]);
        bossTrans = enemy.transform;
        StartCoroutine("Boss");
    }
    IEnumerator Boss()          // 보스 등장 씬
    {
        GameManager.instance.vignette.intensity.value = 1f;  // 초점 포커스 연출
        StartCoroutine("BossText");     // 텍스트액션
        GameManager.instance.isLive = false;
        GameManager.instance.vCam.Follow = bossTrans.transform; // 카메라->보스
        SoundManager.instance.PlayEffect(SoundManager.Effect.Boss); // 보스등장 이펙트사운드
        yield return new WaitForSecondsRealtime(2.5f);              // 보스 2.5초동안 보여주기
        GameManager.instance.vCam.Follow = target.transform;    // 다시 카메라->플레이어
        GameManager.instance.vignette.intensity.value = 0.44f;      // 초점 포커스 풀기
        GameManager.instance.isLive = true;
    }
    IEnumerator BossText()          // 보스 등장시 텍스트액션
    {
        bossTextrect.localScale = Vector3.one;
        yield return new WaitForSeconds(0.5f);
        bossTextrect.DOAnchorPosX(0, 0.7f).SetEase(Ease.OutSine);
        yield return new WaitForSeconds(1.5f);      // DoTween으로 효과 추가
        bossTextrect.DOAnchorPosX(140, 0.6f).SetEase(Ease.InSine);
        yield return new WaitForSeconds(0.6f);
        bossTextrect.localScale = Vector3.zero;
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