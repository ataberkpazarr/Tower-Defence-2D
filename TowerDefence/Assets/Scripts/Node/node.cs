using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using UnityEngine;

public class node : MonoBehaviour
{
    public static Action onTurretDestroyed; // fire when a turret is destroyed
    public static Action<node> onNodeSelected; // which node wants to open shop panel
    public Turret turretToPlace { get; set; } //turret which exists in this node

    [SerializeField] private GameObject attackRangeSprite; //for showing range
    [SerializeField] private Text levelText; 

    // required for showing turret's range when its clicked
    private float rangeSize;
    private Vector3 rangeOriginalSize; 



    void Start()
    {
        rangeSize = attackRangeSprite.GetComponent<SpriteRenderer>().bounds.size.y; // reaching its range 
        rangeOriginalSize = attackRangeSprite.transform.localScale;

    }
   
    void Update()
    {
        if (turretToPlace != null) // if turret is not empty, then update level text which gives level info of the located turret
        {
            try
            {
                
               
                string key_ = nodeManager.Instance.getOrderNumberOfNode(this).ToString();
                string  lev = PlayerPrefs.GetInt(key_).ToString();
                levelText.text  = " Level " + lev;

            }
            catch 
            {
            
            }
        }
        else if(levelText.gameObject.activeInHierarchy) //if node is empty then set false the level text
        {
            levelText.gameObject.SetActive(false);
        }

    }

    public void setTurret(Turret tur) //this node, has this turret 
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
        
        if (!IsEmpty()) // if this node is node empty, meaning have a turret
        {
            showTurretInfo();

        }
    }


    public void closeAttackRangeSprite()
    {
        attackRangeSprite.SetActive(false);
    }

    private void showTurretInfo() //when turret clicked, its range will be shown in the game
    {
        attackRangeSprite.SetActive(true);
        attackRangeSprite.transform.localScale = rangeOriginalSize * turretToPlace.AttackRange / (rangeSize/2);
    }

    public void destroyTheTurret()
    {
        if (!IsEmpty())
        {
            int whichNodeWeDestroyed = nodeManager.Instance.getOrderNumberOfNode(this); //in which node does the turret, that will be destroyed, placed

          
            PlayerPrefs.DeleteKey(whichNodeWeDestroyed.ToString()); //delete level key
            PlayerPrefs.SetInt(whichNodeWeDestroyed.ToString(),1); // set empty node's level to 1 
          

            if (PlayerPrefs.HasKey(whichNodeWeDestroyed.ToString() + "type")) //delete the type storage of this turret to, which identifies the type, machine, single missile etc..
            {
                PlayerPrefs.DeleteKey(whichNodeWeDestroyed.ToString() + "type");
            }

            //reset and destroy
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
