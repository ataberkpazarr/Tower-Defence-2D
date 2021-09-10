using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class damageText : MonoBehaviour
{

    //it is attached to damageTextCanvas and there is a TMP as child of it
    public TextMeshProUGUI damageTextTMP => GetComponentInChildren<TextMeshProUGUI>(); // for adding damage value to emerged text

    public void returnTextToPool() // will be called at the end of damagetext animation
    {
        transform.SetParent(null);
        ObjectPooler.returnToPool(this.gameObject);

    }




}
