using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class damageTextManager : Singleton<damageTextManager>
{


    public ObjectPooler pooler { get; set; }

   
   
    // Start is called before the first frame update
    void Start()
    {
        pooler = GetComponent<ObjectPooler>();
    }

    
}
