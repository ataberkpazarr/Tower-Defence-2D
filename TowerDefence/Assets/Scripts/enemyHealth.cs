using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public class enemyHealth : MonoBehaviour
{
    public static Action<Enemy> onEnemyKilled; // will be fired when the corresponded enemy is dead 
    public static Action<Enemy> onEnemyHitted; //we want to share information about which enemy got damaged, and we can share information in event with <Enemy>


    [SerializeField] private GameObject healthBarPrefab;
    [SerializeField] private Transform barPosition;
    [SerializeField] private float initialHealth; // 10 default
    [SerializeField] private float maxHealth; //10 default

    private Image healthBar;
    private Enemy enemy;


    private float currentHealth;
    private void Start()
    {
        createHealthBar();
        currentHealth = initialHealth;
        enemy = GetComponent<Enemy>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            dealDamage(5f);
        }

        healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, currentHealth / maxHealth,Time.deltaTime*10f);
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
        //resetHealth();
        if (onEnemyKilled != null)
        {
            onEnemyKilled.Invoke(enemy);
        }
        //ObjectPooler.returnToPool(this.gameObject);

    }
}
