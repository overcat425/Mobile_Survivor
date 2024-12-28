using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public Vector2 inputVec;
    public float speed;
    public EnemyScanner scanner;
    Rigidbody2D rigid;
    SpriteRenderer sprite;
    Animator anim;

    public WeaponFlip[] hand;
    public RuntimeAnimatorController[] animCon;

    [Header("�ǰ� ����ȭ��")]
    public Image bloodScreen;
    public AnimationCurve curveBloodScreen;
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        scanner = GetComponent<EnemyScanner>();
        hand = GetComponentsInChildren<WeaponFlip>(true);
    }
    private void OnEnable()
    {
        speed *= Character.Speed;       // ĳ���ͺ� �̵��ӵ� �ο�
        anim.runtimeAnimatorController = animCon[GameManager.instance.playerId];   // ĳ���ͺ� �ִϸ�����
    }
    void OnMove(InputValue value)
    {
        inputVec = value.Get<Vector2>();        // �̵�(���̽�ƽ)
    }
    private void FixedUpdate()
    {
        if(GameManager.instance.isLive == true)         // �̵� ; ������ ����
        {
            Vector2 nextVec = inputVec.normalized * speed * Time.fixedDeltaTime;
            rigid.MovePosition(rigid.position + nextVec);
        }
    }
    private void LateUpdate()
    {
        if (GameManager.instance.isLive == true)
        {
            anim.SetFloat("Speed", inputVec.magnitude);
            if (inputVec.x != 0)
            {
                sprite.flipX = inputVec.x < 0;      // ĳ���� Flip
            }
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (GameManager.instance.isLive && GameManager.instance.isHitable == true)            // �ǰݽ� �����Ӽ��ؿ��� ü�°���
        {               // ���� ������ + ������ �ƴҶ��� �ǰݽ� ������
            StartCoroutine("BloodScreen");
            GameManager.instance.health -= Time.deltaTime * 10;
        }
        if (GameManager.instance.health < 0)            // ����� ���� ��Ȱ��ȭ �� ����ִϸ��̼�
        {
            for (int i = 2; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
            anim.SetTrigger("Dead");
            GameManager.instance.GameOver();
        }
    }
    IEnumerator BloodScreen()           // �ǰ� ȭ��
    {
        float percent = 0;      // 1�ʵ��� ȸ��
        while (percent < 1)
        {                      // ����ȭ�� ���İ��� 0���� 0.5(127)���� ��ȯ
            percent += Time.deltaTime;
            Color color = bloodScreen.color;
            color.a = Mathf.Lerp(0.15f, 0, curveBloodScreen.Evaluate(percent));
            bloodScreen.color = color;
            yield return null;
        }
    }
}
