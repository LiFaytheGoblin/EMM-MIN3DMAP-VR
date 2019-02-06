using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>  
///  This class manages the deletion of one or multiple nodes
/// </summary>  
public class DeleteNode : MonoBehaviour
{
    public NodeController ND; //!< The Node Controller in the scene
    public GameObject UIStage; //!< The delete question UI

    /// <summary>  
    ///  Shows the delete question UI
    /// </summary>  
    public void ShowUI()
    {
        UIStage.SetActive(true);
    }

    /// <summary>  
    ///  Hides the delete question UI
    /// </summary>  
    public void HideUI()
    {
        UIStage.SetActive(false);
    }

    /// <summary>  
    ///  Deletes the selected node
    /// </summary>  
    public void deleteSelectedNode()
    {
        GetChildRecursive(ND.selectedNode);
        UIStage.SetActive(false);
    }

    /// <summary>  
    ///  Deletes a node and all its children
    ///  
    /// @param[in]  node    The node to delete
    /// </summary> 
    private void GetChildRecursive(GameObject node)
    {
        if (node == null) return;

        foreach (GameObject child in node.GetComponent<Node>().children)
        {
            if (child == null) continue;
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
