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
    void RandomItem()       // �������� ��ȭ ������ ī�װ��� 3�� �������� ����
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
    public void Show()          // ������ UI ����
    {
        GameManager.instance.isHitable = false;     // �÷��̾� �ǰ� off
        rect.localScale = Vector3.one;
        RandomItem();                       // ���������� �̾��ִ� �޼ҵ�
        StartCoroutine("Anim");
        SoundManager.instance.PlayEffect(SoundManager.Effect.LvUp); // ������ ȿ����
        SoundManager.instance.StopBgm(true);    //false�� Resume�� ����
    }
    public void Hide()          // ������ UI ����
    {
        GameManager.instance.isHitable = true;      // �÷��̾� �ǰ� on
        rect.localScale = Vector3.zero;
        selectPane.localScale = Vector3.zero;
        GameManager.instance.Resume();              
        SoundManager.instance.PlayEffect(SoundManager.Effect.Select);   // ȿ����
    }
    IEnumerator Anim()              // ������ ��ȭâ �˾��̺�Ʈ
    {
        selectPane.DOScale(1f,0.6f).SetEase(Ease.OutBounce);    // DoTween���� ����â ��ġ��
        yield return new WaitForSeconds(0.6f);                  // DoTween ���
        GameManager.instance.Stop();                            // �ð� ����
    }
    public void InitAttack(int i)
    {
        items[i].OnClick();
    }
}
