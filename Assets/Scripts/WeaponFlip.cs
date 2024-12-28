using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponFlip : MonoBehaviour             // 무기 Flip 스크립트
{
    public bool isLeft;
    public SpriteRenderer sprite;

    SpriteRenderer player;
    //Vector3 rightPos = new Vector3(0.15f, -0.1f, 0);      // 오른손 무기 위치조절
    //Vector3 rightPosRev = new Vector3(0.15f, -0.1f, 0);

    Quaternion leftRot = Quaternion.Euler(0, 0, -30);
    Quaternion leftRotRev = Quaternion.Euler(0, 180, -30);
    private void Awake()
    {
        player = GetComponentsInParent<SpriteRenderer>()[1];
    }

    void LateUpdate()
    {
        bool isRev = player.flipX;
        if (isLeft) // Melee
        {
            transform.localRotation = isRev ? leftRotRev : leftRot;
            sprite.flipY = isRev;
            sprite.sortingOrder = isRev ? 4 : 6;        // 레이어 조정
        }
        else // Range
        {
            sprite.flipX = isRev;
            sprite.sortingOrder = isRev ? 6 : 4;        // 레이어 조정
        }
    }
}
