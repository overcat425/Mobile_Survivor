using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ExpParticle : MonoBehaviour
{
    public Transform dest;
    public Image image;
    private void Awake()
    {
        image = GetComponent<Image>();
        image.transform.SetSiblingIndex(0);
        dest = GameManager.instance.expEnd.transform;
    }
    private void OnEnable()
    {
        StartCoroutine("SetFalse");
    }

    IEnumerator SetFalse()
    {
        yield return new WaitForSeconds(1.5f);
        gameObject.SetActive(false);
    }
}
