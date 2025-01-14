using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySpawner : MonoBehaviour
{
    public RectTransform bossTextrect;
    public Transform eliteTrans;             // �������� ī�޶� ��� ����
    public GameObject eliteHp;
    public GameObject eliteObject;       // ���� ��ȯ�ÿ� �±׸� �޾Ƴ��� �� ����������Ʈ���� ü�°��� ����

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
        Invoke("EliteSpawn", 299f);      // 5��°�� ��������
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
    void EliteSpawn()
    {
        GameObject enemy = GameManager.instance.enemySpawnPool.Spawn(0);
        enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;
        enemy.GetComponent<Enemy>().GetInfo(spawnData[5]);
        eliteTrans = enemy.transform;
        eliteObject = enemy;
        enemy.AddComponent<HpUiScript>();
        eliteHp.SetActive(true);
        StartCoroutine("Elite");
    }
    IEnumerator Elite()          // ���� ���� ��
    {
        GameManager.instance.vignette.intensity.value = 1f;  // ���� ��Ŀ�� ����
        StartCoroutine("EliteText");     // �ؽ�Ʈ�׼�
        GameManager.instance.isLive = false;
        GameManager.instance.vCam.Follow = eliteTrans.transform; // ī�޶�->����
        SoundManager.instance.PlayEffect(SoundManager.Effect.Elite); // �������� ����Ʈ����
        yield return new WaitForSecondsRealtime(2.5f);              // ���� 2.5�ʵ��� �����ֱ�
        GameManager.instance.vCam.Follow = target.transform;    // �ٽ� ī�޶�->�÷��̾�
        GameManager.instance.vignette.intensity.value = 0.44f;      // ���� ��Ŀ�� Ǯ��
        GameManager.instance.isLive = true;
    }
    IEnumerator EliteText()          // ���� ����� �ؽ�Ʈ�׼�
    {
        bossTextrect.gameObject.SetActive(true);      // �ؽ�Ʈ ON
        yield return new WaitForSeconds(0.5f);
        bossTextrect.DOAnchorPosX(0, 0.7f).SetEase(Ease.OutSine);
        yield return new WaitForSeconds(1.5f);      // DoTween���� �������� ȿ��
        bossTextrect.DOAnchorPosX(140, 0.6f).SetEase(Ease.InSine);
        yield return new WaitForSeconds(0.6f);
        bossTextrect.gameObject.SetActive(false); // �ؽ�Ʈ OFF
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