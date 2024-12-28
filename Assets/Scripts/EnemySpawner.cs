using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public SpawnData[] spawnData;
    public Transform[] spawnPoint;
    float timer;
    int level;

    void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>();
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
}
[System.Serializable]
public class SpawnData              // 적 오브젝트 속성타입 직렬화클래스
{
    public int Type;
    public float spawnTime;
    public int health;
    public float speed;
}