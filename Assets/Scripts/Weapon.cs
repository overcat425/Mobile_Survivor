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
                break;
        }
        if (Input.GetButtonDown("Fire1"))
        {
            Debug.Log("�۵���");
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
                break;
        }
    }
    void WeaponCount()
    {
        for (int i = 0; i < count; i++)
        {
            Transform bullet;
            if (i < transform.childCount)       // �̹� �����ִ� ���� ������ ��Ȱ��
            {                                           // �װ� �ƴϸ� ���� ����
                bullet = transform.GetChild(i);
            }else
            {
                bullet = GameManager.instance.enemySpawnPool.Spawn(prefabId).transform;
                bullet.parent = transform;
            }


            bullet.localPosition = Vector3.zero; // bullet�� ��ġ�� �÷��̾�� �ʱ�ȭ
            bullet.localRotation = Quaternion.identity;
            bullet.GetComponent<Bullet>().Init(damage, -1); // ��������� ���Ƚ�� ����(-1)

            Vector3 rotate = Vector3.forward * 360 * i / count;
            bullet.Rotate(rotate);
            bullet.Translate(bullet.up * 1.5f, Space.World);

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
