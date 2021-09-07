using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public turretUpgrade turretUpgrade { get; set; } // turret should know which level it is
    [SerializeField] private float attackRange = 3f;

    public float AttackRange => attackRange;

    private bool gameStarted;

    private List<Enemy> enemyList; //storing all the enemies in the range of our turret 

    public Enemy currentEnemyTarget { get; set; }

    private void Start()
    {
        gameStarted = true;
        enemyList = new List<Enemy>();
        turretUpgrade = GetComponent<turretUpgrade>(); 
    }

    private void Update()
    {
        getCurrentEnemyTarget();
        rotateTurretTowardsTarget();
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
    private void getCurrentEnemyTarget()
    {
        if (enemyList.Count <=0) // if there is no enemy in the range 
        {
            currentEnemyTarget = null;
            return;
        }

        currentEnemyTarget = enemyList[0]; //first entered one to range
    }
    private void OnDrawGizmos()
    {
        
        if (!gameStarted)
        {
            GetComponent<CircleCollider2D>().radius=attackRange ;

        }

        Gizmos.DrawWireSphere(transform.position,attackRange); // drawing attack range for the scene View
    }

    private void OnTriggerEnter2D(Collider2D collision) // if enemy is in the range of our circle collider  
    {
        if (collision.tag =="Enemy")
        {
            Enemy inRangeEnemy = collision.GetComponent<Enemy>();
            enemyList.Add(inRangeEnemy);

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
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
