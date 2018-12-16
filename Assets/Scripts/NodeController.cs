using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeController : MonoBehaviour {

    public GameObject root = null;
    public GameObject NodeContainer;
    public GameObject selectedNode;

    public Material NodeMaterial;
    public Material selectedNodeMaterial;

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
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
