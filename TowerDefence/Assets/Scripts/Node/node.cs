using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using UnityEngine;

public class node : MonoBehaviour
{
    public static Action onTurretDestroyed;
    public static Action<node> onNodeSelected; // which node wants to open shop panel
    public Turret turretToPlace { get; set; }

    [SerializeField] private GameObject attackRangeSprite;
    [SerializeField] private Text levelText;

    private float rangeSize;
    private Vector3 rangeOriginalSize;



    // Start is called before the first frame update
    void Start()
    {
        rangeSize = attackRangeSprite.GetComponent<SpriteRenderer>().bounds.size.y; // reaching its range 
        rangeOriginalSize = attackRangeSprite.transform.localScale;
        

    }
    /*
    private void OnGUI()
    {
        GUI.Label(new Rect(transform.position.x + 100, transform.position.y + 200, 20, 20), "Level " + turretToPlace.turretUpgrade.level.ToString(), levelStyle);


    }*/

    // Update is called once per frame
    void Update()
    {
        if (turretToPlace != null)
        {
            try
            {
                levelText.text  = " Level " + turretToPlace.turretUpgrade.level.ToString();
            }
            catch 
            {
            
            }
            
            

            //updateLevelText

        }
        else if(levelText.gameObject.activeInHierarchy)
        {
            levelText.gameObject.SetActive(false);
        }

    }

    private void updateLevelText()
    {
    }

    public void setTurret(Turret tur)
    {
        turretToPlace = tur;
        levelText.gameObject.SetActive(true); // when turret is placed, then we also need to show level text of it

    }

    public bool IsEmpty() // if the node is empty or not
    {
        return turretToPlace == null; 
    }

    public void selectTurret() //called in our button
    {
        if (onNodeSelected != null) // if there is any subscriber, UImanager listenes this event
        {
            onNodeSelected.Invoke(this); //tell subscribers that this node wants to open the panel
        }

        if (!IsEmpty()) // if this node is node empty
        {
            showTurretInfo();

        }
    }

    public void closeThePanel()
    { }

    public void closeAttackRangeSprite()
    {
        attackRangeSprite.SetActive(false);
    }

    private void showTurretInfo()
    {
        attackRangeSprite.SetActive(true);
        attackRangeSprite.transform.localScale = rangeOriginalSize * turretToPlace.AttackRange / (rangeSize/2);
    }

    public void destroyTheTurret()
    {
        if (!IsEmpty())
        {
            //currencySystem.Instance.AddCoins(turretToPlace.turretUpgrade.upgradeCost);
            Destroy(turretToPlace.gameObject);
            turretToPlace = null;
            attackRangeSprite.SetActive(false);
            if (onTurretDestroyed != null)
            {
                onTurretDestroyed.Invoke();

            }
        }
    }

}
