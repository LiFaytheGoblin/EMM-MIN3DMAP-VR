using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using Leap.Unity;
using System;
using UnityEngine.UI;

public class GesturesManager : MonoBehaviour
{

    /// <summary>  
    ///  The Node Controller in the scene
    /// </summary>  
    public NodeController ND;

    /// <summary>  
    ///  The VR Player GameObject
    /// </summary>  
    public GameObject VRCamera;

    /// <summary>  
    ///  The pointer material if it hits the ground
    /// </summary>  
    public Material pointerColorGround;

    /// <summary>  
    ///  The pointer material if it hits a node
    /// </summary>  
    public Material pointerColorNode;

    /// <summary>  
    ///  The pointer material if it doesn't hit anything
    /// </summary>  
    public Material pointerColor;

    /// <summary>  
    ///  The transportation cylinder gameObject
    /// </summary>  
    public GameObject cylinder;

    /// <summary>  
    ///  The pointer raycast start
    /// </summary>  
    public GameObject handPos;

    /// <summary>  
    ///  The pointer raycast end by node
    /// </summary> 
    public GameObject handtoNode;


    //public GameObject handtoGround;

    /// <summary>  
    ///  The pointer LineRenderer
    /// </summary> 
    LineRenderer line;

    /// <summary>  
    ///  check if pointer gesture is on
    /// </summary>
    public bool pointer = false;

    /// <summary>  
    ///  check if thump gesture is on
    /// </summary>
    public bool thump = false;


    //public bool isNode = false;

    /// <summary>  
    ///  Leap motion Service Provider
    /// </summary>  
    LeapServiceProvider provider;

    /// <summary>  
    ///  The time of last the transportation
    /// </summary>  
    DateTime lastTransport = DateTime.Now;

    /// <summary>  
    ///  check if pointer is Active 
    /// </summary> 
    public bool PointerActive = true;


    // Use this for initialization
    void Start()
    {
        provider = FindObjectOfType<LeapServiceProvider>() as LeapServiceProvider;
        GameObject go = new GameObject();
        line = go.AddComponent<LineRenderer>();
        go.SetActive(false);
    }

    /// <summary>  
    /// Check every frame if pointer is active and check if it hit something
    /// </summary> 
    void Update()
    {
        if (pointer && PointerActive)
        {
            Frame frame = provider.CurrentFrame;
            foreach (Hand hand in frame.Hands)
            {
                if (hand.IsRight)
                {
                    Vector3 from = handPos.transform.position;
                    Vector3 to;
                    //if (isNode)
                    //{
                    to = handtoNode.transform.position;
                    //}
                    //else
                    //{
                    //    to = handtoGround.transform.position;
                    //}
                    RaycastHit[] hits;
                    hits = Physics.RaycastAll(from, to - from);
                    if (hits.Length > 0)
                    {
                        bool hitFound = false;
                        for (int i = 0; i < hits.Length; i++)
                        {
                            RaycastHit hit = hits[i];
                            if (hit.transform.tag == "Node")
                            {
                                hitFound = true;
                                if (thump)
                                {
                                    ND.selectNode(hit.transform.parent.gameObject);
                                }
                                Debug.DrawRay(from, to - from, Color.yellow);
                                drawLine(from, hit.point, pointerColorNode);
                                cylinder.SetActive(false);
                            }
                            else if (hit.transform.tag == "Ground" && !hitFound)
                            {
                                hitFound = true;
                                Debug.Log("Ground hit");
                                Vector3 hitPos = hit.point;
                                hitPos.y = 0;
                                cylinder.SetActive(true);
                                cylinder.transform.position = hitPos;
                                if (thump)
                                {
                                    var seconds = (DateTime.Now - lastTransport).TotalSeconds;
                                    if (seconds > 3)
                                    {
                                        VRCamera.transform.position = hitPos;
                                        lastTransport = DateTime.Now;
                                    }
                                }
                                Debug.DrawRay(from, to - from, Color.magenta);
                                drawLine(from, hit.point, pointerColorGround);
                            }
                            else
                            {
                                //  Debug.Log("else hit");
                                //  Debug.Log(hit.transform.name);
                                //   Debug.DrawRay(from, to - from, Color.white);
                                //  drawLine(from, to, pointerColor);
                                //   cylinder.SetActive(false);
                            }
                        }
                        if (!hitFound)
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

    /// <summary>  
    /// Move the pointer and change its material
    /// </summary> 
    /// <param name="v1">The start point</param>
    /// <param name="v2">The end point</param>
    /// <param name="material">The pointer material</param>
    void drawLine(Vector3 v1, Vector3 v2, Material material)
    {
        line.startWidth = .01f;
        line.endWidth = .01f;
        line.material = material;
        line.SetPosition(0, v1);
        line.SetPosition(1, v2);
    }

    /// <summary>  
    /// Show Pointer when the pointer gesture is on
    /// </summary> 
    public void showPointer()
    {
        pointer = true;
        if (PointerActive) line.gameObject.SetActive(true);

    }

    /// <summary>  
    /// Hide Pointer when the pointer gesture is on
    /// </summary> 
    public void hidePointer()
    {
        pointer = false;
        line.gameObject.SetActive(false);
        cylinder.SetActive(false);
    }

    /// <summary>  
    /// The thump gesture is on
    /// </summary> 
    public void thumpPressed()
    {
        thump = true;
    }

    /// <summary>  
    /// The thump gesture is off
    /// </summary> 
    public void thumpRelease()
    {
        thump = false;
    }

    //public void NodePointer()
    //{
    //    isNode = true;
    //}

    //public void GroundPointer()
    //{
    //    isNode = false;
    //}

    /// <summary>  
    /// Turn the pointer on and off with the hand menu button
    /// </summary> 
    /// <param name="createMoveModeBtn">The Text UI for the pointerOnOff button in the hand Menu</param>
    public void PointerActiveBtn(Text PointerActiveBtn)
    {
        PointerActive = !PointerActive;
        if (PointerActive)
        {
            PointerActiveBtn.text = "Pointer:On";
            line.gameObject.SetActive(true);
        }
        else
        {
            PointerActiveBtn.text = "Pointer:Off";
            line.gameObject.SetActive(false);
        }

    }
}
