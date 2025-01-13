using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnLockManager : MonoBehaviour          // �ر� ��ũ��Ʈ
{
    WaitForSecondsRealtime wait;

    public GameObject notice;
    public GameObject[] lockedCharacter;
    public GameObject[] unlockedCharacter;

    enum Unlock { UnlockSergeant, UnlockGeneral }   // ����,���� ����
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
    void CheckUnlock(Unlock index)
    {
        bool isUnlock = false;
        switch (index)                     // �ر� ����
        {
            case Unlock.UnlockSergeant:         // ų ���� �޼��� ���� �ر�
                isUnlock = GameManager.instance.kills >= 1000;
                break;
            case Unlock.UnlockGeneral:          // 1ȸ Ŭ����� ���� �ر� ( ų���δ� 3~4000ų ���� )
                isUnlock = GameManager.instance.gameTime == GameManager.instance.maxGameTime;
                break;
        }
        if (isUnlock && PlayerPrefs.GetInt(index.ToString()) == 0) // �ر� ���� �޼��ϸ�
        {
            PlayerPrefs.SetInt(index.ToString(), 1);   // �ر��ߴٰ� ����
            for (int i = 0; i < notice.transform.childCount; i++)
            {
                bool isActive = i == (int)index;       // 
                notice.transform.GetChild(i).gameObject.SetActive(isActive);
            }
            StartCoroutine("Notice");
        }
    }
    IEnumerator Notice()            // ĳ���� �ر� �˸�
    {
        notice.SetActive(true);     // �˸�ON
        SoundManager.instance.PlayEffect(SoundManager.Effect.Notice);
        yield return wait;           // 5����
        notice.SetActive(false);    // �˸�OFF
    }
}
