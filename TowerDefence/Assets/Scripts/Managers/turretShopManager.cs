using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turretShopManager : MonoBehaviour
{
    [SerializeField] private GameObject turretCardPrefab;
    [SerializeField] private Transform turretPanelContainer;

    [Header("Turret Settings")]
    [SerializeField] private turretSettings[] turrets; 

    //required reference to button prefab because we need to access to  information inside of turret card 
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < turrets.Length; i++)
        {
            createTurretButton(turrets[i]);//creating buttons on shop panel
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void createTurretButton(turretSettings turSet)
    {
        GameObject ins = Instantiate(turretCardPrefab,turretPanelContainer.position,Quaternion.identity); //each instantiated button has turretCard script which comes from its prefab
        ins.transform.SetParent(turretPanelContainer);
        ins.transform.localScale = Vector3.one; // for preventing possible errors which grid layout can cause, it resets the scale


        turretCard cardButton = ins.GetComponent<turretCard>();
        cardButton.setTurretButton(turSet); //set turret button with these turretsettings
    }
}
