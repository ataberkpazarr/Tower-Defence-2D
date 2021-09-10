using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class enemyHealthContainer : MonoBehaviour
{
   [SerializeField] private Image fillAmountImage;

    public Image getFillAmountImage() //every enemy's health bar needs this image and they creates their images by calling this function
    {
        return fillAmountImage; //fillable, changeable white bar which shows health proportion
    }
}
