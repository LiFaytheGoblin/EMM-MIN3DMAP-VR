using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using Leap.Unity;

public class GesturesManager : MonoBehaviour
{

    public NodeController ND;
    public GameObject VRCamera;
    public Material pointerColorGround;
    public Material pointerColorNode;
    public Material pointerColor;

    public GameObject cylinder;

    public GameObject handPos;
    public GameObject handtoNode;
    public GameObject handtoGround;

    LineRenderer line;
    public bool pointer = false;
    public bool thump = false;
    public bool isNode = false;
    public Vector3 toOffset;

    LeapServiceProvider provider;


    // Use this for initialization
    void Start()
    {
        provider = FindObjectOfType<LeapServiceProvider>() as LeapServiceProvider;
        GameObject go = new GameObject();
        line = go.AddComponent<LineRenderer>();
        go.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (pointer)
        {
            Frame frame = provider.CurrentFrame;
            foreach (Hand hand in frame.Hands)
            {
                if (hand.IsRight)
                {
                    Vector3 from = handPos.transform.position;
                    Vector3 to;
                    if (isNode)
                    {
                         to = handtoNode.transform.position;
                    }
                    else
                    {
                         to = handtoGround.transform.position;
                    }
                    RaycastHit hit;
                    if (Physics.Raycast(from, to - from, out hit))
                    {
                      
                       
                        if (hit.transform.tag == "Ground")
                        {
                            Vector3 hitPos = hit.point;
                            hitPos.y = 0;
                            cylinder.SetActive(true);
                            cylinder.transform.position = hitPos;
                            if (thump)
                            {
                                VRCamera.transform.position = hitPos;

                            }
                            Debug.DrawRay(from, to - from, Color.magenta);
                            drawLine(from, hit.point, pointerColorGround);
                        }
                        else if (hit.transform.tag == "Node")
                        {
                            if (thump)
                            {
                                ND.selectNode(hit.transform.parent.gameObject);
                                Debug.Log("Note with Thump");
                            }
                            Debug.DrawRay(from, to - from, Color.yellow);
                            drawLine(from, hit.point, pointerColorNode);
                            cylinder.SetActive(false);
                        }
                        else
                        {
                            Debug.DrawRay(from, to - from, Color.white);
                            drawLine(from, to, pointerColor);
                                cylinder.SetActive(false);
                            }
                    }
                    else
                    {
                        Debug.DrawRay(from, to - from, Color.white);
                        drawLine(from, to, pointerColor);
                        cylinder.SetActive(false);
                    }
                }
            }
        }
    }

    void drawLine(Vector3 v1, Vector3 v2, Material material)
    {
        line.startWidth = .01f;
        line.endWidth = .01f;
        line.material = material;
        line.SetPosition(0, v1);
        line.SetPosition(1, v2);
    }

    public void showPointer()
    {
        pointer = true;
        line.gameObject.SetActive(true);
    }

    public void hidePointer()
    {
        pointer = false;
        line.gameObject.SetActive(false);
        cylinder.SetActive(false);
    }

    public void thumpPressed()
    {
        thump = true;
    }

    public void thumpRelease()
    {
        thump = false;
    }

    public void NodePointer()
    {
        isNode = true;
    }

    public void GroundPointer()
    {
        isNode = false;
    }
}
