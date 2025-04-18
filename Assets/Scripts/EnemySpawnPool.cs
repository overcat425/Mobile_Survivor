using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnPool : MonoBehaviour
{
    public GameObject[] enemyObject;    // 프리팹 변수

    List<GameObject>[] enemyPool;       // 프리팹 풀 용도
    void Awake()
    {                           // 풀링리스트 enemyPool
        enemyPool = new List<GameObject>[enemyObject.Length];
        for( int i = 0; i< enemyPool.Length; i++)
        {
            enemyPool[i] = new List<GameObject>();
        }
    }
    public GameObject Spawn(int i)  // 비활성화된 게임오브젝트 관리
    {
        GameObject active = null;       // 활성화 오브젝트 초기화
        foreach(GameObject item in enemyPool[i])
        {
            if (!item.activeSelf)   // 비활성화된 오브젝트가 있으면 active에 할당
            {
                active = item;
                active.SetActive(true);     // 재활용
                break;
            }
        }
        if(!active)        // 비활성화된 오브젝트가 없으면 새로 만들고 active에 할당
        {
            active = Instantiate(enemyObject[i], transform);
            enemyPool[i].Add(active);       // 풀 리스트에 등록
        }
        return active;
    }
}
