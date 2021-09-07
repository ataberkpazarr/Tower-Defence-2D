using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;   


public class turretCard : MonoBehaviour //attached to turretButton 
{
    [SerializeField] private Image turretImage;
    [SerializeField] private TextMeshProUGUI turretCost;

    public static Action<turretSettings> onPlaceTurret;  //share information of this turretLoaded in this turretCard

    public turretSettings turretLoaded { get; set; }

    public void setTurretButton(turretSettings turSet) //turret settings is our scriptable object, which stores all required informations needed here like cost, sprite etc.
    {
        turretLoaded = turSet; // turretsettings are setted by outer class
        turretImage.sprite = turSet.turretShopSprite;
        turretCost.text = turSet.costOfTurretOnShop.ToString();
         
    }
    public void placeTurret()
    {
        if (currencySystem.Instance.totalCoins >=turretLoaded.costOfTurretOnShop) //if we have enought coin to do it
        {
            currencySystem.Instance.removeCoins(turretLoaded.costOfTurretOnShop);
            UiManager.Instance.closeTurretShopPanel();
            if (onPlaceTurret !=null)
            {
                onPlaceTurret.Invoke(turretLoaded); // give info about this turretsettings
            }
        }     
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
