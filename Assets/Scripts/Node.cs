using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Node : MonoBehaviour
{
    public NodeData data = new NodeData(); //!< Node data to save later to a file
    public GameObject parent = null; //!< Node parent GameObject
    public List<GameObject> children = new List<GameObject>(); //!< Node children GameObjects List
    public Text UIText; //!< Node name as Text UI
    public GameObject cube; //!< The node cube GameObject

    public GameObject ConnectsPointUp; //!< Connects Point Up
    public GameObject ConnectsPointDown; //!< Connects Point Down
    public GameObject ConnectsPointLeft; //!< Connects Point Left
    public GameObject ConnectsPointRight; //!< Connects Point Right

    private Transform camera; //!< The VR camera

    void Start()
    {
        camera = Camera.main.transform;
        UIText = gameObject.GetComponentInChildren<Text>();
        UIText.text = data.text;
    }

    /// <summary>  
    ///  Rotate towards the camera for each new frame
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
    ///  Sets node cube material
    /// </summary> 
    public void setMaterial(Material m)
    {
        cube.GetComponent<Renderer>().material = m;
    }

    /// <summary>  
    ///  Reconnects all connections to the new position
    /// </summary> 
    public void rePostion()
    {
        foreach (GameObject node in children)
        {
            node.GetComponent<LineRenderer>().SetPosition(0, NodeController.getConnectsPoint(this.gameObject,node));
        }
        if(parent != null) this.GetComponent<LineRenderer>().SetPosition(1, NodeController.getConnectsPoint(this.gameObject, parent));
        resetData();
    }

    /// <summary>  
    ///  Saves the node data to te data game object to save it later to a file
    /// </summary> 
    public void resetData()
    {
        data.xPos = transform.position.x;
        data.yPos = transform.position.y;
        data.zPos = transform.position.z;
        if(parent != null) data.parentId = parent.GetComponent<Node>().data.id;
    }


}
