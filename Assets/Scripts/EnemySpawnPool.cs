using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnPool : MonoBehaviour
{
    public GameObject[] enemyObject;    // ������ ����

    List<GameObject>[] enemyPool;       // ������ Ǯ �뵵
    void Awake()
    {
        enemyPool = new List<GameObject>[enemyObject.Length];

        for( int i = 0; i< enemyPool.Length; i++)
        {
            enemyPool[i] = new List<GameObject>();
        }
    }
    public GameObject Spawn(int i)  // ��Ȱ��ȭ�� ���ӿ�����Ʈ ����
    {
        GameObject active = null;

        foreach(GameObject item in enemyPool[i])
        {
            if (!item.activeSelf)   // ��Ȱ��ȭ�� ������Ʈ�� ������ active�� �Ҵ�
            {
                active = item;
                active.SetActive(true);
                break;
            }
        }
        if(!active)        // ��Ȱ��ȭ�� ������Ʈ�� ������ ���� ����� active�� �Ҵ�
        {
            active = Instantiate(enemyObject[i], transform);
            enemyPool[i].Add(active);
        }
        return active;
    }
}
