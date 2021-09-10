using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class turretProjectile : MonoBehaviour // defining the positions where we will spawns our projectiles
{

    [SerializeField] protected Transform projectileSpawnPosition;
    [SerializeField] protected float delayBetweenAttacks= 2f;



    public float Damage { get; set; }
    public float delayAfterShot { get; set; }


    protected float nextAttackTime;
    protected ObjectPooler pooler;
    private Projectile currentProjectileLoaded;
    protected Turret turret;

    private string keyForLevel; //key for reaching level from player prefs
    private float damageIncremental_; //how much damage-power will increase after level-up, this info is different depending on turret

    private void Start()
    {

        pooler = GetComponent<ObjectPooler>(); //attached objectpooler
        turret = GetComponent<Turret>();        // attached turret
        delayAfterShot = delayBetweenAttacks;   //delay after shot will be 
        keyForLevel = nodeManager.Instance.getOrderNumberOfNode(turret.this_node).ToString(); //key for level
        damageIncremental_ = turret.DamageIncremental;
        UpdateDamage();
    }

    protected virtual void Update()
    {
        
        UpdateDamage(); //update the damage always, in this way if turret levels up and it will be noticed and damage will be changed correspondly


        if (isTurretEmpty()) // if there is no loaded projectile which is ready to fire in the turret  
        {
            loadProjectile(); // then load it
        }
        if (Time.time > nextAttackTime) // if its time for attack
        {
            if (turret.currentEnemyTarget != null && currentProjectileLoaded != null && turret.currentEnemyTarget.EnemyHealth.getCurrentHealth() > 0) //if all set, target, missile projectile, target health
            {
                currentProjectileLoaded.transform.parent = null; //it set to null which make return true isTurretEmpty()
                currentProjectileLoaded.setEnemy(turret.currentEnemyTarget); 
            }
            nextAttackTime = Time.time + delayAfterShot;
        }

    }

    private void UpdateDamage()
    { 
        Damage = 10f + (damageIncremental_ * (float)PlayerPrefs.GetInt(keyForLevel)); 
    }
   

    private bool isTurretEmpty() // is there any missile projectile loaded to turret, which is ready to fire
    {
        if (currentProjectileLoaded == null)
            return true;
        else
            return false;
    }

    protected virtual void loadProjectile() // we have a full of projectiles to create by the objectpooler pattern
    {
        GameObject newProjectile = pooler.GetInstanceFromPool();
        newProjectile.transform.localPosition = projectileSpawnPosition.position;
        newProjectile.transform.SetParent(projectileSpawnPosition); // it needs parent the turret for the following the turret rotation, while moving
        currentProjectileLoaded = newProjectile.GetComponent<Projectile>(); //now we have a reference of the current projectile that we have loaded also it means that turret is not empty anymore
        currentProjectileLoaded.turretOwner = this; // there is an owner of projectile and it has get set properties in the projectile class so projectile object has an owner in type of turretProjectile 
        currentProjectileLoaded.resetProjectile();
        currentProjectileLoaded.damage = Damage; // this projectile's damage is being determined
        newProjectile.SetActive(true);
 
    }

    public void resetTurretProjectile() // called when projectile hits the enemy. Until enemy is being hitted, a new projectile is not loaded or fired for aesthetics and utilization
    {

        currentProjectileLoaded = null; 
    }
}
