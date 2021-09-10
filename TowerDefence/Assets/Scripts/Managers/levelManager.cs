using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class levelManager : Singleton<levelManager>
{



    public int currentWave { get; set; }



    private void Start()
    {
   
        if (PlayerPrefs.HasKey("currentWave")) 
        {
            currentWave = PlayerPrefs.GetInt("currentWave");
            
        }
        else // if this is first-ever playing of user
        {
            currentWave = 1;

        }
    }

    private void waveCompleted() //after player completes the path, increase the cost for spawning a turret 
    {
        currentWave+=1;
        if (PlayerPrefs.HasKey("currentWave"))
        {
            PlayerPrefs.SetInt("currentWave",currentWave);
        }
        else
        {
            PlayerPrefs.SetInt("currentWave",currentWave);
        }

        //increase the cost

        int currentTurretCost = PlayerPrefs.GetInt("turretCost");
        PlayerPrefs.SetInt("turretCost",currentTurretCost+5);
        currencySystem.Instance.amountForSpawningTurret += 5;
            
    }

    private void OnEnable() 
    {
        Spawner.onWaveCompleted += waveCompleted; //fired when waveCompleted
    }

    private void OnDisable()
    {

        Spawner.onWaveCompleted -= waveCompleted;
        
    }


}
