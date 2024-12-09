using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateArea : MonoBehaviour
{
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Area"))           // �ڵ������� ����
        {
            Vector3 playerPos = GameManager.instance.player.transform.position;
            Vector3 myPos = transform.position;
            float diffX = Mathf.Abs(playerPos.x - myPos.x);
            float diffY = Mathf.Abs(playerPos.y - myPos.y);

            Vector3 playerDir = GameManager.instance.player.inputVec;
            float dirX = playerDir.x < 0 ? -1 : 1;
            float dirY = playerDir.y < 0 ? -1 : 1;
            switch (transform.tag)
            {
                case "Ground":
                    if (diffX > diffY)
                    {
                        transform.Translate(Vector3.right * dirX * 40); // ���� 3X3�� 20ĭ�̹Ƿ� 2ĭ�� ������ -> 20x2 = 40
                    }else if (diffX < diffY)
                    {
                        transform.Translate(Vector3.up * dirY * 40);
                    }
                    break;
                case "Enemy":
                    break;
            }
        }
    }
    void Start()
    {

    }

    void Update()
    {
        
    }
}
