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
        for (int i = 0; i < 2; i++)
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
    IEnumerator Boss()
    {
        GameManager.instance.vignette.intensity.value = 1f;
        StartCoroutine("BossText");
        GameManager.instance.isLive = false;
        GameManager.instance.vCam.Follow = bossTrans.transform;
        SoundManager.instance.PlayEffect(SoundManager.Effect.Boss);
        yield return new WaitForSecondsRealtime(2.5f);
        GameManager.instance.vCam.Follow = target.transform;
        GameManager.instance.vignette.intensity.value = 0.44f;
        GameManager.instance.isLive = true;
    }
    IEnumerator BossText()
    {
        bossTextrect.localScale = Vector3.one;
        yield return new WaitForSeconds(0.5f);
        bossTextrect.DOAnchorPosX(0, 0.7f).SetEase(Ease.OutSine);
        yield return new WaitForSeconds(1.5f);
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