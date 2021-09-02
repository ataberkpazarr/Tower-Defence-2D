using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class machineTurretProjectile : turretProjectile
{

    protected virtual void Update()
    {
        if (Time.time > nextAttackTime)
        {
            if (turret.currentEnemyTarget!=null)
            {
                Vector2 directionToTarget = turret.currentEnemyTarget.transform.position - transform.position; // our direction
                fireProjectile(directionToTarget);
            }
            
            nextAttackTime = Time.time + delayBetweenAttacks;
        }

    }
    protected virtual void loadProjectile()
    { }

    private void fireProjectile(Vector3 dir)
    {

        GameObject ins = pooler.GetInstanceFromPool();
        ins.transform.position = projectileSpawnPosition.position;


        machineProjectile projectile = ins.GetComponent<machineProjectile>();
        projectile.direction = dir;
        ins.SetActive(true);
    }


}
