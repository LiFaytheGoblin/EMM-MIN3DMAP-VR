using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>  
///  This class manages all the Nodes. Managing includes selecting nodes,
///  creating new nodes and connecting nodes.
/// </summary>  
public class NodeController : MonoBehaviour
{
    public bool LoadData = true; //!< Load map from File
    public GameObject NodePrefab; //!< The node Prefab
    public GameObject root = null; //!< The root node
    public GameObject NodeContainer; //!< The Container for all Nodes
    public GameObject selectedNode; //!< The current selected node
    public List<GameObject> Nodes = new List<GameObject>(); //!< List contain all the nodes in the map
    public Material NodeMaterial; //!< Not selected Node Material
    public Material selectedNodeMaterial; //!< selected Node Material
    public Material lineColor; //!< The material of the connection between nodes
    public int idCounter = 0; //!< The next not used NodeId
    
    void Start()
    {
        if (LoadData) loadData();
    }


    /// <summary>  
    ///  Selects a node
    ///  
    ///  @param[in] node The node to select
    /// </summary>
    public void selectNode(GameObject node)
    {
        if (selectedNode == null) selectedNode = node;
        selectedNode.GetComponent<Node>().setMaterial(NodeMaterial);
        node.GetComponent<Node>().setMaterial(selectedNodeMaterial);
        selectedNode = node;
    }

    /// <summary>
    ///  Creates a line between the selected Node and a new Node
    ///  
    /// @param[in]  newNode     the GameObject of the new Node
    /// 
    /// \return     the LineRenderer Component between the two nodes
    /// </summary>
    public LineRenderer createLine(GameObject newNode)
    {
        if (selectedNode != null)
        {
            LineRenderer line = newNode.AddComponent<LineRenderer>();
            line.startWidth = .01f;
            line.endWidth = .01f;
            line.material = lineColor;
            line.SetPosition(0, getConnectsPoint(selectedNode, newNode));
            line.SetPosition(1, getConnectsPoint(newNode, selectedNode));
            return line;
        }
        return null;
    }


    /// <summary>
    ///   Calculate which connect point to use to connect to another node
    ///   
    /// @param[in]  node1   The node which we calculate the connect point for
    /// @param[in]  node2   The other node
    /// 
    /// \return     the position of the correct Connection Point
    /// </summary>
    public static Vector3 getConnectsPoint(GameObject node1, GameObject node2)
    {
        float distUp = Vector3.Distance(node1.GetComponent<Node>().ConnectsPointUp.transform.position, node2.transform.position);
        float distDown = Vector3.Distance(node1.GetComponent<Node>().ConnectsPointDown.transform.position, node2.transform.position);
        float distLeft = Vector3.Distance(node1.GetComponent<Node>().ConnectsPointLeft.transform.position, node2.transform.position);
        float distRight = Vector3.Distance(node1.GetComponent<Node>().ConnectsPointRight.transform.position, node2.transform.position);

        List<float> list = new List<float> { distUp, distDown, distLeft, distRight };
        float myMin = list.Min();

        if (myMin == distUp) return node1.GetComponent<Node>().ConnectsPointUp.transform.position;
        else if (myMin == distDown) return node1.GetComponent<Node>().ConnectsPointDown.transform.position;
        else if (myMin == distLeft) return node1.GetComponent<Node>().ConnectsPointLeft.transform.position;
        else return node1.GetComponent<Node>().ConnectsPointRight.transform.position;

    }

    /// <summary>  
    ///  This function initiates the loading process by first
    ///  starting the loading process in the DataController and then
    ///  loading the received data into the Node object's property variables.
    /// </summary>  
    void loadData()
    {
        List<NodeData> newData = DataController.Instance.Load();
        if (newData != null) createLoadedNodes(newData);
    }


    /// <summary>  
    ///   This recursive function traverses the tree of
    ///   nodes and creates a map from it.
    ///   
    /// @param[in] d   the List of Nodes that should be displayed
    /// </summary>
    void createLoadedNodes(List<NodeData> d)
    {
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

    /// <summary>  
    ///  Search for node using the node id
    ///  
    /// @param[in] id the id of the Node you want to find
    ///   \return    the node with the entered id or null
    /// </summary>  
    GameObject findNodeById(int id)
    {
        foreach (GameObject node in Nodes)
        {
            if (node.GetComponent<Node>().data.id == id) return node;
        }
        return null;
    }


    /// <summary>  
    ///   Save the map to file before destroy
    /// </summary>  
    void OnDestroy()
    {
        DataController.Instance.Save();
    }
}
