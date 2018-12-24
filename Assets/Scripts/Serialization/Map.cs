using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour {
    /*
     The Node object will manage the data saving to and received from
     the global DataController object. Managing includes converting
     data to the right format to save and setting local variables
     of this object to the correct data. This object also initiates 
     the saving and loading processes in the DataController.
     */
    public GameObject node;

    public List<NodeData> localData;
    public List<GameObject> NodesList = new List<GameObject>();

    // Use this for initialization
    void Start () {
        localData = new List<NodeData>();
        loadData();
    }

    void ondestroy()
    {
        initSaving();
    }

    void initSaving()
    {
        /*
         This function will initiate the copying of local data to the DataController
         and then initiate the saving process in DataContoller.
         */
        updateDataControllerData();
        DataController.Instance.Save();
    }

    void updateDataControllerData()
    {
        /*
         This function updates the data in DataController.
         */
        DataController.Instance.data = localData;
    }

    void loadData()
    {
        /*
         This function initiates the loading process by first
         starting the loading process in the DataController and then
         loading the received data into the Node object's property variables.
         */
         
        List<NodeData> newData = DataController.Instance.Load();
        updateLocalData(newData); //feeding the function as a param will hopefully make loadData wait for the results.
    }

    void updateLocalData(List<NodeData> d)
    {
        /*
         This function loads the received data into the Node object's property variables.
         It then initiates the rebuild of the map based on the new data.
         However, in the special case that the map is still completely empty,
         it will first create the root node.
         */
        if (DataController.Instance.loading)
        {
            DataController.Instance.loading = false;
            if (d.Count > 0) rebuildMap();
            else createRoot();
        }
    }

    void createRoot()
    {
        /*
         This function creates the root node. It's id is 0 and it has no parent. It does not yet have children.
         After creating the root node, the map will save.
         */
        NodeData root = new NodeData();
        root.xPos = 0;
        root.yPos = 0;
        root.zPos = 0;
        root.id = 0;
        root.text = "Unity";
        List<NodeData> l = new List<NodeData>();
        l.Add(root);
        createNode(l);
        initSaving();
    }

    void rebuildMap()
    {
        /*
         This function deletes the current map and calls the 
         recursive node-creation function.
         */
        deleteMap();
        createNode(DataController.Instance.data);
    }

    void deleteMap()
    {
        /*
         This function deletes the complete map.
         */
        //find root node (node with id 0)
        //call delete function root
    }

    void createNode(List<NodeData> d)
    {
        /*
         This recursive function traverses the tree of 
         nodes and creates a map from it.
         */
         // always create at least one node
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
