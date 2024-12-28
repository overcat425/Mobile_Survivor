using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnPool : MonoBehaviour
{
    public GameObject[] enemyObject;    // ������ ����

    List<GameObject>[] enemyPool;       // ������ Ǯ �뵵
    void Awake()
    {                           // Ǯ������Ʈ enemyPool
        enemyPool = new List<GameObject>[enemyObject.Length];
        for( int i = 0; i< enemyPool.Length; i++)
        {
            enemyPool[i] = new List<GameObject>();
        }
    }
    public GameObject Spawn(int i)  // ��Ȱ��ȭ�� ���ӿ�����Ʈ ����
    {
        GameObject active = null;       // Ȱ��ȭ ������Ʈ �ʱ�ȭ

        foreach(GameObject item in enemyPool[i])
        {
            if (!item.activeSelf)   // ��Ȱ��ȭ�� ������Ʈ�� ������ active�� �Ҵ�
            {
                active = item;
                active.SetActive(true);     // ��Ȱ��
                break;
            }
        }
        if(!active)        // ��Ȱ��ȭ�� ������Ʈ�� ������ ���� ����� active�� �Ҵ�
        {
            active = Instantiate(enemyObject[i], transform);
            enemyPool[i].Add(active);       // Ǯ ����Ʈ�� ���
        }
        return active;
    }
}
