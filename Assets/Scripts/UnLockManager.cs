using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnLockManager : MonoBehaviour
{
    public GameObject[] lockedCharacter;
    public GameObject[] unlockedCharacter;
    public GameObject notice;

    WaitForSecondsRealtime wait;
    enum Unlock { UnlockSergeant, UnlockGeneral }
    Unlock[] unlocks;
    private void Awake()
    {
        wait = new WaitForSecondsRealtime(5);
        unlocks = (Unlock[])Enum.GetValues(typeof(Unlock));

        if (!PlayerPrefs.HasKey("GameData"))        // 이전에 저장된 데이터가 없으면 초기화
        {
            Init();
        }
    }

    private void Init()
    {
        PlayerPrefs.SetInt("GameData", 1);
        foreach(Unlock unlock in unlocks)
        {
            PlayerPrefs.SetInt(unlock.ToString(), 0);
        }
    }
    private void UnlockCharacter()
    {
        for (int i = 0; i < lockedCharacter.Length; i++)    // GameData에 있는 데이터를 불러와서
        {                                                               // 해금여부 확인 후 캐릭터 전시
            string unlockName = unlocks[i].ToString();
            bool isUnlock = PlayerPrefs.GetInt(unlockName) == 1;
            lockedCharacter[i].SetActive(!isUnlock);
            unlockedCharacter[i].SetActive(isUnlock);
        }
    }
    void Start()
    {
        UnlockCharacter();
    }
    void LateUpdate()
    {
        foreach (Unlock unlock in unlocks)
        {
            CheckUnlock(unlock);
        }
    }
    void CheckUnlock(Unlock unlock)
    {
        bool isUnlock = false;
        switch (unlock)                     // 해금 조건
        {
            case Unlock.UnlockSergeant:         // 킬 조건 달성시
                isUnlock = GameManager.instance.kills >= 500;
                break;
            case Unlock.UnlockGeneral:          // 1회 클리어시
                isUnlock = GameManager.instance.gameTime == GameManager.instance.maxGameTime;
                break;
        }
        if (isUnlock && PlayerPrefs.GetInt(unlock.ToString()) == 0) // 해금 조건 달성하면
        {
            PlayerPrefs.SetInt(unlock.ToString(), 1);   // 해금했다고 저장
            for (int i = 0; i < notice.transform.childCount; i++)
            {
                bool isActive = i == (int)unlock;       // 
                notice.transform.GetChild(i).gameObject.SetActive(isActive);
            }
            StartCoroutine("Notice");
        }
    }
    IEnumerator Notice()            // 캐릭터 해금 알림
    {
        notice.SetActive(true);     // 알림ON
        SoundManager.instance.PlayEffect(SoundManager.Effect.Notice);
        yield return wait;           // 5초후
        notice.SetActive(false);    // 알림OFF
    }
}
