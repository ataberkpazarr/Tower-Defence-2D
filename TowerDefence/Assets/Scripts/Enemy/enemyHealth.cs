using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public class enemyHealth : MonoBehaviour
{
    public static Action<Enemy> onEnemyKilled; // will be fired when the corresponded enemy is dead 
    public static Action<Enemy> onEnemyHitted; //we want to share information about which enemy got damaged, and we can share information in event with <Enemy>


    [SerializeField] private GameObject healthBarPrefab; // health Bar in the game is two nested bars actually. and this is the fillable, changeable, white one the other one is stable
    [SerializeField] private Transform barPosition; //where to spawn the bar
     private float initialHealth =200; // 200 by default but being increased while levels passed


    private Image healthBar;
    private Enemy enemy;


    private float currentHealth; 
    private void Start()
    {
        if (PlayerPrefs.HasKey("health")) // if this is not first-ever game, inital health of an enemy is getting increase by levels passed
        {
            initialHealth = PlayerPrefs.GetFloat("health");
        }
        else
        {
            PlayerPrefs.SetFloat("health",initialHealth);
        }
        
        createHealthBar();
        currentHealth = initialHealth;
        enemy = GetComponent<Enemy>();
    }



    private void Update()
    {

        healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, currentHealth / initialHealth, Time.deltaTime * 10f);
    }

    public float getCurrentHealth()
    {
        return currentHealth;
    }
    private void createHealthBar()
    {
        GameObject newBar = Instantiate(healthBarPrefab, barPosition.position, Quaternion.identity);
        newBar.transform.SetParent(transform);

        enemyHealthContainer container = newBar.GetComponent<enemyHealthContainer>();
        healthBar = container.getFillAmountImage();
    }

    public void dealDamage(float receivedDamaged)
    {

        currentHealth -= receivedDamaged;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            die();
        }

        else   // if enemy got hit and have health
        {
            if (onEnemyHitted != null)
            {
                onEnemyHitted.Invoke(enemy); //fire the event
            }
        }
    }

    public void resetHealth() // can be accessed outer classes
    {
        currentHealth = initialHealth;
        healthBar.fillAmount = 1f;
    }
    private void die()
    {
        
        if (onEnemyKilled != null)
        {
            onEnemyKilled.Invoke(enemy); // animation, coin etc all will handled
            UiManager.Instance.totalKilled += 1; //increase totalKilled text

        }
        

    }
}
