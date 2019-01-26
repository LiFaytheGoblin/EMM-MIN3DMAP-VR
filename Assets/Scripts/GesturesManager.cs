using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using Leap.Unity;
using System;
using UnityEngine.UI;

/// <summary>  
///  Manages the gestures recognized by LeapMotion. 
///  These gestures include: pointing & clicking
/// </summary>  
public class GesturesManager : MonoBehaviour
{
    public NodeController ND; //!< The Node Controller in the scene
    public GameObject VRCamera; //!< The VR Player GameObject
    public Material pointerColorGround; //!< The pointer material if it hits the ground
    public Material pointerColorNode; //!< The pointer material if it hits a node
    public Material pointerColor; //!< The pointer material if it doesn't hit anything
    public GameObject cylinder; //!< The transportation cylinder gameObject
    public GameObject handPos; //!< The pointer raycast start
    public GameObject handtoNode; //!< The pointer raycast end by node
    LineRenderer line; //!< The pointer LineRenderer
    public bool pointer = false; //!< check if pointer gesture is on
    public bool thump = false; //!< check if thump gesture is on
    LeapServiceProvider provider; //!< Leap motion Service Provider
    DateTime lastTransport = DateTime.Now; //!< The time of last the transportation
    public bool PointerActive = true; //!< check if pointer is Active 
    
    void Start()
    {
        provider = FindObjectOfType<LeapServiceProvider>() as LeapServiceProvider;
        GameObject go = new GameObject();
        line = go.AddComponent<LineRenderer>();
        go.SetActive(false);
    }

    /// <summary>  
    ///  Checks for each new frame if pointer is active and checks if it hit something
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
                    to = handtoNode.transform.position;
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
                                if (thump) ND.selectNode(hit.transform.parent.gameObject);
                                drawLine(from, hit.point, pointerColorNode);
                                cylinder.SetActive(false);
                            }
                            else if (hit.transform.tag == "Ground" && !hitFound)
                            {
                                hitFound = true;
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
                                drawLine(from, hit.point, pointerColorGround);
                            }
                        }
                        if (!hitFound)
                        {
                            drawLine(from, to, pointerColor);
                            cylinder.SetActive(false);
                        }
                    }
                    else
                    {
                        drawLine(from, to, pointerColor);
                        cylinder.SetActive(false);
                    }
                }
            }
        }
    }

    /// <summary>  
    /// Moves the pointer and changes its material
    /// 
    /// @param[in]  v1          The start point
    /// @param[in]  v2          The end point
    /// @param[in]  material    The pointer material
    /// </summary> 
    void drawLine(Vector3 v1, Vector3 v2, Material material)
    {
        line.startWidth = .01f;
        line.endWidth = .01f;
        line.material = material;
        line.SetPosition(0, v1);
        line.SetPosition(1, v2);
    }

    /// <summary>  
    ///  Show Pointer when the pointer gesture is on
    /// </summary> 
    public void showPointer()
    {
        pointer = true;
        if (PointerActive) line.gameObject.SetActive(true);
    }

    /// <summary>  
    ///  Hide Pointer when the pointer gesture is on
    /// </summary> 
    public void hidePointer()
    {
        pointer = false;
        line.gameObject.SetActive(false);
        cylinder.SetActive(false);
    }

    /// <summary>  
    ///  The thump gesture is on
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

    /// <summary>  
    ///  Turn the pointer on and off with the hand menu button
    ///  @param[in] createMoveModeBtn   The Text UI for the pointerOnOff button in the hand Menu
    /// </summary>
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
