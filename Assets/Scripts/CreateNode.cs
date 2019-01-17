using UnityEngine;
using Leap;
using Leap.Unity;
using UnityEngine.UI;

/// <summary>  
///  This class manage create or move node with Pinch Gesture
/// </summary>  
public class CreateNode : MonoBehaviour
{

    /// <summary>  
    ///  The Node Controller in the scene
    /// </summary>  
    public NodeController ND;

    /// <summary>  
    ///  Boolean to check if the node already created
    /// </summary>  
    bool prefabCreated = false;

    /// <summary>  
    ///  Boolean to check if the pinch gesture is active
    /// </summary>  
    bool IsPinching = false;

    /// <summary>  
    ///  The position of the pinch gesture
    /// </summary>  
    public Vector3 PinchingPOS;

    /// <summary>  
    ///  Allowed distance between two nodes
    /// </summary>  
    public float distanse = 1.0f;

    /// <summary>  
    ///  the max allowed distance between fingers
    /// </summary>  
    public float distanseBetweenFingers = 1.0f;

    /// <summary>  
    ///  Leap motion Service Provider
    /// </summary>  
    LeapServiceProvider provider;

    /// <summary>  
    ///  Boolean to check the mode of the pinch (pinch to create/pinch to move)
    /// </summary>  
    public bool createMode = true;


    /// <summary>  
    ///  Leap motion Service Provider initialization
    /// </summary> 
    void Start()
    {
        provider = FindObjectOfType<LeapServiceProvider>() as LeapServiceProvider;
    }


    /// <summary>  
    ///  Check every frame if pinch gesture is active
    /// </summary> 
    void Update()
    {
        checkIsPinching();
        if (!prefabCreated)
        {
            if (IsPinching)
            {
                if (canCreate())
                {
                    if (createMode)
                    {
                        createNode();
                    }
                    else
                    {
                        MoveNode();
                    }
                }

            }
        }

    }

    /// <summary>  
    /// Move the selected node to the pinch position
    /// </summary> 
    void MoveNode()
    {
        if(ND.selectedNode != null)
        {
            ND.selectedNode.transform.position = PinchingPOS;
            ND.selectedNode.GetComponent<Node>().rePostion();
            prefabCreated = true;
        }
        else
        {
            createNode();
        }

    }

    /// <summary>  
    /// create new node in the pinch position
    /// </summary> 
    void createNode()
    {
        GameObject node = Instantiate(ND.NodePrefab, PinchingPOS, ND.NodePrefab.transform.rotation);
        node.GetComponent<Node>().data.id = ND.idCounter;
        ND.idCounter++;
        if (ND.root == null)
        {
            ND.root = node;
            ND.selectNode(node);
        }
        else
        {
            ND.selectedNode.GetComponent<Node>().children.Add(node);
            //ND.selectedNode.GetComponent<Node>().resetData();
            node.GetComponent<Node>().parent = ND.selectedNode;
            ND.createLine(node);
        }
        node.transform.parent = ND.NodeContainer.transform;
        ND.Nodes.Add(node);
        DataController.Instance.data.Add(node.GetComponent<Node>().data);
        node.GetComponent<Node>().resetData();
        prefabCreated = true;
    }


    /// <summary>  
    /// check if node already exist in the pinch position
    /// </summary> 
    bool canCreate()
    {
        bool canCreate = true;
        foreach (GameObject Node in ND.Nodes)
        {
            Vector3 diff = Node.transform.position - PinchingPOS;
            if (diff.magnitude < distanse)
            {
                canCreate = false;
            }
        }
        return canCreate;
    }

    /// <summary>  
    /// Check if pinch gesture is active
    /// </summary> 
    void checkIsPinching()
    {
        Hand RightHand = null;
        Hand LeftHand = null;

        Vector3 RightHandTPOS = Vector3.zero;
        Vector3 LeftHandTPOS = Vector3.zero;
        Frame frame = provider.CurrentFrame;
        foreach (Hand hand in frame.Hands)
        {
            if (hand.IsRight)
            {
                RightHand = hand;
            }
            if (hand.IsLeft)
            {

                LeftHand = hand;
            }


        }
        if (RightHand != null && LeftHand != null)
        {
            if (RightHand.IsPinching() && LeftHand.IsPinching())
            {
                float distance = Vector3.Distance(RightHand.Fingers[0].TipPosition.ToVector3(), LeftHand.Fingers[0].TipPosition.ToVector3());
                Debug.Log(distance);
                if (distanseBetweenFingers > distance)
                    PinchingPOS = Vector3.Lerp(RightHand.Fingers[0].TipPosition.ToVector3(), LeftHand.Fingers[0].TipPosition.ToVector3(), 0.5f);
                //   PinchingPOS.x = PinchingPOS.x + 1f;
                IsPinching = true;
            }
            else
            {
                prefabCreated = false;
                IsPinching = false;
            }

        }
    }


    /// <summary>  
    /// Switch between the move and create mode
    /// </summary> 
    /// <param name="createMoveModeBtn">The Text UI for the createMoveMode button in the hand Menu</param>
    public void createMoveModeBtn(Text createMoveModeBtn)
    {
        createMode = !createMode;
        if (createMode)
        {
            createMoveModeBtn.text = "Pinch to create";
        }
        else
        {
            createMoveModeBtn.text = "Pinch to move";
        }
    }
}
