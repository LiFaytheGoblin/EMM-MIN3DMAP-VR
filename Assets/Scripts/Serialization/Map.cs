using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>  
///  The Node object will manage the data saving to and received from
///  the global DataController object. Managing includes converting
///  data to the right format to save and setting local variables
///  of this object to the correct data. This object also initiates
///  the saving and loading processes in the DataController.
/// </summary> 
public class Map : MonoBehaviour {

    public GameObject node; //!< Node prefab to use

    public List<NodeData> localData; //!< temporary, up-to-date, local map data
    public List<GameObject> NodesList = new List<GameObject>(); //!< list of all nodes

    void Start () {
        localData = new List<NodeData>();
        loadData();
    }

    void ondestroy()
    {
        initSaving();
    }

    /// <summary>  
    ///   This function will initiate the copying of local data to the DataController
    ///   and then initiate the saving process in DataContoller.
    /// </summary> 
    void initSaving()
    {
        updateDataControllerData();
        DataController.Instance.Save();
    }

    /// <summary>  
    ///   This function updates the data in DataController.
    /// </summary> 
    void updateDataControllerData()
    {
        DataController.Instance.data = localData;
    }

    /// <summary>  
    ///  This function initiates the loading process by first
    ///  starting the loading process in the DataController and then
    ///  loading the received data into the Node object's property variables.
    /// </summary> 
    void loadData()
    {
        List<NodeData> newData = DataController.Instance.Load();
        updateLocalData(newData);
    }

    /// <summary>  
    ///  This function loads the received data into the Node object's property variables.
    ///  It then initiates the rebuild of the map based on the new data.
    ///  However, in the special case that the map is still completely empty,
    ///  it will first create the root node.
    ///  
    ///  @param[in] d   the List of Nodes that should be displayed
    /// </summary> 
    void updateLocalData(List<NodeData> d)
    {
        if (DataController.Instance.loading)
        {
            DataController.Instance.loading = false;
            if (d.Count > 0) rebuildMap();
            else createRoot();
        }
    }


    /// <summary>  
    ///   This function creates the root node. It's id is 0 and it has no parent. It does not yet have children.
    ///   After creating the root node, the map will save.
    /// </summary> 
    void createRoot()
    {
        NodeData root = new NodeData();
        root.xPos = 0;
        root.yPos = 0;
        root.zPos = 0;
        root.id = 0;
        root.text = "...";
        List<NodeData> l = new List<NodeData>();
        l.Add(root);
        createNode(l);
        initSaving();
    }

    /// <summary>  
    ///   This function calls the recursive node-creation function.
    /// </summary> 
    void rebuildMap()
    {
        createNode(DataController.Instance.data);
    }

    /// <summary>  
    ///   This recursive function traverses the tree of 
    ///   nodes and creates a map from it.
    ///   
    ///   @param[in]    d   the List of Nodes that should be created
    /// </summary> 
    void createNode(List<NodeData> d)
    {
         foreach (NodeData n in d)
        {
            Vector3 currentPosition = new Vector3(n.xPos, n.yPos, n.zPos);
            GameObject currentNode = Instantiate(node, currentPosition, Quaternion.identity);
            currentNode.GetComponent<Node>().data = n;
            GameObject parent = findNodeById(n.parentId);
            if(parent != null)
            {
                currentNode.GetComponent<Node>().parent = findNodeById(n.parentId);
                currentNode.GetComponent<Node>().parent.GetComponent<Node>().children.Add(currentNode);
            }
        }
    }

    /// <summary>  
    ///   This function returns the node with the given id
    ///   
    ///   @param[in] id the id of the Node you want to find
    ///   \return    the node with the entered id or null
    /// </summary> 
    GameObject findNodeById(int id) {
        foreach (GameObject node in NodesList){
            if(node.GetComponent<Node>().data.id == id)
            {
                return node;
            }
        }
        return null;
    }

}
