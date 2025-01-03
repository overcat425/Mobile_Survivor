using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainTextEffect : MonoBehaviour
{                                                  // 메인화면 이펙트 메소드
    public RectTransform[] rect;
    public GameObject btns;
    private void Awake()
    {
        Time.timeScale = 1f;  // 게임 끝내고 메인화면 귀환 대비
    }
    void Start()
    {
        StartCoroutine("MainEffect1");
    }       // 타이틀 -> 시작버튼 -> 종료버튼 순으로 이벤트발생
    IEnumerator MainEffect1()
    {
        for (int i = 0; i < rect.Length; i++)
        {
            rect[i].DOAnchorPosX(0, 1.5f);
            rect[i].DOAnchorPosY(50, 1.5f).SetEase(Ease.OutBounce);
        }
        if (btns.activeSelf == true)            // 유저가 이벤트 다 나오기 전에 광클해서 넘어가는경우 방지
            Invoke("BounceSound", 0.6f);
        yield return new WaitForSeconds(1.5f);
        StartCoroutine("MainEffect2");
    }
    IEnumerator MainEffect2()
    {
        for (int i = 1; i < rect.Length; i++)
        {
            rect[i].DOAnchorPosX(0, 1f);
            rect[i].DOAnchorPosY(-10, 1f).SetEase(Ease.OutBounce);
        }
        if (btns.activeSelf == true)
            Invoke("SlotSound", 0.4f);
        yield return new WaitForSeconds(1f);
        StartCoroutine("MainEffect3");
    }
    IEnumerator MainEffect3()
    {
        rect[2].DOAnchorPosX(0, 1f);
        rect[2].DOAnchorPosY(-50, 1f).SetEase(Ease.OutBounce);
        if (btns.activeSelf == true)
            Invoke("SlotSound", 0.4f);
        yield return null;
    }
    void BounceSound()
    {
        SoundManager.instance.PlayEffect(SoundManager.Effect.Bounce);
    }
    void SlotSound()
    {
        SoundManager.instance.PlayEffect(SoundManager.Effect.Hit);
    }
}
