using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Node : MonoBehaviour
{
    /// <summary>  
    /// Node data to save later to a file
    /// </summary> 
    public NodeData data = new NodeData();

    /// <summary>  
    /// Node parent GameObject
    /// </summary> 
    public GameObject parent = null;

    /// <summary>  
    /// Node children GameObjects List
    /// </summary> 
    public List<GameObject> children = new List<GameObject>();

    /// <summary>  
    /// Node name as Text UI
    /// </summary> 
    public Text UIText;

    /// <summary>  
    /// The node cube GameObject
    /// </summary> 
    public GameObject cube;

    //ConnectsPoints

    /// <summary>  
    /// Connects Point Up
    /// </summary> 
    public GameObject ConnectsPointUp;

    /// <summary>  
    /// Connects Point Down
    /// </summary> 
    public GameObject ConnectsPointDown;

    /// <summary>  
    /// Connects Point Left
    /// </summary> 
    public GameObject ConnectsPointLeft;

    /// <summary>  
    /// Connects Point Right
    /// </summary> 
    public GameObject ConnectsPointRight;


    /// <summary>  
    /// The VR camera
    /// </summary> 
    private Transform camera;


    /// <summary>  
    /// Initialization
    /// </summary> 
    void Start()
    {
        camera = Camera.main.transform;
        UIText = gameObject.GetComponentInChildren<Text>();
        UIText.text = data.text;
    }

    /// <summary>  
    ///  Rotate every frame toward the camera
    /// </summary> 
    void Update()
    {
        transform.LookAt(camera);
        if (parent != null)
        {
            this.GetComponent<LineRenderer>().SetPosition(0, NodeController.getConnectsPoint( parent, this.gameObject));
            this.GetComponent<LineRenderer>().SetPosition(1, NodeController.getConnectsPoint(this.gameObject, parent));
        }
    }

    /// <summary>  
    ///  set node cube Material
    /// </summary> 
    public void setMaterial(Material m)
    {
        cube.GetComponent<Renderer>().material = m;
    }

    /// <summary>  
    /// Reconnect all connections to the new position
    /// </summary> 
    public void rePostion()
    {
        foreach (GameObject node in children)
        {
            node.GetComponent<LineRenderer>().SetPosition(0, NodeController.getConnectsPoint(this.gameObject,node));
        }
        if(parent != null){
            this.GetComponent<LineRenderer>().SetPosition(1, NodeController.getConnectsPoint(this.gameObject, parent));
        }
        resetData();
    }

    /// <summary>  
    /// Save the node data to te data game object to save it later to a file
    /// </summary> 
    public void resetData()
    {
        data.xPos = transform.position.x;
        data.yPos = transform.position.y;
        data.zPos = transform.position.z;
        if(parent != null)
        {
            data.parentId = parent.GetComponent<Node>().data.id;
        }
    }


}
