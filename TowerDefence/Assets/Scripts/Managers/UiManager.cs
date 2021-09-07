using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


public class UiManager : Singleton<UiManager>
{
    [Header("Panels")]
    [SerializeField] private GameObject turretShopPanel; // need reference to our turretShopPanel
    [SerializeField] private GameObject nodeUiPanel;
    [SerializeField] private GameObject upgradePanel;
    [SerializeField] private GameObject gameOverPanel;


    [Header("Text")]
    [SerializeField] private TextMeshProUGUI upgradeText;
    [SerializeField] private TextMeshProUGUI sellText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI totalCoinText;
    [SerializeField] private TextMeshProUGUI livesText;
    [SerializeField] private TextMeshProUGUI currentWaveText;
    [SerializeField] private TextMeshProUGUI gameOverCoinsText;


    private node currentNodeSelected;

    private void Update()
    {
        totalCoinText.text = currencySystem.Instance.totalCoins.ToString();
        livesText.text = levelManager.Instance.totalLives.ToString();
        currentWaveText.text = "Wave " +levelManager.Instance.currentWave.ToString();

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

    public void closeNodeUiPanel()
    {
        currentNodeSelected.closeAttackRangeSprite();
        nodeUiPanel.SetActive(false);
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

    public void onClickForUpgrade() //onclick of upgrade button which provide upgrade to turret
    {
        currentNodeSelected.turretToPlace.turretUpgrade.upgradeTurret();
        updateUpgradeText();
        updateTurretLevelText();
        /// update all active turrets level
        //updateSell();

    }

    public void destroyTurret()
    {
        currentNodeSelected.destroyTheTurret();
        currentNodeSelected = null;
        nodeUiPanel.SetActive(false);
    }

    /*
    private void updateSell()
    {
        int sellAmount = currentNodeSelected.turretToPlace.turretUpgrade.getSellValue() ;
        sellText.text = sellAmount.ToString();
    }*/

    private void nodeSelected(node selectedNode) //event has node reference
    {
        currentNodeSelected = selectedNode;
        if (currentNodeSelected.IsEmpty())
        {
            turretShopPanel.SetActive(true);

        }
        else
        {

            showNodeUI();
        }
    }
}
