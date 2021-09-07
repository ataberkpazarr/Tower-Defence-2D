using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turretShopManager : MonoBehaviour
{
    [SerializeField] private GameObject turretCardPrefab;
    [SerializeField] private Transform turretPanelContainer;

    [Header("Turret Settings")]
    [SerializeField] private turretSettings[] turrets;

    private node _currentNodeSelected; 

    //required reference to button prefab because we need to access to  information inside of turret card 
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < turrets.Length; i++)
        {
            createTurretButton(turrets[i]);//creating buttons on shop panel
        }
    }
    private void placeTurret(turretSettings loadedTurret) // this is onclick function of turretbutton
    {
        if (_currentNodeSelected != null) // if there is a node selected, then instantiate the required turret at the selected node
        {
            GameObject turretInstance = Instantiate(loadedTurret.turretPrefab);
            turretInstance.transform.localPosition = _currentNodeSelected.transform.position; // instantiate at this node 
            turretInstance.transform.parent = _currentNodeSelected.transform;

            Turret turretPlaced = turretInstance.GetComponent<Turret>();
            _currentNodeSelected.setTurret(turretPlaced); // place the selected turret at the selected node

        }
    }
    private void turretDestroyed()
    {
        _currentNodeSelected = null;
    }
    private void OnEnable()
    {
        node.onNodeSelected += nodeSelected;
        turretCard.onPlaceTurret += placeTurret;
        node.onTurretDestroyed += turretDestroyed;
    }
    private void OnDisable()
    {
        node.onNodeSelected -= nodeSelected;
        turretCard.onPlaceTurret -= placeTurret;
        node.onTurretDestroyed -= turretDestroyed;

    }

    private void nodeSelected(node selectedNode)
    {
        _currentNodeSelected = selectedNode;

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
