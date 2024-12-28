using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour                // 캔버스에 UI 전시 스크립트
{
    public enum UiType { Exp, Level, Kills, Time, Health }
    public UiType type;

    Text text;
    Slider slider;
    private void Awake()
    {
        text = GetComponent<Text>();
        slider = GetComponent<Slider>();
    }

    void LateUpdate()
    {
        switch (type)
        {
            case UiType.Exp:
                float nowExp = GameManager.instance.exp;
                float maxExp = GameManager.instance.nextExp[Mathf.Min(GameManager.instance.level, GameManager.instance.nextExp.Length-1)];
                slider.value = nowExp / maxExp;
                break;
            case UiType.Level:
                text.text = string.Format("Level : {0:F0}", GameManager.instance.level + 1);
                break;
            case UiType.Kills:
                text.text = string.Format("{0:F0}", GameManager.instance.kills);
                break;
            case UiType.Time:
                float timeRemain = GameManager.instance.maxGameTime - GameManager.instance.gameTime;
                int sec = Mathf.FloorToInt(timeRemain % 60);
                int min = Mathf.FloorToInt(timeRemain / 60);
                text.text = string.Format("{0:D2}:{1:D2}", min, sec);
                break;
            case UiType.Health:
                float nowHealth = GameManager.instance.health;
                float maxHealth = GameManager.instance.maxHealth;
                slider.value = nowHealth / maxHealth;
                break;
        }
    }
}
