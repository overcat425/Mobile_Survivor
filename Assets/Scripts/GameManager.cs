using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Player player;
    public static GameManager instance;
    public EnemySpawnPool enemySpawnPool;

    private void Awake()
    {
        instance = this;
    }
}
