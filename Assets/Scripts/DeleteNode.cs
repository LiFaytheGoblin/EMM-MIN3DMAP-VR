using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteNode : MonoBehaviour
{

    public NodeController ND;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void deleteSelectedNode()
    {
        //foreach (Node n in ND.selectedNode.GetComponentsInChildren<Node>(true)) //include inactive
        //{
        //    DataController.Instance.data.Remove(n.data);
        //    Destroy(n.gameObject);
        //}
        GetChildRecursive(ND.selectedNode);
    }

    private void GetChildRecursive(GameObject node)
    {
         if (null == node)
            return ;

        foreach (GameObject child in node.GetComponent<Node>().children)
        {
            if (null == child)
                continue;
            //child.gameobject contains the current child you can do whatever you want like add it to an array
            GetChildRecursive(child.gameObject);
            ND.Nodes.Remove(child.gameObject);
            Destroy(child.gameObject);
        }
        ND.Nodes.Remove(node);
        Destroy(node);
    }

}
