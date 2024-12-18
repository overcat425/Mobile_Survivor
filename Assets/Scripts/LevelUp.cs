using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUp : MonoBehaviour
{
    RectTransform rect;
    Item[] items;
    void Awake()
    {
        rect = GetComponent<RectTransform>();
        items =GetComponentsInChildren<Item>(true);
    }
    void RandomItem()
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
    public void Show()
    {
        RandomItem();
        rect.localScale = Vector3.one;
        GameManager.instance.Stop();
    }
    public void Hide()
    {
        rect.localScale = Vector3.zero;
        GameManager.instance.Resume();
    }
    public void InitAttack(int i)
    {
        items[i].OnClick();
    }
}
