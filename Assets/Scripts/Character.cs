using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour          // 캐릭터별 다른 어드밴티지 부여
{
    public static float Speed       // 신병은 이동속도 10%
    {
        get { return GameManager.instance.playerId == 0 ? 1.1f : 1f; }
    }
    public static float AttackSpeed   // 병장은 근접 공격속도 10%
    {
        get { return GameManager.instance.playerId == 1 ? 1.1f : 1f; }
    }
    public static float AttackRate   // 병장은 원거리 연사속도 10%
    {
        get { return GameManager.instance.playerId == 1 ? 0.9f : 1f; }
    }
    public static float Damage          // 간부는 공격력 20%
    {
        get { return GameManager.instance.playerId == 2 ? 1.2f : 1f; }
    }
}
