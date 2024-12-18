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
        {
            timer += Time.deltaTime;
            level = Mathf.Min(Mathf.FloorToInt(GameManager.instance.gameTime / 10f), spawnData.Length -1);
            if(timer > spawnData[level].spawnTime)
            {
                Spawn();
                timer = 0f;
            }
        }
    }
    void Spawn()
    {
        GameObject enemy = GameManager.instance.enemySpawnPool.Spawn(0);
        enemy.transform.position = spawnPoint[Random.Range(1,spawnPoint.Length)].position;
        enemy.GetComponent<Enemy>().GetInfo(spawnData[level]);
    }
}
[System.Serializable]
public class SpawnData
{
    public int Type;
    public float spawnTime;
    public int health;
    public float speed;
}