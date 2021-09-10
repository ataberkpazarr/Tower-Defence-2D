using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    //protected for using them in child classes lie machineProjectile
    [SerializeField] protected float moveSpeedOfProjectile = 10f; 

    [SerializeField] protected float minimumDistanceToDamage = 0.1f;

    public float damage { get; set; } // it will be going to be determined when we load a new projectile. It will be determined by turretProjectile class

    public turretProjectile turretOwner { get; set; } // every projectile has an turretOwner in the type of turretProjectile and

    protected Enemy enemyTarget;

    //enemyFX subscribed to it
    public static Action<Enemy, float> onEnemyHitAnim; //which enemy is colliding with projectile thats why <enemy> and amount of damage <float>


    protected virtual void Update()
    {
        if (enemyTarget != null) // if we have an enemy to attack, which is inside of turret's sphere rigidbody, certain seconds after the last send missile
        {
            moveProjectile();
            rotateProjectile();
        }
    }




    protected virtual void moveProjectile() // going to be called in update, as long as projectile has an enemy target
    {
        transform.position = Vector2.MoveTowards(transform.position, enemyTarget.transform.position, moveSpeedOfProjectile * Time.deltaTime);
        float distanceToTarget = (enemyTarget.transform.position - transform.position).magnitude;

        //to be able to damage enemy, the missile should be close enough to enemy
        if (distanceToTarget <minimumDistanceToDamage) // so turret achieved to damage enemy
        {
            if (onEnemyHitAnim !=null)
            {
                onEnemyHitAnim.Invoke(enemyTarget,turretOwner.Damage); //enemy target, for indicating collided enemy, damage for the amount
            }
            enemyTarget.EnemyHealth.dealDamage(turretOwner.Damage);
            turretOwner.resetTurretProjectile();
            ObjectPooler.returnToPool(this.gameObject);
        }
    }

    private void rotateProjectile()
    {
        Vector3 enemyPos = enemyTarget.transform.position - transform.position;
        float angle = Vector3.SignedAngle(transform.up,enemyPos,transform.forward);
        transform.Rotate(0,0,angle);
    }

    public void setEnemy(Enemy en)
    {
        enemyTarget = en;  
    }

    public void resetProjectile() // projectile should be resetted before it returned to objectpool because when it will be set active again, It should be initialized based on that moments properties like rotation etc
    {
        enemyTarget = null;
        transform.localRotation = Quaternion.identity; // projectile is child of projectileSpawnPosObject, thus it should be local rotation, intead global
         
    }

}
