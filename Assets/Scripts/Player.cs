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

    [Header("피격 붉은화면")]
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
        GameManager.instance.vCam.Follow = this.transform;
        speed *= Character.Speed;       // 캐릭터별 이동속도 부여
        anim.runtimeAnimatorController = animCon[GameManager.instance.playerId];   // 캐릭터별 애니메이터
    }
    void OnMove(InputValue value)
    {
        inputVec = value.Get<Vector2>();        // 이동(조이스틱)
    }
    private void FixedUpdate()
    {
        if(GameManager.instance.isLive == true)         // 이동 ; 프레임 보정
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
                sprite.flipX = inputVec.x < 0;      // 캐릭터 Flip
            }
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {                                                               // 피격시  체력감소 메소드
        if (GameManager.instance.isLive && GameManager.instance.isHitable == true)
        {               // 게임 진행중 + 무적이 아닐때만 피격시 데미지
            StartCoroutine("BloodScreen");
            GameManager.instance.health -= Time.deltaTime * 10;
        }
        if (GameManager.instance.health < 0)    // 사망시 무기 비활성화 후 사망애니메이션
        {
            for (int i = 2; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
            anim.SetTrigger("Dead");
            GameManager.instance.GameOver();
        }
    }
    IEnumerator BloodScreen()           // 피격 화면
    {
        float percent = 0;
        while (percent < 0.7f)
        {                      // 빨간화면 알파값을 0에서 0.1(25)까지 변환
            percent += Time.deltaTime;
            Color color = bloodScreen.color;
            color.a = Mathf.Lerp(0f, 0.1f, curveBloodScreen.Evaluate(percent));
            bloodScreen.color = color;      //0부터 0.1까지 커브곡선 값을 받아 변경
            yield return null;
        }
    }
}
