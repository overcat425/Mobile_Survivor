using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateArea : MonoBehaviour
{
    Collider2D coll;
    private void Awake()
    {
        coll = GetComponent<Collider2D>();
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Area"))           // 자동생성맵 로직
        {
            Vector3 playerPos = GameManager.instance.player.transform.position;
            Vector3 myPos = transform.position;
            //Vector3 playerDir = GameManager.instance.player.inputVec;
            switch (transform.tag)
            {
                case "Ground":          // 맵 재배치
                    float diffX = playerPos.x - myPos.x;
                    float diffY = playerPos.y - myPos.y;
                    float dirX = diffX < 0 ? -1 : 1;
                    float dirY = diffY < 0 ? -1 : 1;
                    diffX = Mathf.Abs(diffX);
                    diffY = Mathf.Abs(diffY);
                    if (diffX > diffY)
                    {
                        transform.Translate(Vector3.right * dirX * 40); // 맵이 3X3에 20칸이므로 2칸씩 가야함 -> 20x2 = 40
                    }else if (diffX < diffY)
                    {
                        transform.Translate(Vector3.up * dirY * 40);
                    }
                    break;
                case "Enemy":           // 적이 멀리 떨어질 시 플레이어 이동방향으로 재배치
                    if (coll.enabled)
                    {
                        Vector3 posDiff = playerPos - myPos;
                        Vector3 rand = new Vector3(Random.Range(-5, 5), Random.Range(-5, 5), 0);
                        transform.Translate(posDiff * 2 +rand);
                    }
                    break;
            }
        }
    }
}
