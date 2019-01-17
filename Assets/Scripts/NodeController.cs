using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>  
///  This class manage all the Nodes  
/// </summary>  
public class NodeController : MonoBehaviour
{

    /// <summary>  
    ///  Load map from File
    /// </summary>  
    public bool LoadData = true;

    /// <summary>  
    ///  The node Prefab
    /// </summary>  
    public GameObject NodePrefab;

    /// <summary>  
    ///  The root node
    /// </summary>  
    public GameObject root = null;

    /// <summary>  
    ///  The Container for all Nodes
    /// </summary>  
    public GameObject NodeContainer;

    /// <summary>  
    ///  The current selected node 
    /// </summary>  
    public GameObject selectedNode;

    /// <summary>  
    ///  List contain all the nodes in the map
    /// </summary>  
    public List<GameObject> Nodes = new List<GameObject>();

    /// <summary>  
    /// Not selected Node Material
    /// </summary>  
    public Material NodeMaterial;

    /// <summary>  
    /// selected Node Material
    /// </summary>  
    public Material selectedNodeMaterial;

    /// <summary>  
    /// The material of the connection between nodes
    /// </summary>  
    public Material lineColor;

    /// <summary>  
    /// The next not used NodeId
    /// </summary>  
    public int idCounter = 0;


    /// <summary>  
    /// Load the map from file at start
    /// </summary>  
    void Start()
    {
        if (LoadData) loadData();
    }


    /// <summary>  
    /// Select another node
    /// </summary>  
    /// <param name="node">The node to select</param>
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

    /// <summary>
    /// Create LineRenderer between the selected Node and the newNode param
    /// </summary>
    /// <param name="newNode">newNode is the GameObject of the new Node</param>
    /// <returns>return the LineRenderer Component between the two nodes</returns>
    public LineRenderer createLine(GameObject newNode)
    {
        if (selectedNode != null)
        {
            // GameObject go = new GameObject();
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
    /// Calculate which connect point to use to connect to another node
    /// </summary>
    /// <param name="node1">The node which we calculate the connect point for</param>
    /// <param name="node2">The other node</param>
    /// <returns>return the position of the correct Connect Point</returns>
    public static Vector3 getConnectsPoint(GameObject node1, GameObject node2)
    {
        float distUp = Vector3.Distance(node1.GetComponent<Node>().ConnectsPointUp.transform.position, node2.transform.position);
        float distDown = Vector3.Distance(node1.GetComponent<Node>().ConnectsPointDown.transform.position, node2.transform.position);
        float distLeft = Vector3.Distance(node1.GetComponent<Node>().ConnectsPointLeft.transform.position, node2.transform.position);
        float distRight = Vector3.Distance(node1.GetComponent<Node>().ConnectsPointRight.transform.position, node2.transform.position);
        List<float> list = new List<float> { distUp, distDown, distLeft, distRight };
        float myMin = list.Min();
        if (myMin == distUp)
        {
            return node1.GetComponent<Node>().ConnectsPointUp.transform.position;
        }
        else if (myMin == distDown)
        {
            return node1.GetComponent<Node>().ConnectsPointDown.transform.position;
        }
        else if (myMin == distLeft)
        {
            return node1.GetComponent<Node>().ConnectsPointLeft.transform.position;
        }
        else
        {
            return node1.GetComponent<Node>().ConnectsPointRight.transform.position;
        }

    }

    /** public static Vector3 getConnectsPoint(GameObject node1, GameObject node2)
   {
       Vector3 relativePoint = node1.transform.InverseTransformPoint(node2.transform.position);
       if (relativePoint.x > 0)
       {
           Debug.Log("Right");
           if (relativePoint.y > 0)
           {
               Debug.Log("Above");
               if (relativePoint.x > relativePoint.y) return node1.GetComponent<Node>().ConnectsPointRight.transform.position;
               else return node1.GetComponent<Node>().ConnectsPointUp.transform.position;
           }
           else
           {
               Debug.Log("Below");
               if (relativePoint.x > -relativePoint.y) return node1.GetComponent<Node>().ConnectsPointRight.transform.position;
               else return node1.GetComponent<Node>().ConnectsPointDown.transform.position;
           }
       }
       else
       {
           Debug.Log("Left");
           if (relativePoint.y > 0)
           {
               Debug.Log("Above");
               if (-relativePoint.x > relativePoint.y) return node1.GetComponent<Node>().ConnectsPointLeft.transform.position;
               else return node1.GetComponent<Node>().ConnectsPointUp.transform.position;
           }
           else
           {
               Debug.Log("Below");
               if (-relativePoint.x > -relativePoint.y) return node1.GetComponent<Node>().ConnectsPointLeft.transform.position;
               else return node1.GetComponent<Node>().ConnectsPointDown.transform.position;
           }
       }

     

}   **/

    // Load Save Data

    /// <summary>  
    ///  This function initiates the loading process by first
    ///  starting the loading process in the DataController and then
    ///  loading the received data into the Node object's property variables.
    /// </summary>  
    void loadData()
    {
        List<NodeData> newData = DataController.Instance.Load();
        if (newData != null)
        {
            createLoadedNodes(newData);
        }
    }


    /// <summary>  
    ///   This recursive function traverses the tree of
    ///   nodes and creates a map from it.
    /// </summary>  
    /// <param name="d">list with all the loaded nodes data</param>
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
    /// </summary>  
    /// <param name="id">The node id</param>
    /// <returns>The node GameObject</returns>
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


    /// <summary>  
    ///   Save the map to file before destroy
    /// </summary>  
    void OnDestroy()
    {
        Debug.Log("onDestury");
        Debug.Log("onDestury data size" + DataController.Instance.data.Count);
        DataController.Instance.Save();
    }



}
