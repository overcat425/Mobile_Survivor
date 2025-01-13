using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour
{
    public ItemData.ItemType type;      // 아이템 타입
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
            case ItemData.ItemType.Glove:       // 장갑이면 공격속도증가
                RateUp();
                break;
            case ItemData.ItemType.Shoe:        // 신발이면 이동속도증가
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
        Weapon[] weapons = transform.parent.GetComponentsInChildren<Weapon>();  // 처음 실행될때 플레이어가 갖고있는 모든 무기에 속성을 부여함
        foreach (Weapon weapon in weapons) {
            switch (weapon.id)
            {
                case 0:             // 근접공격이면 회전속도 증가
                    float speed = 150;
                    weapon.speed = speed + (speed * rate);
                    break;
                default:            // 원거리 공격이면 발사간격 감소
                    speed = 0.4f;
                    weapon.speed = speed * (1f - rate);
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
