using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour
{
    public ItemData.ItemType type;
    public float rate;
    public void Init(ItemData data)
    {
        name = "Gear " + data.itemId;
        transform.parent = GameManager.instance.player.transform;
        transform.localPosition = Vector3.zero;

        type = data.itemType;
        rate = data.damageUp[0];
        UpGrade();
    }
    void UpGrade()
    {
        switch (type)
        {
            case ItemData.ItemType.Glove:
                RateUp();
                break;
            case ItemData.ItemType.Shoe:
                SpeedUp();
                break;
        }
    }
    public void EqLvUp(float rate)
    {
        this.rate = rate;
        UpGrade();
    }
    void RateUp()
    {
        Weapon[] weapons = transform.parent.GetComponentsInChildren<Weapon>();  // 처음 실행될때 플레이어가 갖고있는 모든 무기에 속성을 부여함 ; Line.17
        foreach (Weapon weapon in weapons) {
            switch (weapon.id)
            {
                case 0:
                    weapon.speed = 150 + (150 * rate);
                    break;
                default:
                    weapon.speed = 0.5f * (1f - rate);
                    break;
            }
        }
    }
    void SpeedUp()
    {
        float speed = 3f;
        GameManager.instance.player.speed = speed + speed * rate;
    }
}
