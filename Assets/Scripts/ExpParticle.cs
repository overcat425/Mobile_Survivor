using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ExpParticle : MonoBehaviour
{                                                   // 경험치 구슬 UI전시 스크립트
    public Transform dest;
    public Image image;
    private void Awake()
    {
        image = GetComponent<Image>();
        image.transform.SetSiblingIndex(0);      // 강화선택 UI보다 뒤쪽에 설정(맨뒤)
        dest = GameManager.instance.expEnd.transform;   // 경험치 바 끝으로 목적지 설정
    }
    private void OnEnable()
    {
        StartCoroutine("SetFalse");
    }

    IEnumerator SetFalse()          // 풀링을 위한 비활성화
    {
        yield return new WaitForSeconds(1.5f);
        gameObject.SetActive(false);
    }
}
