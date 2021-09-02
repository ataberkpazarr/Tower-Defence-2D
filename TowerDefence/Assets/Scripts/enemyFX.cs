using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
// if certain event is subscribed, this new event will be fired when a projectile 
//collides an enemy and this class will respond this event by getting an instance of animated 
//text which is inside of pooler that created in the manager 

public class enemyFX : MonoBehaviour 

{
    [SerializeField] private Transform textDamagedSpawnPos;
    private Enemy en;

    private void Start()
    {
        en = GetComponent<Enemy>();
    }
    private void OnEnable()
    {
        Projectile.onEnemyHitAnim += enemyHitHandler;
    }

    private void OnDisable()
    {
        Projectile.onEnemyHitAnim -= enemyHitHandler;

    }

    private void enemyHitHandler(Enemy enemy, float damage)
    {
        //we need to check if the enemy that has been collidied with the projectile, is the same enemy where this class is attached to 
        if (en == enemy)
        {
            GameObject dmgTextSingleton = damageTextManager.instance.pooler.GetInstanceFromPool();
            TextMeshProUGUI damageText = dmgTextSingleton.GetComponent<damageText>().damageTextTMP;
            damageText.text = damage.ToString();

            //animated text needs to follow enemy position so 
            dmgTextSingleton.transform.SetParent(textDamagedSpawnPos);
            dmgTextSingleton.transform.position = textDamagedSpawnPos.position;
            dmgTextSingleton.SetActive(true);
        }
    }
}
