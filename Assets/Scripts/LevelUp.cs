using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LevelUp : MonoBehaviour
{
    RectTransform rect;
    public RectTransform selectPane;
    Item[] items;
    void Awake()
    {
        rect = GetComponent<RectTransform>();
        items =GetComponentsInChildren<Item>(true);
    }
    void RandomItem()       // 레벨업시 강화 가능한 카테고리중 3개 랜덤으로 뽑음
    {
        foreach (Item item in items){
            item.gameObject.SetActive(false);
        }
        int[] rand = new int[3];
        while (true)
        {
            for (int i = 0; i < 3; i++) {
                rand[i] = Random.Range(0, items.Length);
            }
            if (rand[0] != rand[1] && rand[1] != rand[2] && rand[0] != rand[2])
            {
                break;
            }
        }
        for (int i = 0; i < rand.Length; i++)
        {
            Item randItem = items[rand[i]];
            if (randItem.level == randItem.data.damageUp.Length){
                items[4].gameObject.SetActive(true);
            }
            else{
                randItem.gameObject.SetActive(true);
            }
        }
    }
    public void Show()          // 레벨업 UI 전시
    {
        GameManager.instance.isHitable = false;
        rect.localScale = Vector3.one;
        RandomItem();
        StartCoroutine("Anim");
        SoundManager.instance.PlayEffect(SoundManager.Effect.LvUp);
        SoundManager.instance.StopBgm(true);    //false는 Resume에 있음
    }
    public void Hide()          // 레벨업 UI 숨김
    {
        GameManager.instance.isHitable = true;
        rect.localScale = Vector3.zero;
        selectPane.localScale = Vector3.zero;
        GameManager.instance.Resume();
        SoundManager.instance.PlayEffect(SoundManager.Effect.Select);
    }
    public void InitAttack(int i)
    {
        items[i].OnClick();
    }
    IEnumerator Anim()              // 레벨업 강화창 팝업이벤트
    {
        selectPane.DOScale(1f,0.6f).SetEase(Ease.OutBounce);
        yield return new WaitForSeconds(0.6f);
        GameManager.instance.Stop();
    }
}
