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
                    float speed = 150 * Character.AttackSpeed;
                    weapon.speed = speed + (speed * rate);
                    break;
                default:
                    speed = 0.3f * Character.AttackRate;
                    weapon.speed = speed * (1f - rate);
                    //Debug.Log("감소한 시간 : " + speed + " * (1 - " + rate + ") = " + weapon.speed); // Test sc.
                    break;
            }
        }
    }
    void SpeedUp()
    {
        float speed = 3 * Character.Speed;      // 캐릭터별 이동속도 부여
        GameManager.instance.player.speed = speed + speed * rate;
    }
}
