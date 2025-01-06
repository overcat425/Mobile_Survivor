using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpItemPool : MonoBehaviour        // 경험치 구슬 풀링 클래스
{
    public GameObject[] expObject;    // 경험치 프리팹

    List<GameObject>[] expPool;       // 풀 용도
    void Awake()
    {
        expPool = new List<GameObject>[expObject.Length];

        for (int i = 0; i < expPool.Length; i++)
        {
            expPool[i] = new List<GameObject>();
        }
    }
    public GameObject GetExpItem(int i)  // 비활성화 게임오브젝트 관리
    {
        GameObject active = null;
        foreach (GameObject item in expPool[i])
        {
            if (!item.activeSelf)   // 비활성화 오브젝트가 있으면 active에 할당
            {
                active = item;
                active.SetActive(true);
                break;
            }
        }
        if (!active)        // 비활성화 오브젝트가 없으면 새로 active에 할당
        {
            active = Instantiate(expObject[i], transform);
            expPool[i].Add(active);
        }
        return active;
    }
}
