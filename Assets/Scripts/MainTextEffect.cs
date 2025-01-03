using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainTextEffect : MonoBehaviour
{                                                  // ����ȭ�� ����Ʈ �޼ҵ�
    public RectTransform[] rect;
    public GameObject btns;
    private void Awake()
    {
        Time.timeScale = 1f;  // ���� ������ ����ȭ�� ��ȯ ���
    }
    void Start()
    {
        StartCoroutine("MainEffect1");
    }       // Ÿ��Ʋ -> ���۹�ư -> �����ư ������ �̺�Ʈ�߻�
    IEnumerator MainEffect1()
    {
        for (int i = 0; i < rect.Length; i++)
        {
            rect[i].DOAnchorPosX(0, 1.5f);
            rect[i].DOAnchorPosY(50, 1.5f).SetEase(Ease.OutBounce);
        }
        if (btns.activeSelf == true)            // ������ �̺�Ʈ �� ������ ���� ��Ŭ�ؼ� �Ѿ�°�� ����
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
