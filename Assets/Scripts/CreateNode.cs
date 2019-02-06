using UnityEngine;
using Leap;
using Leap.Unity;
using UnityEngine.UI;

/// <summary>  
///  This class manage create or move node with Pinch Gesture
/// </summary>  
public class CreateNode : MonoBehaviour
{
    public NodeController ND; //!< The Node Controller in the scene
    bool prefabCreated = false; //!< Boolean to check if the node was already created
    bool IsPinching = false; //!< Boolean to check if the pinch gesture is active
    public Vector3 PinchingPOS; //!< The position of the pinch gesture
    public float distanse = 1.0f; //!< Allowed distance between two nodes
    public float distanseBetweenFingers = 1.0f; //!< the max allowed distance between fingers
    LeapServiceProvider provider; //!< Leap motion Service Provider
    public bool createMode = true; //!< Boolean to check the mode of the pinch (pinch to create/pinch to move)

    void Start()
    {
        provider = FindObjectOfType<LeapServiceProvider>() as LeapServiceProvider;
    }

    /// <summary>  
    ///  Check for each new frame if pinch gesture is active
    /// </summary> 
    void Update()
    {
        checkIsPinching();
        if (!prefabCreated && IsPinching && canCreate())
        {
            if (createMode) createNode();
            else MoveNode();
        }
    }

    /// <summary>  
    ///  Moves the selected node to the pinch position
    /// </summary> 
    void MoveNode()
    {
        if(ND.selectedNode != null)
        {
            ND.selectedNode.transform.position = PinchingPOS;
            ND.selectedNode.GetComponent<Node>().rePostion();
            prefabCreated = true;
        }
        else createNode();
    }

    /// <summary>  
    ///  Creates a new node in the pinch position
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
    ///  Checks if node already exist in the pinch position
    ///  
    /// \return true if the node can be created at the desired place and false, if there is already another node there
    /// </summary> 
    bool canCreate()
    {
        bool canCreate = true;
        foreach (GameObject Node in ND.Nodes)
        {
            Vector3 diff = Node.transform.position - PinchingPOS;
            if (diff.magnitude < distanse) canCreate = false;
        }
        return canCreate;
    }

    /// <summary>  
    /// Checks if pinch gesture is active
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
            if (hand.IsRight) RightHand = hand;
            if (hand.IsLeft) LeftHand = hand;
        }
        if (RightHand != null && LeftHand != null)
        {
            if (RightHand.IsPinching() && LeftHand.IsPinching())
            {
                float distance = Vector3.Distance(RightHand.Fingers[0].TipPosition.ToVector3(), LeftHand.Fingers[0].TipPosition.ToVector3());
                if (distanseBetweenFingers > distance) PinchingPOS = Vector3.Lerp(RightHand.Fingers[0].TipPosition.ToVector3(), LeftHand.Fingers[0].TipPosition.ToVector3(), 0.5f);
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
    ///  Switches between the move and create mode
    ///  
    /// @param[in] createMoveModeBtn The Text UI for the createMoveMode button in the hand Menu
    /// </summary>
    public void createMoveModeBtn(Text createMoveModeBtn)
    {
        createMode = !createMode;
        if (createMode) createMoveModeBtn.text = "Pinch to create";
        else createMoveModeBtn.text = "Pinch to move";
    }
}
