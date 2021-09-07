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

    [Header("Destroy Turret")]
    [SerializeField] private float sellPert;

    public float sellPerct { get; set; }

    private turretProjectile turretProjectile_; //reference to turretProjectile class, to be able to upgrade our damage

    public int upgradeCost { get; set; }
    public int level { get; set; } //which level our turret is


    // Start is called before the first frame update
    void Start()
    {
        turretProjectile_ = GetComponent<turretProjectile>();
        upgradeCost = upgradeInitialCost;

        level = 1; //user prefs koy
        //sellPerct = sellPert;

    }

    private void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.D))
        {
             upgradeTurret();

        }*/
    }

    public void upgradeTurret() //turret is being upgraded if there exist enough coins, 
    {
        if (currencySystem.Instance.totalCoins >= upgradeCost)
            {

            turretProjectile_.Damage += damageIncremental; // so when turret is upgraded, turret's damage value also will be upgraded
            turretProjectile_.delayAfterShot -= delayReducer; // so it will fire more often since it is upgraded
            updateUpgradeCost();
            level += 1;
            }
    }

    /*
    public int getSellValue()
    {
        int sellVal = Mathf.RoundToInt(upgradeCost*sellPerct);
        return sellVal;
    }*/

    private void updateUpgradeCost() // 
    {
        currencySystem.Instance.removeCoins(upgradeCost);
        upgradeCost += upgradeCostIncremental;
    }
}
