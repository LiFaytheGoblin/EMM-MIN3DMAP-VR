using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>  
///  This class manage delete node 
/// </summary>  
public class DeleteNode : MonoBehaviour
{

    /// <summary>  
    ///  The Node Controller in the scene
    /// </summary>  
    public NodeController ND;

    /// <summary>  
    ///  The delete question UI
    /// </summary>  
    public GameObject UIStage;

    /// <summary>  
    ///  Show the delete question UI
    /// </summary>  
    public void ShowUI()
    {
        UIStage.SetActive(true);
    }

    /// <summary>  
    ///  Hide the delete question UI
    /// </summary>  
    public void HideUI()
    {
        UIStage.SetActive(false);
    }

    /// <summary>  
    ///  Delete the selected node
    /// </summary>  
    public void deleteSelectedNode()
    {
        GetChildRecursive(ND.selectedNode);
        UIStage.SetActive(false);
    }

    /// <summary>  
    ///  Delete node and all its children
    /// </summary>  
    /// <param name="node">The node to delete</param>
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
