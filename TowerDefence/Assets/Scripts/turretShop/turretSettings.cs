 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Turret Shop Settings")]
public class turretSettings : ScriptableObject 
    //the information about each turret like, turret image, cost etc 
    //should be stored and scriptable objects can be used for it, since they do not need to be attached to an active object
{
    //these will be accessed by turretCard, used in creation of menu

    public GameObject turretPrefab; // when clicked to turret, we are waiting it to be pop up
     public int costOfTurretOnShop;
    public Sprite turretShopSprite; 





}
