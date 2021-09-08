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
    //public int selected_level;
    //public string selected_tag;

    public int selected_level { get; set; }
    public string selected_tag { get; set; }

    public static Action<turretSettings,int> onPlaceTurret;  //share information of this turretLoaded in this turretCard
    

    //private List<GameObject> listOfUpgradableTurretButtons;
    public turretSettings turretLoaded { get; set; }

    public void setTurretButton(turretSettings turSet) //turret settings is our scriptable object, which stores all required informations needed here like cost, sprite etc.
    {
        turretLoaded = turSet; // turretsettings are setted by outer class
        turretImage.sprite = turSet.turretShopSprite;
        turretCost.text = turSet.costOfTurretOnShop.ToString();
         
    }
    public void setTurretUpgradeOptionsButton(turretSettings turSet,int level,string tag_) //turret settings is our scriptable object, which stores all required informations needed here like cost, sprite etc.
    {
        turretLoaded = turSet; // turretsettings are setted by outer class
        turretImage.sprite = turSet.turretShopSprite;
        turretCost.text = level.ToString();
        selected_level = level;
        selected_tag=tag_;

    }
    public void placeTurret()
    {
        if (currencySystem.Instance.totalCoins >=turretLoaded.costOfTurretOnShop) //if we have enought coin to do it
        //if(firstSelected.turretToPlace.tag == selected_tag && )
        {
            currencySystem.Instance.removeCoins(turretLoaded.costOfTurretOnShop);
            UiManager.Instance.closeTurretShopPanel();
            if (onPlaceTurret !=null)
            {
                onPlaceTurret.Invoke(turretLoaded,selected_level); // give info about this turretsettings
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
