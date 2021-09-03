using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turretProjectile : MonoBehaviour // defining the positions where we will spawns our projectiles
{

    [SerializeField] protected Transform projectileSpawnPosition;
    [SerializeField] protected float delayBetweenAttacks= 2f;


    protected float nextAttackTime;
    protected ObjectPooler pooler;
    private Projectile currentProjectileLoaded;
    protected Turret turret;

    private void Start()
    {

        pooler = GetComponent<ObjectPooler>();
        turret = GetComponent<Turret>();
        loadProjectile();
    }

    protected virtual void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.K))
        {

            loadProjectile();  
        }*/

        if (isTurretEmpty())
        {

            loadProjectile();
        }

        if (Time.time > nextAttackTime)
        {
            if (turret.currentEnemyTarget != null && currentProjectileLoaded != null && turret.currentEnemyTarget.EnemyHealth.getCurrentHealth() > 0)
            {
                currentProjectileLoaded.transform.parent = null;
                currentProjectileLoaded.setEnemy(turret.currentEnemyTarget);
            }

            nextAttackTime = Time.time + delayBetweenAttacks;
        }

        
    }

    private bool isTurretEmpty()
    {
        if (currentProjectileLoaded == null)
            return true;
        else
            return false;
    }

    protected virtual void loadProjectile() // assume that we have a full of projectiles to create the logic of loading a new projectile
    {
        GameObject newProjectile = pooler.GetInstanceFromPool();
        newProjectile.transform.localPosition = projectileSpawnPosition.position;
        newProjectile.transform.SetParent(projectileSpawnPosition); // it needs parent the turret for the following the turret rotation, while moving
        currentProjectileLoaded = newProjectile.GetComponent<Projectile>(); //now we have a reference of the current projectile that we have loaded also it means that turret is not empty anymore
        currentProjectileLoaded.turretOwner = this; // there is an owner of projectile and it has get set properties in the projectile class so projectile object has an owner in type of turretProjectile 
        currentProjectileLoaded.resetProjectile();
        newProjectile.SetActive(true);
 
    }

    public void resetTurretProjectile()
    {

        currentProjectileLoaded = null;
    }
}
