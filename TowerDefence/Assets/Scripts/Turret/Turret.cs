using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Turret : MonoBehaviour
{
    

    [SerializeField] private float attackRange = 3f; // attack range of turret which can be changed by SF
    [SerializeField] private int damageIncremental; //how much does damage-power of this turret will be increased after level-up 

    [HideInInspector] public node this_node;
    //private string key1;
    public float AttackRange => attackRange; // but can not changed after the game is started

    [HideInInspector]public int DamageIncremental; // equal to above damageIncremental
    private bool gameStarted; //for draw gizmos 

    private List<Enemy> enemyList; //storing all the enemies in the range of our turret 

    public Enemy currentEnemyTarget { get; set; }

    public bool selected { get; set; } // if turrret is selected
  

    private void Start()
    {
        DamageIncremental = damageIncremental;
        gameStarted = true;
        enemyList = new List<Enemy>();
        selected = false;

    }

    private void Update()
    {
        getCurrentEnemyTarget(); //is there any one at this turret's range
        rotateTurretTowardsTarget(); // if there is, then rotate towards it
    }

    private void rotateTurretTowardsTarget()
    {
        if (!(currentEnemyTarget ==null))
        {
            Vector3 targetPos = currentEnemyTarget.transform.position - transform.position;
            float angle = Vector3.SignedAngle(transform.up,targetPos, transform.forward );
            transform.Rotate(0,0,angle);
        }


    }
    private void getCurrentEnemyTarget() //who is in this turret's range
    {
        if (enemyList.Count <=0) // if there is no enemy in the range 
        {
            currentEnemyTarget = null;
            return;
        }

        currentEnemyTarget = enemyList[0]; //first entered one to range
    }
    private void OnDrawGizmos() // for range of turret
    {
        
        if (!gameStarted)
        {
            GetComponent<CircleCollider2D>().radius=attackRange ;

        }

        if (selected)
        {

            Gizmos.DrawWireSphere(transform.position, attackRange); // drawing attack range for the scene View
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) // if enemy is in the range of our circle collider  
    {
        if (collision.tag =="Enemy")
        {
            Enemy inRangeEnemy = collision.GetComponent<Enemy>();
            enemyList.Add(inRangeEnemy);

        }
    }

    private void OnTriggerExit2D(Collider2D collision) //when enemy exits the range then there is no need to keep it in the list
    {
        if (collision.tag =="Enemy")
        {
            Enemy leavedEnemy = collision.GetComponent<Enemy>();
            if (enemyList.Contains(leavedEnemy))
            {
                enemyList.Remove(leavedEnemy);
            }
        }
    }

}
