using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nodeManager : Singleton<nodeManager>
{
    [SerializeField] private List<GameObject> nodeList;

    public List<GameObject> getNodeList()
    {
        return nodeList;
    }
}
