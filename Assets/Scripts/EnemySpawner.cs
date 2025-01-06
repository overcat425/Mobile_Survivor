using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public RectTransform bossTextrect;
    Transform bossTrans;

    public SpawnData[] spawnData;
    public Transform[] spawnPoint;
    float timer;
    int level;
    public Rigidbody2D target;


    void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>();
        target = GameManager.instance.player.GetComponent<Rigidbody2D>();
    }
    private void OnEnable()
    {
        Invoke("BossSpawn", 419f);      // 7��°�� ��������
    }
    void Update()
    {
        if (GameManager.instance.isLive == true)
        {                                       // level : �� ���̵� ������ ���̺극��
            timer += Time.deltaTime;
            level = Mathf.FloorToInt(5 - ((GameManager.instance.maxGameTime - GameManager.instance.gameTime) / 120));
            if(timer > spawnData[level].spawnTime)
            {                           // ���� �������̸� �� ������Ʈ ��ȯ
                Spawn();
                timer = 0f;
            }
        }
    }
    void Spawn()
    {
        switch (level)  // ���̺극���� 1�̸� �⺻ ���͸� ��ȯ�ϰ�, 2���ʹ� ���Ͱ� �� ���� ���(DoubleSpawn())
        {
            case 0:
                GameObject enemy = GameManager.instance.enemySpawnPool.Spawn(0);
                enemy.transform.position = spawnPoint[Random.Range(1,spawnPoint.Length)].position;
                enemy.GetComponent<Enemy>().GetInfo(spawnData[level]);
                break;
            default:
                DoubleSpawn();
                break;
        }
    }
    void DoubleSpawn()
    {
        for (int i = 0; i < 2; i++)     // 2���� ���� + 2��� ���
        {
            GameObject enemy = GameManager.instance.enemySpawnPool.Spawn(0);
            enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;
            enemy.GetComponent<Enemy>().GetInfo(spawnData[level - i]);
        }
    }
    void BossSpawn()
    {
        GameObject enemy = GameManager.instance.enemySpawnPool.Spawn(0);
        enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;
        enemy.GetComponent<Enemy>().GetInfo(spawnData[5]);
        bossTrans = enemy.transform;
        StartCoroutine("Boss");
    }
    IEnumerator Boss()          // ���� ���� ��
    {
        GameManager.instance.vignette.intensity.value = 1f;  // ���� ��Ŀ�� ����
        StartCoroutine("BossText");     // �ؽ�Ʈ�׼�
        GameManager.instance.isLive = false;
        GameManager.instance.vCam.Follow = bossTrans.transform; // ī�޶�->����
        SoundManager.instance.PlayEffect(SoundManager.Effect.Boss); // �������� ����Ʈ����
        yield return new WaitForSecondsRealtime(2.5f);              // ���� 2.5�ʵ��� �����ֱ�
        GameManager.instance.vCam.Follow = target.transform;    // �ٽ� ī�޶�->�÷��̾�
        GameManager.instance.vignette.intensity.value = 0.44f;      // ���� ��Ŀ�� Ǯ��
        GameManager.instance.isLive = true;
    }
    IEnumerator BossText()          // ���� ����� �ؽ�Ʈ�׼�
    {
        bossTextrect.localScale = Vector3.one;
        yield return new WaitForSeconds(0.5f);
        bossTextrect.DOAnchorPosX(0, 0.7f).SetEase(Ease.OutSine);
        yield return new WaitForSeconds(1.5f);      // DoTween���� ȿ�� �߰�
        bossTextrect.DOAnchorPosX(140, 0.6f).SetEase(Ease.InSine);
        yield return new WaitForSeconds(0.6f);
        bossTextrect.localScale = Vector3.zero;
    }
}
[System.Serializable]
public class SpawnData              // �� ������Ʈ �Ӽ�Ÿ�� ����ȭŬ����
{
    public int Type;
    public float spawnTime;
    public int health;
    public float speed;
}