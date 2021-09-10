using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class currencySystem : Singleton<currencySystem>
{
    [SerializeField] private int coinAmountTest; // serialize field for trial issues
    private string Currency_Save_Key = "MYGAME_CURRENCY";
    public int totalCoins { get; set; } //store the amount of coins
    public int addCoinsAmount { get; set; }
    public int amountForSpawningTurret { get; set; }
    private void Start()
    {
        
        if (PlayerPrefs.HasKey("turretCost"))  //if this is not first evet game, then we have already had a turretCost amount, whic is being increased wave by wave(level)
        {
            amountForSpawningTurret = PlayerPrefs.GetInt("turretCost");
        }

        else
        {
            PlayerPrefs.SetInt("turretCost",40);
        }


        if (PlayerPrefs.HasKey("killEarning")) // if this is not the first-ever game, then we have already set kill Earning amount so make it equal to addCoinsAmount 
        {
            addCoinsAmount = PlayerPrefs.GetInt("killEarning"); // how much we will earn per kill   
        }

        else
        {
            addCoinsAmount = 25;
            PlayerPrefs.SetInt("killEarning", addCoinsAmount); //if not set then make it 25
        }
        
        loadCoins(); 
    }

    private void loadCoins() //Load coins at the initial
    {
        PlayerPrefs.SetInt(Currency_Save_Key, coinAmountTest); // make total balance, coinAmountTest which is avaliable to being modify by serialize field
        totalCoins = PlayerPrefs.GetInt(Currency_Save_Key);
    }

    public void AddCoins(int amount) // add coins to our balance when an enemy is killed
    {
        totalCoins += amount;
        PlayerPrefs.SetInt(Currency_Save_Key,totalCoins);
        PlayerPrefs.Save();
    }


    public void removeCoins() // remove coins when a new turret is spawned
    {
        if (totalCoins >= amountForSpawningTurret)
        {
            totalCoins -= amountForSpawningTurret;
            PlayerPrefs.SetInt(Currency_Save_Key, totalCoins);
            PlayerPrefs.Save();
        }
        
    }

    private void addCoinsAfterEnemyKill(Enemy en) //every time an enemy is being killed, update the total coins 
    {
        AddCoins(addCoinsAmount);
    }

    private void OnEnable()
    {
        enemyHealth.onEnemyKilled += addCoinsAfterEnemyKill; // will be fired when an enemy is being killed

    }

    private void OnDisable()
    {
        enemyHealth.onEnemyKilled -= addCoinsAfterEnemyKill;

    }
}
