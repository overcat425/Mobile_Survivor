using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int id;
    public int prefabId;
    public float damage;
    public int count;
    public float speed;
    float timer;
    Player player;
    private void Awake()
    {
        player = GetComponentInParent<Player>();
    }
    private void Start()
    {
        Init();
    }
    void Update()
    {
        switch (id)
        {
            case 0:
                transform.Rotate(Vector3.back * speed * Time.deltaTime);
                break;
            default:
                timer += Time.deltaTime;
                if (timer > speed)
                {
                    timer = 0f;
                    Fire();
                }
                break;
        }
        if (Input.GetButtonDown("Fire1"))
        {
            Debug.Log("작동함");
            LvUp(20f, 1);
        }
    }
    public void Init()
    {
        switch (id)
        {
            case 0:
                speed = 150;
                WeaponCount();
                break;
            default:
                speed = 0.3f;
                break;
        }
    }
    void WeaponCount()
    {
        for (int i = 0; i < count; i++)
        {
            Transform bullet;
            if (i < transform.childCount)       // 이미 나와있는 삽이 있으면 재활용
            {                                           // 그게 아니면 새로 생성
                bullet = transform.GetChild(i);
            }else
            {
                bullet = GameManager.instance.enemySpawnPool.Spawn(prefabId).transform;
                bullet.parent = transform;
            }


            bullet.localPosition = Vector3.zero; // bullet의 위치를 플레이어로 초기화
            bullet.localRotation = Quaternion.identity;
            bullet.GetComponent<Bullet>().Init(damage, -1, Vector3.zero); // 근접무기라 사용횟수 무한(-1)

            Vector3 rotate = Vector3.forward * 360 * i / count;
            bullet.Rotate(rotate);
            bullet.Translate(bullet.up * 1.5f, Space.World);

        }
    }
    void Fire()
    {
        if (player.scanner.nearest)
        {
            Vector3 targetPos = player.scanner.nearest.position;
            Vector3 dir = targetPos - transform.position; // 총알이 나갈 방향
            dir = dir.normalized;

            Transform bullet = GameManager.instance.enemySpawnPool.Spawn(prefabId).transform;
            bullet.position = transform.position;
            bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir); // 목표에 맞게 총알 회전
            bullet.GetComponent<Bullet>().Init(damage, count, dir);
        }
    }
    public void LvUp(float damage, int count)
    {
        this.damage = damage;
        this.count += count;
        if(id == 0)
        {
            WeaponCount();
        }
    }
}
