using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class damageTextManager : Singleton<damageTextManager> // create a singleton which have a pooler for the damage texts that will be instantiating for per hit to enemy
{


    public ObjectPooler pooler { get; set; }

   
   
    // Start is called before the first frame update
    void Start()
    {
        pooler = GetComponent<ObjectPooler>();
    }

    
}
