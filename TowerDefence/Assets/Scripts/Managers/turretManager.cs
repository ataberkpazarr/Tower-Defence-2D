using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class turretManager : Singleton<turretManager>
{


    
    [SerializeField] private GameObject [] turretsArray; //stores of turret Prefabs, used in instantiation

    public static Action<node,string> onPrevGameTurretLoaded; //informing turrets to update their levels when they preloaded

    private node _currentNodeSelected; //the node which currently selected

    System.Random rnd;
    //required reference to button prefab because we need to access to  information inside of turret card 
    // Start is called before the first frame update
    void Start()
    {
        rnd = new System.Random(); // initializaing in here for preventing memoization
        checkIfThisIsNotAnewGame(); // if this game is first-ever or already played

    }

    private void checkIfThisIsNotAnewGame()
    {
        if (PlayerPrefs.HasKey("saved"))
        {

            spawnSavedTurrets();
        }
        else
        {
            PlayerPrefs.SetInt("saved", 1); // since this creation, it will always go above if, which will reload previous game's turrets
        }
    }
    
    private void spawnSavedTurrets() // this method for spawning saved turrets
    {
        List<GameObject> allNodes = nodeManager.Instance.getNodeList(); // get the nodelist in the map
        for (int i = 0; i < allNodes.Count; i++) //iterate in all nodes
        {
            node no = allNodes[i].GetComponent<node>(); 
            

            string first_key = nodeManager.Instance.getOrderNumberOfNode(no).ToString();
            string second_key = PlayerPrefs.GetString(first_key + "type");

            //I have scriptable objects array which provide me to reach the turret prefabs, and 0 is for one missile, 1 is for two missile etc..
            if (second_key == "oneMissile") //if saved turret is one missile type
            {
                loadOldTurrets(0,no,first_key); //then spawn it at the node no with level where it will be found by first_key
            }
            else if (second_key == "twoMissile")
            {
                loadOldTurrets(1,no, first_key);

            }
            else if (second_key == "diffTwoMissile")
            {
                loadOldTurrets(2,no, first_key);

            }
            else if (second_key == "machine")
            {
                loadOldTurrets(3,no, first_key);

            }
        }

    }

    private void loadOldTurrets(int num,node node_,string firstKey) // num for which turretPrefab, the turret will be loaded, corresponds
    {

        GameObject turretInstance = Instantiate(turretsArray[num]);

        turretInstance.transform.localPosition = node_.transform.position; // instantiate at this node 
        turretInstance.transform.parent = node_.transform;

        Turret turretPlaced = turretInstance.GetComponent<Turret>();
        node_.setTurret(turretPlaced); // place the selected turret at the selected node    
        turretPlaced.this_node = node_;
    }

    public void spawnTurret(node node_) //randomly spawn a turret
    {
            
              int num = rnd.Next(0,4); // randomization
              GameObject turretInstance = Instantiate(turretsArray[num]);

             turretInstance.transform.localPosition = node_.transform.position; // instantiate at this node 
             turretInstance.transform.parent = node_.transform;

             Turret turretPlaced = turretInstance.GetComponent<Turret>();
             node_.setTurret(turretPlaced); // place the selected turret at the selected node
             turretPlaced.this_node = node_;
             int whichNodeIsThis = nodeManager.Instance.getOrderNumberOfNode(node_);

            //upgradeLevelManager.Instance.resetLevel(whichNodeIsThis.ToString());manager
            //upgradeLevelManager.Instance.setType(whichNodeIsThis.ToString()+"type",turretPlaced.tag);manager

            //reset related playerPrefs
            PlayerPrefs.DeleteKey(whichNodeIsThis.ToString());
            PlayerPrefs.SetInt(whichNodeIsThis.ToString(),1); // saved level, it is 1 since it newly spawned   
            PlayerPrefs.SetString(whichNodeIsThis.ToString() + "type",turretPlaced.tag);

    }
   
    private void turretDestroyed() // called when a turret is destroyed, for updatig current selected node
    {
        _currentNodeSelected = null;
    }
   
    private void OnEnable()
    {
        node.onNodeSelected += nodeSelected; //when a node is selected, it is being invoked
        node.onTurretDestroyed += turretDestroyed; //when a turretDestoryed
    }

    private void OnDisable()
    {
        node.onNodeSelected -= nodeSelected;
        node.onTurretDestroyed -= turretDestroyed;
    }

    private void nodeSelected(node selectedNode) //invoked when a new node selected
    {
        _currentNodeSelected = selectedNode;

    }

}
