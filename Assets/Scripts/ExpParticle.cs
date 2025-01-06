using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ExpParticle : MonoBehaviour
{                                                   // ����ġ ���� UI���� ��ũ��Ʈ
    public Transform dest;              // ������ ������ ������ (����ġĭ)
    public Image image;
    private void Awake()
    {
        image = GetComponent<Image>();
        image.transform.SetSiblingIndex(0);  // ��ȭ���� UI���� ���ʿ� ����(�ǵ�)
        dest = GameManager.instance.expEnd.transform;
                                                        // ����ġ �� �������� ������ ����
    }
    private void OnEnable()
    {
        StartCoroutine("SetFalse");
    }

    IEnumerator SetFalse()          // Ǯ��(����)�� ���� ��Ȱ��ȭ
    {
        yield return new WaitForSeconds(1.5f);
        gameObject.SetActive(false);
    }
}
