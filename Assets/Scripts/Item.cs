using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public ItemData data;
    public int level;
    public Weapon weapon;
    public Equipment equipment;

    Image icon;
    Text textLevel;
    Text textName;
    Text textInfo;
    void Awake()
    {
        icon = GetComponentsInChildren<Image>()[1];
        icon.sprite = data.itemIcon;

        Text[] texts = GetComponentsInChildren<Text>();
        textLevel =  texts[0];
        textName = texts[1];
        textInfo = texts[2];
        textName.text = data.itemName;
    }
    private void OnEnable()
    {
        textLevel.text = "Lv." + (level);
        switch (data.itemType)
        {
            case ItemData.ItemType.Melee:
            case ItemData.ItemType.Range:
                textInfo.text = string.Format(data.itemInfo, data.damageUp[level] * 100, data.countUp[level]);
                break;
            case ItemData.ItemType.Glove:
            case ItemData.ItemType.Shoe:
                textInfo.text = string.Format(data.itemInfo, data.damageUp[level] * 100);
                break;
            default:
                textInfo.text = string.Format(data.itemInfo);
                break;
        }
    }
    public void OnClick()
    {
        switch (data.itemType)
        {
            case ItemData.ItemType.Melee:
            case ItemData.ItemType.Range:
                if(level == 0)
                {
                    GameObject newWeapon = new GameObject();
                    weapon = newWeapon.AddComponent<Weapon>();
                    weapon.Init(data);
                }
                else
                {
                    float nextDamage = data.baseDamage;
                    int nextCount = 0;

                    nextDamage += data.baseDamage * data.damageUp[level];
                    nextCount += data.countUp[level];
                    weapon.LvUp(nextDamage, nextCount);         // 레벨업시 실질적으로 적용되는곳
                }
                level++;
                break;
            case ItemData.ItemType.Glove:
            case ItemData.ItemType.Shoe:
                if ( level == 0)
                {
                    GameObject newEquipment = new GameObject();
                    equipment = newEquipment.AddComponent<Equipment>();
                    equipment.Init(data);
                }
                else
                {
                    float nextRate = data.damageUp[level];
                    equipment.EqLvUp(nextRate);
                }
                level++;
                break;
            case ItemData.ItemType.Heal:
                GameManager.instance.health = GameManager.instance.maxHealth;
                break;
        }
        if (level == data.damageUp.Length)
        {
            GetComponent<Button>().interactable = false;
        }
    }
}
