using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteNode : MonoBehaviour
{

    public NodeController ND;

    public GameObject UIStage;


    public void ShowUI()
    {
        UIStage.SetActive(true);
    }

    public void HideUI()
    {
        UIStage.SetActive(false);
    }


    public void deleteSelectedNode()
    {
        GetChildRecursive(ND.selectedNode);
        UIStage.SetActive(false);
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
            DataController.Instance.data.Remove(child.GetComponent<Node>().data);
            Destroy(child.gameObject);
        }
        ND.Nodes.Remove(node);
        DataController.Instance.data.Remove(node.GetComponent<Node>().data);
        GameObject parent = node.GetComponent<Node>().parent;
        if(parent != null)
        {
            ND.selectNode(parent);
            parent.GetComponent<Node>().children.Remove(node);
        }
        Destroy(node);
    }

}
