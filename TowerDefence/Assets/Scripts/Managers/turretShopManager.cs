using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class turretShopManager : Singleton<turretShopManager>
{
    [SerializeField] private GameObject turretCardPrefab;
    [SerializeField] private Transform turretPanelContainer;
    [SerializeField] private Transform turretUpgradeContainer;

    [Header("Turret Settings")]
    [SerializeField] private turretSettings[] turrets;

    private node _currentNodeSelected;
    private bool lookingForUpgradeMatch;
    private List<GameObject> listOfUpgradableTurretButtons;

    //required reference to button prefab because we need to access to  information inside of turret card 
    // Start is called before the first frame update
    void Start()
    {
        lookingForUpgradeMatch = false;
        listOfUpgradableTurretButtons = new List<GameObject>();
        
        for (int i = 0; i < turrets.Length; i++)
        {
            createTurretButton(turrets[i]);//creating buttons on shop panel
        }
    }

    public void spawnTurret(node node_)
    {
            System.Random rnd = new System.Random();
             int num = rnd.Next(0,4);
             GameObject turretInstance = Instantiate(turrets[num].turretPrefab);
            turretInstance.transform.localPosition = node_.transform.position; // instantiate at this node 
            turretInstance.transform.parent = node_.transform;

            Turret turretPlaced = turretInstance.GetComponent<Turret>();
            node_.setTurret(turretPlaced); // place the selected turret at the selected node

        
    }
    /*
    private void placeTurret(turretSettings loadedTurret,int level) // this is onclick function of turretbutton
    {
        /*
        if (_currentNodeSelected != null) // if there is a node selected, then instantiate the required turret at the selected node
        {
            GameObject turretInstance = Instantiate(loadedTurret.turretPrefab);
            turretInstance.transform.localPosition = _currentNodeSelected.transform.position; // instantiate at this node 
            turretInstance.transform.parent = _currentNodeSelected.transform;

            Turret turretPlaced = turretInstance.GetComponent<Turret>();
            _currentNodeSelected.setTurret(turretPlaced); // place the selected turret at the selected node

        }
        //
        */
        //
        /*
        if (!lookingForUpgradeMatch)
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
       
        
        else if (_currentNodeSelected != null)
        {
            node firstSelected = UiManager.Instance.getFirstSelectedNode();
            lookingForUpgradeMatch = false;
            //firstSelected.turretToPlace.turretUpgrade.level== 
            if (level== _currentNodeSelected.turretToPlace.turretUpgrade.level)
            {

                destroySecondTurret();
                firstSelected.turretToPlace.turretUpgrade.upgradeTurret();
            }
            

        }
    }
    */
    private void destroySecondTurret()
    {
        List<GameObject> listOfNodes_= nodeManager.Instance.getNodeList();
        for (int i = 0; i < listOfNodes_.Count; i++)
        {
            node no = listOfNodes_[i].GetComponent<node>();
            if (!no.IsEmpty())
            {
               Turret tur = no.turretToPlace;
                if (tur!=_currentNodeSelected.turretToPlace&& tur.turretUpgrade.level==_currentNodeSelected.turretToPlace.turretUpgrade.level)
                {
                    no.destroyTheTurret();
                    break; // iki if var sorun olabilir
                }

            }
        }
    }
    
    private void turretDestroyed()
    {
        _currentNodeSelected = null;
    }

    private void lookingUpgradeMatch()
    {
        lookingForUpgradeMatch = true;
        //lookingForUpgradeMatch = false;
    }
    private void OnEnable()
    {
        node.onNodeSelected += nodeSelected;
        //turretCard.onPlaceTurret += placeTurret;
        node.onTurretDestroyed += turretDestroyed;
        //UiManager.onUpgradePanel += lookingUpgradeMatch;
    }
    private void OnDisable()
    {
        node.onNodeSelected -= nodeSelected;
        //turretCard.onPlaceTurret -= placeTurret;
        node.onTurretDestroyed -= turretDestroyed;
       // UiManager.onUpgradePanel -= lookingUpgradeMatch;


    }

    private void nodeSelected(node selectedNode)
    {
        _currentNodeSelected = selectedNode;

    }

    public void upgradeOnClick()
    {

        List<GameObject> nodeList = nodeManager.Instance.getNodeList();
        for (int i = 0; i < nodeList.Count; i++)
        {
            
            node node_ = nodeList[i].GetComponent<node>();

            if (!node_.IsEmpty() && node_ != _currentNodeSelected&&node_.turretToPlace.tag == _currentNodeSelected.turretToPlace.tag)
            {
                Turret turretOnNode = node_.turretToPlace;
                int level = turretOnNode.turretUpgrade.level;

                /*
                if (turretOnNode != null)
                {

                */
                if (_currentNodeSelected.turretToPlace.turretUpgrade.level == level)
                {


                    if (turretOnNode.tag == "oneMissile")
                    {
                        createUpgradeTurretButton(turrets[0], level);
                    }
                    else if (turretOnNode.tag == "twoMissile")
                    {
                        createUpgradeTurretButton(turrets[1], level);
                    }
                    else if (turretOnNode.tag == "diffTwoMissile")
                    {
                        createUpgradeTurretButton(turrets[2], level);
                    }
                    else if (turretOnNode.tag == "machine")
                    {
                        createUpgradeTurretButton(turrets[3], level);
                    }
                }
            }
            
        }
    }

    private void createTurretButton(turretSettings turSet)
    {

      

        GameObject ins = Instantiate(turretCardPrefab,turretPanelContainer.position,Quaternion.identity); //each instantiated button has turretCard script which comes from its prefab
        ins.transform.SetParent(turretPanelContainer);
        ins.transform.localScale = Vector3.one; // for preventing possible errors which grid layout can cause, it resets the scale


        turretCard cardButton = ins.GetComponent<turretCard>();
        cardButton.setTurretButton(turSet); //set turret button with these turretsettings
    }

    public void destroyClosedPanelItems()
    {
        for (int i = 0; i < listOfUpgradableTurretButtons.Count; i++)
        {
            Destroy(listOfUpgradableTurretButtons[i]);
            //listOfUpgradableTurretButtons.RemoveAt(i);
        }
        listOfUpgradableTurretButtons = new List<GameObject>();
    }

    private void createUpgradeTurretButton(turretSettings turSet,int level_)
    {



        GameObject ins = Instantiate(turretCardPrefab, turretUpgradeContainer.position, Quaternion.identity); //each instantiated button has turretCard script which comes from its prefab
        ins.transform.SetParent(turretUpgradeContainer);
        ins.transform.localScale = Vector3.one; // for preventing possible errors which grid layout can cause, it resets the scale

       
        turretCard cardButton = ins.GetComponent<turretCard>();
        cardButton.setTurretUpgradeOptionsButton(turSet,level_,_currentNodeSelected.tag); //set turret button with these turretsettings
        listOfUpgradableTurretButtons.Add(cardButton.gameObject);
        ins.SetActive(true);
    }
}
