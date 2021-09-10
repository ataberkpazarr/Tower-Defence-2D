using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class machineTurretProjectile : turretProjectile
{

    protected override void Update()
    {
        if (Time.time > nextAttackTime) // if it is time
        {
            if (turret.currentEnemyTarget!=null) //if have an enemy
            {
                Vector2 directionToTarget = turret.currentEnemyTarget.transform.position - transform.position; // our direction
                fireProjectile(directionToTarget);
            }
            
            nextAttackTime = Time.time + delayBetweenAttacks;
        }
        updateDamage_();
    }

    private void updateDamage_()
    {
        Damage = 10f + (turret.DamageIncremental * PlayerPrefs.GetInt(nodeManager.Instance.getOrderNumberOfNode(turret.this_node).ToString()));
    }

    private void fireProjectile(Vector3 dir) // go in this direction, not locked to target like other turret projectiles
    {

        GameObject ins = pooler.GetInstanceFromPool();
        ins.transform.position = projectileSpawnPosition.position;
        machineProjectile projectile = ins.GetComponent<machineProjectile>();
        projectile.direction = dir;
        projectile.damage = Damage; //Right Damage is the Damage of turretProjectile
        ins.SetActive(true);
    }


}
