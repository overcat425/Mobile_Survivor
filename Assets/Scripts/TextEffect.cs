using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextEffect : MonoBehaviour
{
    RectTransform rectTransform;
    int rand;
    private void Start()        // 게임오버, 게임클리어 텍스트 이펙트(랜덤)
    {
        rectTransform = GetComponent<RectTransform>();
        rectTransform.DOAnchorPosX(0, 1.5f);
        switch(rand = Random.Range(0, 3)){
            case 0:
                rectTransform.DOAnchorPosY(30, 1.5f).SetEase(Ease.OutBounce);
                break;
            case 1:
                rectTransform.DOAnchorPosY(30, 1.5f).SetEase(Ease.OutElastic);
                break;
            case 2:
                rectTransform.DOAnchorPosY(30, 1.5f).SetEase(Ease.OutFlash);
                break;
        }
    }
}
