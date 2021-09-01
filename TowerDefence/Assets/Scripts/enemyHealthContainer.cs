using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class enemyHealthContainer : MonoBehaviour
{
   [SerializeField] private Image fillAmountImage;

    public Image getFillAmountImage()
    {
        return fillAmountImage;
    }
}
