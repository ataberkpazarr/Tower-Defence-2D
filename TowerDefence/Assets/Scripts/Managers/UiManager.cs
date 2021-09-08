using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System;


public class UiManager : Singleton<UiManager>
{
    [Header("Panels")]
    [SerializeField] private GameObject turretShopPanel; // need reference to our turretShopPanel
    [SerializeField] private GameObject nodeUiPanel;
    [SerializeField] private GameObject upgradePanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject turretUpgradePanel;


    [Header("Text")]
    [SerializeField] private TextMeshProUGUI upgradeText;
    [SerializeField] private TextMeshProUGUI sellText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI totalCoinText;
    [SerializeField] private TextMeshProUGUI livesText;
    [SerializeField] private TextMeshProUGUI currentWaveText;
    [SerializeField] private TextMeshProUGUI gameOverCoinsText;

    public bool user_move_Enabled { get; set; }

    private node currentNodeSelected;
    private bool lookingForMatchATurretToUpgrade =false;
    public bool areWeInMatchPanel { get; set; }
    
    private node firstSelectedNode;
    private node secondSelectedNode;

    public static Action onUpgradePanel; 


    private void Start()
    {
        firstSelectedNode = null;
        areWeInMatchPanel = false;
    }

    private void Update()
    {
        totalCoinText.text = currencySystem.Instance.totalCoins.ToString();
        livesText.text = levelManager.Instance.totalLives.ToString();
        currentWaveText.text = "Wave " +levelManager.Instance.currentWave.ToString();

        if (lookingForMatchATurretToUpgrade)
        {

        }

    }
    private void OnEnable()
    {
        node.onNodeSelected += nodeSelected;
    }
    private void OnDisable()
    {
        node.onNodeSelected -= nodeSelected;

    }

    public void closeTurretShopPanel() // called when the turret is placed to the desired node 
    {

        turretShopPanel.SetActive(false);

    }

    public void closeTurretTurretUpgrade() // called when the turret is placed to the desired node 
    {
        turretShopManager.Instance.destroyClosedPanelItems();

        turretUpgradePanel.SetActive(false);
        firstSelectedNode = null;
    }
    public void closeNodeUiPanel()
    {
        turretShopManager.Instance.destroyClosedPanelItems();

        currentNodeSelected.closeAttackRangeSprite();
        nodeUiPanel.SetActive(false);
        currentNodeSelected.turretToPlace.selected = false;
    }


    private void updateUpgradeText()
    { 
            upgradeText.text = currentNodeSelected.turretToPlace.turretUpgrade.upgradeCost.ToString();


    }

    private void updateTurretLevelText()
    {
        levelText.text = "Level" + currentNodeSelected.turretToPlace.turretUpgrade.level;

    }
    private void showNodeUI() // if current node selected is not null
    {
        nodeUiPanel.SetActive(true);
        updateUpgradeText();
        updateTurretLevelText();
        //updateSell();


    }

    public void showGameOverPanel()
    {
        gameOverPanel.SetActive(true);
        gameOverCoinsText.text = currencySystem.Instance.totalCoins.ToString();
        
    }
    public void restartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void showUpgradePanel() // onclick function of upgrade button which is permanent at view
    {
        upgradePanel.SetActive(true);
    }
    private bool checkIfEmptyExists()
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
        List<GameObject> nodeList_ = nodeManager.Instance.getNodeList();
        bool notFound = true;
        while (notFound)
        {
            System.Random rnd = new System.Random();
            int num = rnd.Next(0, 8);

            node no = nodeList_[num].GetComponent<node>();
            if (no.IsEmpty())
            {
                turretShopManager.Instance.spawnTurret(no);
                notFound = false;
            }
            if (!checkIfEmptyExists())
            {
                notFound = false;
            }
        }
    }

    public void onClickForUpgrade() //onclick of upgrade button which provide upgrade to turret
    {
        if (onUpgradePanel!=null)
        {
            onUpgradePanel.Invoke();
        }

        List<GameObject> nodeList_ = nodeManager.Instance.getNodeList(); //get nodelist from node manager, check if there exists a turret which is at the same level with current selected one

        for (int i = 0; i < nodeList_.Count; i++)
        {
            node no = nodeList_[i].GetComponent<node>();
            
            if (!no.IsEmpty()&&no.turretToPlace != currentNodeSelected.turretToPlace && no.turretToPlace.turretUpgrade.level == currentNodeSelected.turretToPlace.turretUpgrade.level && currentNodeSelected.turretToPlace.tag== no.turretToPlace.tag)
            {

                currentNodeSelected.turretToPlace.turretUpgrade.upgradeTurret(); // if exists then merge them into one 
                no.destroyTheTurret(); // destroy second one
                break;
            }
        }

        closeNodeUiPanel();


      



    }

    public void destroyTurret()//onclick
    {
        currentNodeSelected.destroyTheTurret();
        currentNodeSelected = null;
        nodeUiPanel.SetActive(false);
    }

  
    public node getFirstSelectedNode()
    {
        return firstSelectedNode;
     }
    private void nodeSelected(node selectedNode) //event has node reference
    {

        
        currentNodeSelected = selectedNode;

        if (currentNodeSelected.IsEmpty()) // turret shop panel
        {
            //turretShopPanel.SetActive(true);

        }
        else //upgrade-destroy panel
        {
            currentNodeSelected.turretToPlace.selected = true;
            showNodeUI();
        }
    }
}
