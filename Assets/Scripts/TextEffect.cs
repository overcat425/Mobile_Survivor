using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextEffect : MonoBehaviour
{
    RectTransform rectTransform;
    private void Start()                // ���ӿ���, ����Ŭ���� �ؽ�Ʈ ����Ʈ
    {
        rectTransform = GetComponent<RectTransform>();
        rectTransform.DOAnchorPosX(0, 1.5f);
        rectTransform.DOAnchorPosY(30, 1.5f).SetEase(Ease.OutBounce);
    }
}
