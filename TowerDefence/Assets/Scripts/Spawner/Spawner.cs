using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public enum spawnModes //different spawn modes
{
    Fixed,  // spawn next enemy certain time after the previous one 
    Random //spawn enemy between random small intervals 
}
public class Spawner : MonoBehaviour
{
    
    public static Action onWaveCompleted;

    [Header("Settings")]
    [SerializeField] private spawnModes spawnMode;
    [SerializeField] private int enemyCount=10; // amount of enemy we want to spawn with this spawner
    [SerializeField] private float delayBtwWaves;  //1f by default 


    [Header("Fixed Delay")]
    [SerializeField] private float delayBtwSpawns;

    [Header("Random Delay")]
    [SerializeField] private float minRandomDelay;
    [SerializeField] private float maxRandomDelay;

    [Header("Poolers")]  // not a major difference, between 1-10, 21-30 , 41+ waves, red enemy will come between 11-20 and 31-40 green enemy will come
    [SerializeField] private ObjectPooler enemyWave10Pooler;
    [SerializeField] private ObjectPooler enemyWave11to20Pooler;
    [SerializeField] private ObjectPooler enemyWave21to30Pooler;
    [SerializeField] private ObjectPooler enemyWave31to40Pooler;
    [SerializeField] private ObjectPooler enemyWave41plus;

    private float spawnTimer; //will be used as counter for timing 
    private int enemiesSpawned; // total amount of enemies spawned at this wave, 
  
    bool cantNotCompleteWave;
  
    private Waypoint wayp;
    private int remainingEnemies;
    private bool timeToNextWave = false;
    List<Enemy> spawnedEnemies;
    

    private void Start()
    {
        
        wayp = GetComponent<Waypoint>(); // attached waypoint
        remainingEnemies = enemyCount; //total enemy count that will be spawned for a wave
        cantNotCompleteWave = false; //when wave (level) is not completed succesfully
        spawnedEnemies = new List<Enemy>(); //

            
    }


    void Update()
    {
        
        if (!timeToNextWave) // if we havent accomplish this wave succesfully yet
        {
        
            spawnTimer -= Time.deltaTime; // eventually it will be less than zero 

        if (spawnTimer < 0) // if its time, meaning, the corresponded delay is passed
        {
            spawnTimer = getSpawnDelay();
            if (enemiesSpawned < enemyCount && !cantNotCompleteWave)
            {
                enemiesSpawned++; //update total enemies that spawned
                spawnEnemy(); //spawn it

            }

            else if (cantNotCompleteWave) //then spawn enemyCount amount of enemies
            {

                enemiesSpawned++; //update total enemies that spawned
                spawnEnemy();

                if (enemiesSpawned == enemyCount)
                {
                    cantNotCompleteWave = false;
                    remainingEnemies = enemyCount; // mechanics will start again 

                }
            }
        }


        }
            
        else // if this wave is done
        {
            handleTheKill();

        }

        if (remainingEnemies > 0 && !UiManager.Instance.isThereAnyActiveEnemy()) //defensive programming
        {
            timeToNextWave = true;
        }
    }

    private ObjectPooler getPooler() // which pool we are? 
    {
        //not a major difference, between 1-10, 21-30 , 41+ waves, red enemy will come between 11-20 and 31-40 green enemy will come

        int currentWave = levelManager.Instance.currentWave;
        if (currentWave <=10)
        {
            return enemyWave10Pooler;
        }
       else  if (currentWave > 10 && currentWave <= 20)
        {
            return enemyWave11to20Pooler;
        }
       else  if (currentWave > 20 && currentWave <= 30)
        {
            return enemyWave21to30Pooler;
        }
       else  if (currentWave > 30 && currentWave <= 40)
        {
            return enemyWave31to40Pooler;
        }
        else if (currentWave > 40 )
        {
            return enemyWave41plus;
        }
        return null;
    }


    private void handleTheKill()
    {
       
        remainingEnemies--;
        if (remainingEnemies <= 0)
        {
            timeToNextWave = true; // ready to next wave
        }
        if (!UiManager.Instance.isThereAnyActiveEnemy()) // if there are not any active enemy in scene
        {

            enemiesSpawned = 0;
            timeToNextWave = false;

            if (onWaveCompleted != null)
            {

                PlayerPrefs.SetFloat("health", PlayerPrefs.GetFloat("health") + 10); //enemy's health will be increased by 10 for makin harder
                onWaveCompleted.Invoke();

            }
            currencySystem.Instance.addCoinsAmount += 3; // coins amount for killing an enemy
            PlayerPrefs.SetInt("killEarning", currencySystem.Instance.addCoinsAmount);
            StartCoroutine(nextWave()); // we need to start a new wave and since we have a delay between the waves, we need to use coroutine
        }

    }


    private void onEnemyKilledHandle(Enemy en)
    {
        handleTheKill();
       
    }

    private IEnumerator nextWave() // time for next wave, some resets
    {
        yield return new WaitForSeconds(delayBtwWaves);

        // mechanics will start again 
        remainingEnemies = enemyCount; 
        spawnTimer = 0f;
        enemiesSpawned = 0;
    }

    private void OnEnable()
    {

        enemyHealth.onEnemyKilled += onEnemyKilledHandle; // if an enemy killed
        Enemy.onEndReach += onEndReachHandler; //if an enemy reaches to end

    }

    private void OnDisable()
    {

        enemyHealth.onEnemyKilled -= onEnemyKilledHandle;
        Enemy.onEndReach += onEndReachHandler;

    }
    private void onEndReachHandler(Enemy en) // when an enemy reached end of the path
    {
        
        enemiesSpawned = 0;
        UiManager.Instance.destroyActiveEnemies();
        cantNotCompleteWave = true;

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

        
            // get the already instantiated instance from pool and make it active with corresponding resets
            
            GameObject newInstance = getPooler().GetInstanceFromPool();                                                             
            Enemy enemy = newInstance.GetComponent<Enemy>();

            enemy.setWaypoint(wayp);
            enemy.resetEnemy(); // reset health etc..
            enemy.transform.localPosition = transform.position; // make newly spawned enemy's position to main-spawning positioin                                                     

            UiManager.Instance.addActiveEnemyList(enemy); 
            newInstance.SetActive(true);
        
    }
    
    private float GetRandomDelay()
   {

        float randomTimer = UnityEngine.Random.Range(minRandomDelay,maxRandomDelay);
        return randomTimer;
   }

}
