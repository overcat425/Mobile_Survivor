using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ExpParticle : MonoBehaviour
{                                                   // 경험치 구슬 UI전시 스크립트
    public Transform dest;              // 구슬이 추적할 목적지 (경험치칸)
    public Image image;
    private void Awake()
    {
        image = GetComponent<Image>();
        image.transform.SetSiblingIndex(0);  // 강화선택 UI보다 뒤쪽에 설정(맨뒤)
        dest = GameManager.instance.expEnd.transform;
                                                        // 경험치 바 끝쪽으로 목적지 설정
    }
    private void OnEnable()
    {
        StartCoroutine("SetFalse");
    }

    IEnumerator SetFalse()          // 풀링(재사용)을 위한 비활성화
    {
        yield return new WaitForSeconds(1.5f);
        gameObject.SetActive(false);
    }
}
