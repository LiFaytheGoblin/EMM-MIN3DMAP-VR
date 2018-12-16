using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour {

    public GameObject parent = null;
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

  
}
