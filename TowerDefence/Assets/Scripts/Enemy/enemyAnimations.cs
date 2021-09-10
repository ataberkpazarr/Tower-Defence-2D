using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyAnimations : MonoBehaviour
{
    private Animator animator; //for reaching setted animations
    private Enemy enemy;

    private enemyHealth enemyHealth;
    private void Start()
    {
        animator = GetComponent<Animator>();
        enemy = GetComponent<Enemy>();
        enemyHealth = GetComponent<enemyHealth>();
    }

    private float getCurrentAnimationLength()
    {
        float animLenght = animator.GetCurrentAnimatorStateInfo(0).length; // take the length of the animation which is playing right now
        return animLenght;
    }
    private void playHurtAnimation()
    {
        animator.SetTrigger("HurtTrigger"); // already set trigger name in Unity under animator
         
    }

    private IEnumerator playHurtAnim()
    {
        enemy.stopMovement();
        playHurtAnimation();
        yield return new WaitForSeconds(getCurrentAnimationLength() +0.15f);
        enemy.continueMovement();

    }

    private void playDieAnimation()
    {
        animator.SetTrigger("dieTrigger"); 

    }

    private IEnumerator playDieAnim() 
    {
        enemy.stopMovement();
        playDieAnimation();
        yield return new WaitForSeconds(getCurrentAnimationLength() + 0.3f);
        enemy.continueMovement();
        enemyHealth.resetHealth();
        ObjectPooler.returnToPool(enemy.gameObject);

    }

    private void OnEnable() // when enemy killed or hitted, there will be animation played thats why these events are subscribed
    {
        enemyHealth.onEnemyHitted += enemyHit;
        enemyHealth.onEnemyKilled += enemyDead;


    }

    private void OnDisable()
    {
        enemyHealth.onEnemyHitted -= enemyHit;
        enemyHealth.onEnemyKilled -= enemyDead;

    }

    private void enemyDead(Enemy en)
    {
        if (enemy ==en)
        {
            StartCoroutine(playDieAnim());
        }
        
    }
    private void enemyHit(Enemy en) 
    {

       
        if (enemy == en) // if the enemy that is attached in the same game object where the enemyanimations class is attached, is the same as the enemy that has been hitted, play that animation in this enemy
        {
            StartCoroutine(playHurtAnim());
        }
        
    }
}
