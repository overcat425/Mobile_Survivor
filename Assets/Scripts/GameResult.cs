using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameResult : MonoBehaviour
{
    public GameObject[] title;
    public void Clear()
    {
        title[1].SetActive(true);
    }
    public void Defeat()
    {
        title[0].SetActive(true);
    }
}
