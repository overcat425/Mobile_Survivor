using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour                // 캔버스에 UI 전시 스크립트
{
    public enum UiType { Exp, Level, Kills, Time, Health, Elite, Boss }       // 열거형 타입
    public UiType type;
    Transform eliteHp;              // 체력바가 따라갈 좌표들
    Text text;
    Slider slider;
    private void Awake()
    {
        text = GetComponent<Text>();
        slider = GetComponent<Slider>();
        switch (type)
        {
            case UiType.Elite:
                eliteHp = GameManager.instance.enemySpawner.eliteTrans[0];
                break;
        }
    }
    void LateUpdate()
    {
        switch (type)           // UI 타입에 따른 스크립트 역할화
        {
            case UiType.Exp:
                float nowExp = GameManager.instance.exp;
                float maxExp = GameManager.instance.nextExp[Mathf.Min(GameManager.instance.level, GameManager.instance.nextExp.Length-1)];
                slider.DOValue(nowExp/maxExp, 1f);
                break;
            case UiType.Level:
                text.text = string.Format("Lv : {0:F0}", GameManager.instance.level + 1);
                break;
            case UiType.Kills:
                text.text = string.Format("{0:F0}", GameManager.instance.kills);
                break;
            case UiType.Time:
                float timeRemain = GameManager.instance.maxGameTime - GameManager.instance.gameTime;
                int sec = Mathf.FloorToInt(timeRemain % 60);
                int min = Mathf.FloorToInt(timeRemain / 60);
                text.text = string.Format("{0:D2}:{1:D2}", min, sec);
                if(min<=0)text.color = Color.red;
                break;
            case UiType.Health:
                float nowHealth = GameManager.instance.health;
                float maxHealth = GameManager.instance.maxHealth;
                slider.value = nowHealth / maxHealth;
                break;
            case UiType.Elite:
                HpUiScript eliteScript = GameManager.instance.enemySpawner.eliteObject.GetComponent<HpUiScript>();
                float eliteHealth = eliteScript.eliteHealth;
                float maxEliteHealth = eliteScript.eliteMaxHealth;
                transform.position = Camera.main.WorldToScreenPoint(eliteHp.position + new Vector3(0, 1.3f, 0));
                slider.value = eliteHealth / maxEliteHealth;
                if (eliteHealth < 0) gameObject.SetActive(false);
                break;
            case UiType.Boss:
                BossScript bossScript = GameManager.instance.enemySpawner.boss.GetComponent<BossScript>();
                float bossHealth = bossScript.health;
                float maxBossHealth = bossScript.maxHealth;
                transform.position = Camera.main.WorldToScreenPoint(bossScript.gameObject.transform.position + new Vector3(0, 1.5f, 0));
                slider.value = bossHealth / maxBossHealth;
                if (bossHealth < 0) gameObject.SetActive(false);
                break;
        }
    }
}