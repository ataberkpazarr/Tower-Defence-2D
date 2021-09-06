using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class currencySystem : Singleton<currencySystem>
{
    [SerializeField] private int coinTest;
    private string Currency_Save_Key = "MYGAME_CURRENCY";
    public int totalCoins { get; set; } //store the amount of coins

    private void Start()
    {
        PlayerPrefs.DeleteKey(Currency_Save_Key); // delete the previous game's coins and reset the balance
        loadCoins(); 
    }

    private void loadCoins()
    {
        totalCoins = PlayerPrefs.GetInt(Currency_Save_Key,coinTest);
    }
    public void AddCoins(int amount)
    {
        totalCoins += amount;
        PlayerPrefs.SetInt(Currency_Save_Key,totalCoins);
        PlayerPrefs.Save();

    }

    public void removeCoins(int amount)
    {
        if (totalCoins >= amount)
        {
            totalCoins -= amount;
            PlayerPrefs.SetInt(Currency_Save_Key, totalCoins);
            PlayerPrefs.Save();
        }
        
    }
    private void addCoinsAfterEnemyKill(Enemy en) //every time an enemy is being killed, update the total coins 
    {
        AddCoins(1);
    }
    private void OnEnable()
    {
        enemyHealth.onEnemyKilled += addCoinsAfterEnemyKill; // subscribe to the onEnemyKilledEvent

    }

    private void OnDisable()
    {
        enemyHealth.onEnemyKilled -= addCoinsAfterEnemyKill;

    }
}
