using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpItemPool : MonoBehaviour        // ����ġ ���� Ǯ�� Ŭ����
{
    public GameObject[] expObject;    // ����ġ ������

    List<GameObject>[] expPool;       // Ǯ �뵵
    void Awake()
    {
        expPool = new List<GameObject>[expObject.Length];

        for (int i = 0; i < expPool.Length; i++)
        {
            expPool[i] = new List<GameObject>();
        }
    }
    public GameObject GetExpItem(int i)  // ��Ȱ��ȭ ���ӿ�����Ʈ ����
    {
        GameObject active = null;
        foreach (GameObject item in expPool[i])
        {
            if (!item.activeSelf)   // ��Ȱ��ȭ ������Ʈ�� ������ active�� �Ҵ�
            {
                active = item;
                active.SetActive(true);
                break;
            }
        }
        if (!active)        // ��Ȱ��ȭ ������Ʈ�� ������ ���� active�� �Ҵ�
        {
            active = Instantiate(expObject[i], transform);
            expPool[i].Add(active);
        }
        return active;
    }
}
