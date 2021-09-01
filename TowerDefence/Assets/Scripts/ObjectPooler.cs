using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [SerializeField] private GameObject prefab; //the prefab we need to preload when the game starts 
    [SerializeField] private int poolSize; //how many objects we want to preload 


    private List<GameObject> pool; //every object that we preload are needed to stored in a list to keep track of it
    private GameObject poolContainer;
   


    private void Awake()
    {
        pool = new List<GameObject>();
        poolContainer = new GameObject($"Pool - {prefab.name}");
        createPooler(); 
    }

    private GameObject createInstance() // to create a new instance
    {

        GameObject newInstance = Instantiate(prefab);
        newInstance.transform.SetParent(poolContainer.transform);
        // when the pooler reloads all of these prefabs, they dont need to be activated until they are necessary 
        newInstance.SetActive(false);
        return newInstance;
    }

    private void createPooler()
    {
        for (int i = 0; i < poolSize; i++)
        {
            pool.Add(createInstance());
        }
    }

    public GameObject GetInstanceFromPool()
    {
        for (int i = 0; i < pool.Count; i++)
        {
            if (!(pool[i].activeInHierarchy))
            {
                return pool[i];
            }
        }

        return createInstance(); // total i gececek bak buna 
    }

    public static void returnToPool(GameObject gam) //static because it provides access to it without a reference to an instance of objectpooler class
    {
        gam.SetActive(false);
    }
}
