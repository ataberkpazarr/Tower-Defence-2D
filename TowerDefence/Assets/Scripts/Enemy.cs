  using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;
     private Waypoint waypoint;

    public static Action<Enemy> onEndReach;

    public float MoveSpeed { get; set; } // want to change it if it is necessary from outer classes

    // each position in our waypoint (path) has an index, first position is at index of zero, second pos is at index of one etc..
    //so enemy class should be able to keep track of which index it will move to

    private int nextWayPointIndex;
    private Vector3 lastVisitedPointPosition; //storing last passed point position of path, in order to decide to the rotation of enemy
    private enemyHealth enemyHealth_;
    private SpriteRenderer spriteRenderer;

    public enemyHealth EnemyHealth { get; set; }

    public Vector3 currentNextPositionCasted => waypoint.GetWaypointPosition(nextWayPointIndex);

    
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        nextWayPointIndex = 0;
        MoveSpeed = moveSpeed;
        enemyHealth_ = GetComponent<enemyHealth>();
        lastVisitedPointPosition = transform.position;
        EnemyHealth = GetComponent<enemyHealth>();

    }
    private void Update()
    {
        Move();
        rotateDirectionOfEnemy();
        if (nextPointPositionReached())
        {
            
            updateNextPointIndex();
        }
    }
    /*
    private void OnEnable()
    {
        levelManager.onGameOver += stopMovement;  //stop the movement when game is over
    }
    private void OnDisable()
    {
        levelManager.onGameOver -= stopMovement;

    }*/
    public void stopMovement()
    {
        MoveSpeed = 0f;
    }

    public void continueMovement()
    {
        MoveSpeed = moveSpeed;
    }

    private void rotateDirectionOfEnemy()
    {
        if (currentNextPositionCasted.x > lastVisitedPointPosition.x) // so we are going in the right direction
        {
            spriteRenderer.flipX = false;
        }

        else //going in the left direction
        {
            spriteRenderer.flipX = true;

        }
    }
    public void setWaypoint(Waypoint w)
    {
        waypoint = w;
    }

    private bool nextPointPositionReached()
    {
        float distanceToNextPointPos = (transform.position - currentNextPositionCasted).magnitude;

        if (distanceToNextPointPos <0.1f)
        {
            lastVisitedPointPosition = transform.position;
            return true;
        }

        else
        {
            return false;
        }
        
    }
    private void Move()
    {

        //Vector3 currentPosition = waypoint.GetWaypointPosition(currentWayPointIndex);
        transform.position = Vector3.MoveTowards(transform.position, currentNextPositionCasted, MoveSpeed* Time.deltaTime);
    }

    private void updateNextPointIndex()
    {
        int lastWaypointIndex = waypoint.Points.Length - 1;
        if (nextWayPointIndex < lastWaypointIndex)
        {
            nextWayPointIndex++;
        }
        else // enemy at the last position which means that it succeded to finish the path despite turrets 
        {
            returnEnemyToPool(); //if the enemy is at the last position of path we should return in to object pool
        }
    }

    private void returnEnemyToPool()
    {
        if (onEndReach != null) //if there exists any class that subscribed to this event
        {
            onEndReach.Invoke(this);
        }
        enemyHealth_.resetHealth();
        ObjectPooler.returnToPool(this.gameObject);

    }

    //_currentWaypointIndex bende nextWayPointIndex vs vs 
    public void resetEnemy()
    {
        nextWayPointIndex = 0;

    }


}
