using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turretUpgrade : MonoBehaviour
{
    //everytime a turrete upgraded, the upgrade cost should be increased 
    // also we need to upgrade the damage value
    [SerializeField] private int upgradeInitialCost;
    [SerializeField] private int upgradeCostIncremental;
    [SerializeField] private float damageIncremental;
    [SerializeField] private float delayReducer;

    private turretProjectile turretProjectile_; //reference to turretProjectile class, to be able to upgrade our damage

    public int upgradeCost { get; set; }


    // Start is called before the first frame update
    void Start()
    {
        turretProjectile_ = GetComponent<turretProjectile>();
        upgradeCost = upgradeInitialCost;

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
             upgradeTurret();

        }
    }

    private void upgradeTurret() //turret is being upgraded there exist enough coins
    {
        if (currencySystem.Instance.totalCoins >= upgradeCost)
            {

            turretProjectile_.Damage += damageIncremental; // so when turret is upgraded, turret's damage value also will be upgraded
            turretProjectile_.delayAfterShot -= delayReducer; // so it will fire more often since it is upgraded
            updateUpgradeCost();
            }
    }

    private void updateUpgradeCost() // 
    {
        currencySystem.Instance.removeCoins(upgradeCost);
        upgradeCost += upgradeCostIncremental;
    }
}
