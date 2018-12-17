using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour {

    public int id;
    public int parentId;
    public List<int> childrenIds;

    public GameObject parent = null;
 //   public LineRenderer lineToParent;
    public List<GameObject> children = new List<GameObject>();
    public string text = "Unity";
    public GameObject cube;


    private Transform camera;

    // Use this for initialization
    void Start () {
        camera = Camera.main.transform;
    }
	
	// Update is called once per frame
	void Update () {
        transform.LookAt(camera);
    }

    public void setMaterial(Material m)
    {
        cube.GetComponent<Renderer>().material = m;
    }

    public void rePostion()
    {
        foreach (GameObject node in children)
        {
            node.GetComponent<LineRenderer>().SetPosition(0, this.transform.position);
        }
        this.GetComponent<LineRenderer>().SetPosition(1, this.transform.position);
    }


}
