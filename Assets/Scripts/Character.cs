using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour          // ĳ���ͺ� �ٸ� ����Ƽ�� �ο�
{
    public static float Speed       // �ź��� �̵��ӵ� 10%
    {
        get { return GameManager.instance.playerId == 0 ? 1.1f : 1f; }
    }
    public static float IsGun   // ������ ���� ��� ����
    {
        get { return GameManager.instance.playerId == 1 ? 1f : 0f; }
    }
    public static float Damage          // ���δ� ���ݷ� 20%
    {
        get { return GameManager.instance.playerId == 2 ? 1.2f : 1f; }
    }
}
    //public static float AttackSpeed   // ������ ���� ���ݼӵ� 10%
    //{
    //    get { return GameManager.instance.playerId == 1 ? 1.1f : 1f; }
    //}