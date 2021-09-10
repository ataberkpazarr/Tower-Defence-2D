using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System;


public class UiManager : Singleton<UiManager>
{
    [Header("Panel")]
    [SerializeField] private GameObject nodeUiPanel;

    [Header("Text")]
    [SerializeField] private TextMeshProUGUI upgradeText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI totalCoinText;
    [SerializeField] private TextMeshProUGUI totalKilledText;
    [SerializeField] private TextMeshProUGUI currentWaveText;


    public static Action<string> onLevelIncreased; // invoke it when level of Turret is increased
    public int totalKilled { get; set; } 
    private node currentNodeSelected;

    public static Action onUpgradePanel; // when upgrade panel is opened
    public static Action onTimeToRestartWave; // when player can not succeded the current Wave

    System.Random rnd; //random initialization
    private List<Enemy> activeEnemyList;  // store this wave's created enemies, until all are deactivated, do not restart or go next Wave

    private void Start()
    {
        totalKilled = 0;
        rnd = new System.Random();
        activeEnemyList = new List<Enemy>();
    }

    private void Update()
    {
        //update required texts in game scene
        totalKilledText.text = totalKilled.ToString();
        totalCoinText.text = currencySystem.Instance.totalCoins.ToString();
        currentWaveText.text = "Wave " + levelManager.Instance.currentWave.ToString();


    }

    private void OnEnable()
    {
        node.onNodeSelected += nodeSelected; // if a node is selected

    }
    private void OnDisable()
    {
        node.onNodeSelected -= nodeSelected;

    }
 

    public void destroyActiveEnemies()
    {
        for (int i = 0; i < activeEnemyList.Count; i++)
        {

            if (activeEnemyList[i].gameObject.activeInHierarchy)
            {
                activeEnemyList[i].gameObject.SetActive(false);
            }
        }
        activeEnemyList = new List<Enemy>();
        
    }
    
    public void addActiveEnemyList(Enemy en)
    {
        activeEnemyList.Add(en);
    }


    public bool isThereAnyActiveEnemy() //in the scene
    {
            for (int i = 0; i < activeEnemyList.Count; i++)
            {
                
                if (activeEnemyList[i].gameObject.activeInHierarchy)
                {
                    return true;
                }
            }
            activeEnemyList = new List<Enemy>();
            return false;
        
    }

    public void closeNodeUiPanel() //close the upgrade/destroy panel
    {
        currentNodeSelected.closeAttackRangeSprite();
        nodeUiPanel.SetActive(false);
        currentNodeSelected.turretToPlace.selected = false;
    }


    private void updateTurretLevelText() //the turret level text which locates in upgrade/destroy pop-up
    {
        string key_ = nodeManager.Instance.getOrderNumberOfNode(currentNodeSelected).ToString();
        string lev = PlayerPrefs.GetInt(key_).ToString();
        levelText.text = "Level" + lev;

    }
    private void showNodeUI() // if current node selected is not null
    {
        nodeUiPanel.SetActive(true);
        updateTurretLevelText();



    }

    private bool checkIfEmptyExists() // check if there exists an empty node
    {
        List<GameObject> nodeList_ = nodeManager.Instance.getNodeList();
        for (int i = 0; i < nodeList_.Count; i++)
        {
            node no = nodeList_[i].GetComponent<node>();

            if (no.IsEmpty())
            {
                return true;
            }
        }

        return false;
    }
    public void onClickForSpawnTurret()
    {
        StartCoroutine(spawnTurretRoutine());
    }

   

IEnumerator spawnTurretRoutine()//spawn a turret on a node, randomly
{
        
        yield return new WaitForSeconds(0.3f);
        List<GameObject> nodeList_ = nodeManager.Instance.getNodeList();
        bool notFound = true;
        if (currencySystem.Instance.totalCoins >= currencySystem.Instance.amountForSpawningTurret) // if we have enough money to spawn a turret
        {
            while (notFound)
            {

                int num = rnd.Next(0, 10); // randomization
                node no = nodeList_[num].GetComponent<node>();
                
                if (no.IsEmpty()) // if there is no turret on the node
                {
                    turretManager.Instance.spawnTurret(no);
                    currencySystem.Instance.removeCoins(); // decrease total coins when a new turret spawned
                    notFound = false;
                }
                if (!checkIfEmptyExists()) // if there is no empty location then dont look for an empty node
                {
                    notFound = false;
                }
            }
        }

    }
    private bool checkIfLevelsMatch(node n1,node n2) //if two turret's levels are same or not
    {
        string key_1 = nodeManager.Instance.getOrderNumberOfNode(n1).ToString();
        string key_2 = nodeManager.Instance.getOrderNumberOfNode(n2).ToString();

        if (PlayerPrefs.GetInt(key_1) == PlayerPrefs.GetInt(key_2))
        {
            return true;
        }
        return false;

    }
    public void onClickForUpgrade() //onclick of upgrade button which provide upgrade to turret
    {
       

        List<GameObject> nodeList_ = nodeManager.Instance.getNodeList(); //get nodelist from node manager, check if there exists a turret which is at the same level with current selected one

        for (int i = 0; i < nodeList_.Count; i++)
        {
            node no = nodeList_[i].GetComponent<node>();
            
            // if nodes are not empty, levels of turrets on the nodes are same and also types are same. Then its time for merge operation 
            if (!no.IsEmpty()&& no.turretToPlace != currentNodeSelected.turretToPlace && checkIfLevelsMatch(no,currentNodeSelected) && currentNodeSelected.turretToPlace.tag== no.turretToPlace.tag)
            {
                string whichNodeisThis = nodeManager.Instance.getOrderNumberOfNode(currentNodeSelected).ToString(); //key for level
               
                int lev_ = PlayerPrefs.GetInt(whichNodeisThis); // get current level info
                lev_ += 1; //increase it by one

                PlayerPrefs.SetInt(whichNodeisThis, lev_); //set the new level
                no.destroyTheTurret(); // destroy second one

                break;
            }
        }

        closeNodeUiPanel();

    }

    public void destroyTurret() //onclick for destroy button
    {
        currentNodeSelected.destroyTheTurret();
        currentNodeSelected = null;
        nodeUiPanel.SetActive(false);
    }


    private void nodeSelected(node selectedNode) //event has node reference
    {
 
        currentNodeSelected = selectedNode;

        if (!currentNodeSelected.IsEmpty()) // upgrade destroy panel
        {
            showNodeUI();
        }

    }

    public void deleteUserPrefs() //deletes all saved data and restarts the game
    {

        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
