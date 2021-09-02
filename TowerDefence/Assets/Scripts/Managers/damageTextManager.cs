using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class damageTextManager : MonoBehaviour
{


    public ObjectPooler pooler { get; set; }

    public static damageTextManager instance; //singleton

    private void Awake()
    {
        instance = this; //singleton
    }
    // Start is called before the first frame update
    void Start()
    {
        pooler = GetComponent<ObjectPooler>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
