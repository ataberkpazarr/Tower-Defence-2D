﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum spawnModes 
{
    Fixed,
    Random
}
public class Spawner : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private spawnModes spawnMode;
    //[SerializeField] private GameObject testGameOb;
    [SerializeField] private int enemyCount=10; // amount of enemy we want to spawn with this spawner
    [SerializeField] private float delayBtwWaves;  //1f by default 


    [Header("Fixed Delay")]
    [SerializeField] private float delayBtwSpawns;

    [Header("Random Delay")]
    [SerializeField] private float minRandomDelay;
    [SerializeField] private float maxRandomDelay;

    private float spawnTimer;
    private int enemiesSpawned;

    private ObjectPooler pooler;
    private Waypoint wayp;
    private int remainingEnemies;

    private void Start()
    {
        pooler = GetComponent<ObjectPooler>();
        wayp = GetComponent<Waypoint>();
        remainingEnemies = enemyCount;

            
    }

    void Update()
    {
        spawnTimer -= Time.deltaTime; // eventually it will be less than zero 
        if (spawnTimer <0)
        {
            spawnTimer = getSpawnDelay();
            if (enemiesSpawned< enemyCount)
            {
                enemiesSpawned++; //update total enemies that spawned
                spawnEnemy();
            }
        }
    }

    private void enemyReachedEndorKilled(Enemy en)
    {
        remainingEnemies--;//whenever final position is reached by an enemy, decrease total remainings
        if (remainingEnemies <=0) // we need to start a new wave and since we have a delay between the waves, we need to use coroutine
        {
            StartCoroutine(nextWave()); 
        }

    }

    private IEnumerator nextWave()
    {
        yield return new WaitForSeconds(delayBtwWaves);
        remainingEnemies = enemyCount; // mechanics will start again 
        spawnTimer = 0f;
        enemiesSpawned = 0;
    }

    private void OnEnable()
    {
        Enemy.onEndReach += enemyReachedEndorKilled; // onEndReach event should be listened and if an enemy reaches that position, remaining enemies should be decreased
        enemyHealth.onEnemyKilled += enemyReachedEndorKilled;
    }

    private void OnDisable()
    {
        Enemy.onEndReach -= enemyReachedEndorKilled;
        enemyHealth.onEnemyKilled -= enemyReachedEndorKilled;
    }
    private float getSpawnDelay()
    {
        float delay = 0f;
        if (spawnMode==spawnModes.Fixed)
        {
            delay = delayBtwSpawns;
        }

        else if (spawnMode == spawnModes.Random)
        {
            delay = GetRandomDelay();
        }

        return delay; 
    }
    private void spawnEnemy()
    {
        GameObject newInstance = pooler.GetInstanceFromPool();
        //Instantiate(newInstance, transform.position,Quaternion.identity);
        Enemy enemy = newInstance.GetComponent<Enemy>();
        //Waypoint w = new Waypoint();
        enemy.setWaypoint(wayp);
        enemy.resetEnemy();
        enemy.transform.localPosition = transform.position; // make newly spawned enemy's position to main-spawning positioin  
        //newInstance.AddComponent<Enemy>();
        newInstance.SetActive(true);
    }
    private float GetRandomDelay()
   {

        float randomTimer = Random.Range(minRandomDelay,maxRandomDelay);
        return randomTimer;
   }

}