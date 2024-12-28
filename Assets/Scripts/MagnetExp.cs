using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MagnetExp : MonoBehaviour
{
    Player player;
    public Rigidbody2D playerTransform;

    void Start()                // 플레이어 콜라이더 탐색
    {
        playerTransform = GameManager.instance.player.GetComponent<Rigidbody2D>();
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Magnet"))     // 흡수 콜라이더에 닿으면 플레이어에게 이동
        {
            transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, 0.1f);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))         // 플레이어에게 닿으면 획득
        {
            //player.GetExp();
            GameManager.instance.GetExp();
            SoundManager.instance.PlayEffect(SoundManager.Effect.Coin);
            Destroy();
        }
    }
    public void Destroy()               // 오브젝트 재사용을 위한 비활성화
    {
        gameObject.SetActive(false);
    }
}
