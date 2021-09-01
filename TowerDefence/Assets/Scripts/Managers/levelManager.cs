using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class levelManager : MonoBehaviour
{
    [SerializeField] private int Lives; // 10 right now 

    public int totalLives { get; set; }

    private void Start()
    {
        totalLives = Lives; // so in this way even if Lives is being changed by inspector or somehow after game started, totalLives will not be changed 
    }

    private void OnEnable() // fired when level manager gets enabled
    {
        Enemy.onEndReach += reduceLives;
    }

    private void OnDisable() //vice versa
    {
        Enemy.onEndReach -= reduceLives;
    }

    private void reduceLives(Enemy en)
    {
        totalLives -= 1;
        if (totalLives <=0 )
        {
            totalLives = 0;
        }
    }
}
