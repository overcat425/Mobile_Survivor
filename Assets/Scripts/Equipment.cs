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
        Weapon[] weapons = transform.parent.GetComponentsInChildren<Weapon>();  // ó�� ����ɶ� �÷��̾ �����ִ� ��� ���⿡ �Ӽ��� �ο��� ; Line.17
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
                    //Debug.Log("������ �ð� : " + speed + " * (1 - " + rate + ") = " + weapon.speed); // Test sc.
                    break;
            }
        }
    }
    void SpeedUp()
    {
        float speed = 3 * Character.Speed;      // ĳ���ͺ� �̵��ӵ� �ο�
        GameManager.instance.player.speed = speed + speed * rate;
    }
}
