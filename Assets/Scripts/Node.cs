using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Node : MonoBehaviour
{

    //  public int id;
    //   public int parentId;
    //   public List<int> childrenIds;
    public NodeData data = new NodeData();

    public GameObject parent = null;
    public List<GameObject> children = new List<GameObject>();
    public Text UIText;
    public GameObject cube;


    private Transform camera;

    // Use this for initialization
    void Start()
    {
        camera = Camera.main.transform;

        UIText = gameObject.GetComponentInChildren<Text>();
        UIText.text = data.text;
        //UIText.horizontalOverflow = HorizontalWrapMode.Wrap;
        //UIText.verticalOverflow = VerticalWrapMode.Truncate;
        //UIText.resizeTextForBestFit = true;
        //UIText.supportRichText = true;
        //Debug.Log(UIText.rectTransform.rect.width + " --- " + UIText.rectTransform.rect.height);
    }

    // Update is called once per frame
    void Update()
    {
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
        if(parent != null){
            this.GetComponent<LineRenderer>().SetPosition(1, this.transform.position);
        }
        resetData();
    }

    public void resetData()
    {
        data.xPos = transform.position.x;
        data.yPos = transform.position.y;
        data.zPos = transform.position.z;
        if(parent != null)
        {
            data.parentId = parent.GetComponent<Node>().data.id;
        }
        //foreach (GameObject child in children)
        //{
        //    data.childrenIds.Add(child.GetComponent<Node>().data.id);
        //}

    }


}
