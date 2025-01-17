using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySpawner : MonoBehaviour
{
    public GameObject bossPrefab;
    public GameObject boss;
    public GameObject bossHp;

    public RectTransform[] bossTextrect;
    public Transform[] eliteTrans;             // 0�� ����Ʈ, 1�� ����
    public GameObject eliteHp;
    public GameObject eliteObject;       // ����Ʈ ��ȯ�ÿ� �±׸� �޾Ƴ��� �� ����������Ʈ���� ü�°��� ����

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
        Invoke("EliteSpawn", 299f);      // 5��°�� ����Ʈ����
        Invoke("BossSpawn", 422f);      // 7��°�� ��������
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
        boss = Instantiate(bossPrefab, spawnPoint[Random.Range(1, spawnPoint.Length)]);
        eliteTrans[1] = boss.transform;
        StartCoroutine(Elite(1));
    }
    void EliteSpawn()
    {
        GameObject enemy = GameManager.instance.enemySpawnPool.Spawn(0);
        enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;
        enemy.GetComponent<Enemy>().GetInfo(spawnData[5]);
        eliteTrans[0] = enemy.transform;
        eliteObject = enemy;
        enemy.AddComponent<HpUiScript>();
        eliteHp.SetActive(true);
        StartCoroutine(Elite(0));
    }
    IEnumerator Elite(int i)          // ����Ʈ �� ���� ���� ��
    {
        GameManager.instance.vignette.intensity.value = 1f;  // ���� ��Ŀ�� ����
        StartCoroutine(EliteText(i));     // �ؽ�Ʈ�׼�
        GameManager.instance.isLive = false;
        GameManager.instance.vCam.Follow = eliteTrans[i].transform; // ī�޶�->����
        SoundManager.instance.PlayEffect(SoundManager.Effect.Elite); // ���� ����Ʈ����
        yield return new WaitForSecondsRealtime(2.5f);              // 2.5�ʵ��� �����ֱ�
        GameManager.instance.vCam.Follow = target.transform;    // �ٽ� ī�޶�->�÷��̾�
        GameManager.instance.vignette.intensity.value = 0.44f;      // ���� ��Ŀ�� Ǯ��
        GameManager.instance.isLive = true;
    }
    IEnumerator EliteText(int i)          // ����Ʈ �� ���� ����� �ؽ�Ʈ�׼�
    {
        bossTextrect[i].gameObject.SetActive(true);      // �ؽ�Ʈ ON
        yield return new WaitForSeconds(0.5f);
        bossTextrect[i].DOAnchorPosX(0, 0.7f).SetEase(Ease.OutSine);
        yield return new WaitForSeconds(1.5f);      // DoTween���� �������� ȿ��
        bossTextrect[i].DOAnchorPosX(140, 0.6f).SetEase(Ease.InSine);
        yield return new WaitForSeconds(0.6f);
        bossTextrect[i].gameObject.SetActive(false); // �ؽ�Ʈ OFF
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