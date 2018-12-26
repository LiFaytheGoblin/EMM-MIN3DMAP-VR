using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeController : MonoBehaviour
{

    public bool LoadData = true;
    public GameObject NodePrefab;

    public GameObject root = null;
    public GameObject NodeContainer;
    public GameObject selectedNode;

    public List<GameObject> Nodes = new List<GameObject>();

    public Material NodeMaterial;
    public Material selectedNodeMaterial;

    public Material lineColor;

    public int idCounter = 0;


    // Use this for initialization
    void Start()
    {
        if(LoadData) loadData();
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Use this for initialization
    public void selectNode(GameObject node)
    {
        if (selectedNode == null)
        {
            selectedNode = node;
        }
        selectedNode.GetComponent<Node>().setMaterial(NodeMaterial);
        node.GetComponent<Node>().setMaterial(selectedNodeMaterial);
        selectedNode = node;
    }

    public LineRenderer createLine(GameObject newNode)
    {
        if (selectedNode != null)
        {
            // GameObject go = new GameObject();
            LineRenderer line = newNode.AddComponent<LineRenderer>();
            line.startWidth = .01f;
            line.endWidth = .01f;
            line.material = lineColor;
            line.SetPosition(0, selectedNode.transform.position);
            line.SetPosition(1, newNode.transform.position);
            return line;
        }
        return null;
    }


    // Load Save Data
    void loadData()
    {
        /*
         This function initiates the loading process by first
         starting the loading process in the DataController and then
         loading the received data into the Node object's property variables.
         */
        List<NodeData> newData = DataController.Instance.Load();
        if(newData != null)
        {
            createLoadedNodes(newData);
        }
    }

    void createLoadedNodes(List<NodeData> d)
    {
        /*
         This recursive function traverses the tree of 
         nodes and creates a map from it.
         */
        // always create at least one node
        foreach (NodeData n in d)
        {
            Vector3 currentPosition = new Vector3(n.xPos, n.yPos, n.zPos);
            GameObject currentNode = Instantiate(NodePrefab, currentPosition, Quaternion.identity);
            idCounter = n.id + 1;
            currentNode.GetComponent<Node>().data = n;
            if (root == null)
            {
                root = currentNode;
                selectNode(currentNode);
            }
            else
            {
                GameObject parent = findNodeById(n.parentId);
                if (parent != null)
                {
                    selectNode(parent);
                    currentNode.GetComponent<Node>().parent = parent;
                    currentNode.GetComponent<Node>().parent.GetComponent<Node>().children.Add(currentNode);
                }
                createLine(currentNode);
            }
            currentNode.transform.parent = NodeContainer.transform;
            Nodes.Add(currentNode);
        }
    }

    GameObject findNodeById(int id)
    {
        foreach (GameObject node in Nodes)
        {
            if (node.GetComponent<Node>().data.id == id)
            {
                return node;
            }
        }
        return null;
    }

    void OnDestroy()
    {
        /*
        This function will initiate the copying of local data to the DataController
        and then initiate the saving process in DataContoller.
         */
        Debug.Log("onDestury");
        Debug.Log("onDestury data size" + DataController.Instance.data.Count);
        DataController.Instance.Save();
    }



}
