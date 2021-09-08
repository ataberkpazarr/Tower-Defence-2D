using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class levelManager : Singleton<levelManager>
{
    [SerializeField] private int Lives; // 10 right now 

    public static Action onGameOver;

    public int currentWave { get; set; }

    public int totalLives { get; set; }

    private void Start()
    {
        totalLives = Lives; // so in this way even if Lives is being changed by inspector or somehow after game started, totalLives will not be changed 
        currentWave = 1; 
    }

    private void waveCompleted()
    {
        currentWave+=1;
    }

    private void OnEnable() // fired when level manager gets enabled
    {
        Enemy.onEndReach += reduceLives;
        Spawner.onWaveCompleted += waveCompleted;
    }

    private void OnDisable() //vice versa
    {
        Enemy.onEndReach -= reduceLives;
        Spawner.onWaveCompleted -= waveCompleted;
        
    }

    public void gameOver()
    {
        if (onGameOver != null)
        {
            onGameOver.Invoke();
        }
        UiManager.Instance.showGameOverPanel();
        
    }

    private void reduceLives(Enemy en)
    {
        totalLives -= 1;
        if (totalLives <=0 )
        {
            totalLives = 0;
            gameOver();
        }
    }
}
