using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MagnetExp : MonoBehaviour
{
    Player player;
    public Rigidbody2D playerTransform;

    void Start()                // �÷��̾� �ݶ��̴� Ž��
    {
        playerTransform = GameManager.instance.player.GetComponent<Rigidbody2D>();
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Magnet"))     // ��� �ݶ��̴��� ������ �÷��̾�� �̵�
        {
            transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, 0.1f);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))         // �÷��̾�� ������ ȹ��
        {
            //player.GetExp();
            GameManager.instance.GetExp();
            SoundManager.instance.PlayEffect(SoundManager.Effect.Coin);
            Destroy();
        }
    }
    public void Destroy()               // ������Ʈ ������ ���� ��Ȱ��ȭ
    {
        gameObject.SetActive(false);
    }
}
