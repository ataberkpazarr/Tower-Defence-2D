using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class turretCard : MonoBehaviour //attached to turretButton 
{
    [SerializeField] private Image turretImage;
    [SerializeField] private TextMeshProUGUI turretCost;

    public void setTurretButton(turretSettings turSet) //turret settings is our scriptable object, which stores all required informations needed here like cost, sprite etc.
    {
        turretImage.sprite = turSet.turretShopSprite;
        turretCost.text = turSet.costOfTurretOnShop.ToString();
         
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
