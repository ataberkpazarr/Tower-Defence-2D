using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class machineProjectile : Projectile
{

    public Vector2 direction { get; set; } //for storing the direction
    override protected  void Update() 
    {
        
            moveProjectile();
        
    }

    override protected  void moveProjectile() //moving behaviour will be different, not locked on enemy
    {

        Vector2 movement = direction.normalized * moveSpeedOfProjectile * Time.deltaTime;
        transform.Translate(movement);

    }
    private void OnEnable()
    {
        StartCoroutine(ObjectPooler.returnThePoolWithDelay(gameObject, 5f));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag=="Enemy")
        {
            Enemy en = collision.GetComponent<Enemy>();

            if (en.EnemyHealth.getCurrentHealth()>0) //if enemy has health
            {
                if (onEnemyHitAnim!=null)
                {
                    onEnemyHitAnim.Invoke(en, damage);
                }
                en.EnemyHealth.dealDamage(damage); //damage variable comes from parent projectile class 

            }

            ObjectPooler.returnToPool(this.gameObject );
        }
    }


}
