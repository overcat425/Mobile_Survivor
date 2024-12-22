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

        if (!PlayerPrefs.HasKey("GameData"))        // ������ ����� �����Ͱ� ������ �ʱ�ȭ
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
        for (int i = 0; i < lockedCharacter.Length; i++)    // GameData�� �ִ� �����͸� �ҷ��ͼ�
        {                                                               // �رݿ��� Ȯ�� �� ĳ���� ����
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
        switch (unlock)
        {
            case Unlock.UnlockSergeant:
                isUnlock = GameManager.instance.gameTime == GameManager.instance.maxGameTime;
                break;
            case Unlock.UnlockGeneral:
                isUnlock = GameManager.instance.kills >= 100;
                break;
        }
        if (isUnlock && PlayerPrefs.GetInt(unlock.ToString()) == 0) // �ر� ���� �޼��ϸ�
        {
            PlayerPrefs.SetInt(unlock.ToString(), 1);   // �ر��ߴٰ� ����
            for (int i = 0; i < notice.transform.childCount; i++)
            {
                bool isActive = i == (int)unlock;       // 
                notice.transform.GetChild(i).gameObject.SetActive(isActive);
            }
            StartCoroutine("Notice");
        }
    }
    IEnumerator Notice()
    {
        notice.SetActive(true);     // �˸�ON
        yield return wait;           // 5����
        notice.SetActive(false);    // �˸�OFF
    }
}
