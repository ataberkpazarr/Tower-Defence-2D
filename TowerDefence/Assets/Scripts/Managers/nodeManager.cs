using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nodeManager : Singleton<nodeManager> 
{
    [SerializeField] private List<GameObject> nodeList; // node list which locates in the map

    public List<GameObject> getNodeList() // return all nodes
    {
        return nodeList;
    }

    public int getOrderNumberOfNode(node n) // which node is this, will be used for userpref keys
    {
        for (int i = 0; i < nodeList.Count; i++)
        {
            node no = nodeList[i].GetComponent<node>();
            if (no == n)
            {
                return i;
            }
        }

        return 0;
    }
}
