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
        {                                       // level : �� ���̵� ������ ���̺극��
            timer += Time.deltaTime;
            level = Mathf.FloorToInt(5 - ((GameManager.instance.maxGameTime - GameManager.instance.gameTime) / 120));
            if(timer > spawnData[level].spawnTime)
            {                           // ���� �������̸� �� ������Ʈ ��ȯ
                Spawn();
                timer = 0f;
            }
        }
    }
    void Spawn()
    {
        switch (level)  // ���̺극���� 1�̸� �⺻ ���͸� ��ȯ�ϰ�, 2���ʹ� ���Ͱ� �� ���� ���(DoubleSpawn())
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
public class SpawnData              // �� ������Ʈ �Ӽ�Ÿ�� ����ȭŬ����
{
    public int Type;
    public float spawnTime;
    public int health;
    public float speed;
}