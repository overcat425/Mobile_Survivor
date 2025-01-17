using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEditor.Rendering;
using UnityEngine.SocialPlatforms;
using Unity.Mathematics;
using static Unity.Collections.AllocatorManager;
using System;
using UnityEngine.SocialPlatforms.Impl;

public class GoogleManager : MonoBehaviour
{
    bool isUnlockSergeant;
    bool isUnlockGeneral;
    public GameObject[] unlockedCharacter;
    void Start()
    {
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
        LogIn();
        Invoke("LoadLocks", 1f);
    }
    void Update()
    {
        CheckUnlock();
    }
    void CheckUnlock()
    {
        if (GameManager.instance.kills >= 1000&& GameManager.instance.kills < 1050) UnlockSergeant();
        if (GameManager.instance.gameTime >= GameManager.instance.maxGameTime) UnlockGeneral();
        if (isUnlockSergeant) unlockedCharacter[0].SetActive(true);
        if (isUnlockGeneral) unlockedCharacter[1].SetActive(true);
    }
    public void UnlockSergeant()
    {
        Social.ReportProgress(GPGSIds.achievement, 100, (bool success) =>{});
        LoadLocks();
    }
    public void UnlockGeneral()
    {
        Social.ReportProgress(GPGSIds.achievement_2, 100, (bool success) => { });
        LoadLocks();
    }
    public void LoadLocks()
    {
        PlayGamesPlatform.Instance.LoadAchievements((IAchievement[] achievements) =>
        {                                                   // 업적 목록 검색 후
            foreach (var achievement in achievements)
            {
                if (achievement.id == GPGSIds.achievement)  // 업적 완료면 캐릭터 오픈
                {
                    if (achievement.percentCompleted >= 100.0f)
                        isUnlockSergeant = true;
                }
                if (achievement.id == GPGSIds.achievement_2)
                {
                    if (achievement.percentCompleted >= 100.0f)
                        isUnlockGeneral = true;
                }
            }
        });
    }
    public void LogIn()             // 로그인
    {
        PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
    }
    internal void ProcessAuthentication(SignInStatus status)
    {
        if (status == SignInStatus.Success)
        {
            string name = PlayGamesPlatform.Instance.GetUserDisplayName();
            string id = PlayGamesPlatform.Instance.GetUserId();
            string ImgUrl = PlayGamesPlatform.Instance.GetUserImageUrl();
            //str.Append("로그인 성공 \n").Append(name);
            //text.text = str.ToString();
        }
        //else
        //{
        //    text.text = "로그인 실패!";
        //}
    }
}