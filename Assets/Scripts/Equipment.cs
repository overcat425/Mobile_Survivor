using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour
{
    public ItemData.ItemType type;      // ������ Ÿ��
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
            case ItemData.ItemType.Glove:       // �尩�̸� ���ݼӵ�����
                RateUp();
                break;
            case ItemData.ItemType.Shoe:        // �Ź��̸� �̵��ӵ�����
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
        Weapon[] weapons = transform.parent.GetComponentsInChildren<Weapon>();  // ó�� ����ɶ� �÷��̾ �����ִ� ��� ���⿡ �Ӽ��� �ο���
        foreach (Weapon weapon in weapons) {
            switch (weapon.id)
            {
                case 0:             // ���������̸� ȸ���ӵ� ����
                    float speed = 150;
                    weapon.speed = speed + (speed * rate);
                    break;
                default:            // ���Ÿ� �����̸� �߻簣�� ����
                    speed = 0.4f;
                    weapon.speed = speed * (1f - rate);
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
