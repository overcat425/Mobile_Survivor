using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthHUD : MonoBehaviour
{
    RectTransform rect;
    void Awake()
    {
        rect = GetComponent<RectTransform>();
    }
    void FixedUpdate()
    {                                       // ü�¹� UI�� �÷��̾ �����ϰ� ���󰡵��� ��
        rect.position = Camera.main.WorldToScreenPoint(GameManager.instance.player.transform.position);
    }
}
