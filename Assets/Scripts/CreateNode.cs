using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using Leap.Unity;

public class CreateNode : MonoBehaviour {

    public NodeController ND;

    public GameObject prefab;
    public Material lineColor;

    public bool prefabCreated = false;
    public bool IsPinching = false;
    public Vector3 PinchingPOS;
    public float distanse = 1.0f;
    public float distanseBetweenFingers = 1.0f;

    List<GameObject> Nodes = new List<GameObject>();
    LeapServiceProvider provider;
  

    // Use this for initialization
    void Start () {
        provider = FindObjectOfType<LeapServiceProvider>() as LeapServiceProvider;
    }
	
	// Update is called once per frame
	void Update () {
        checkIsPinching();
        if (!prefabCreated)
        {
            createNode();
        }
      
    }

    void createNode()
    {
        if (IsPinching)
        {
            if (canCreate())
            {
                //Debug.Log(prefabCreated);
                GameObject node = Instantiate(prefab, PinchingPOS, prefab.transform.rotation);
                if(ND.root == null)
                {
                    ND.root = node;
                    ND.selectNode(node);
                    node.transform.parent = ND.NodeContainer.transform;
                }
                else 
                {
                    ND.selectedNode.GetComponent<Node>().children.Add(node);
                    node.GetComponent<Node>().parent = ND.selectedNode;
                    createLine(node);
                    node.transform.parent = ND.NodeContainer.transform;
                }
                Nodes.Add(node);
                prefabCreated = true;
            }
        
        }
    }

    LineRenderer createLine(GameObject newNode)
    {
        if(ND.selectedNode != null)
        {
           // GameObject go = new GameObject();
            LineRenderer line = newNode.AddComponent<LineRenderer>();
            line.startWidth = .01f;
            line.endWidth = .01f;
            line.material = lineColor ;
            line.SetPosition(0, ND.selectedNode.transform.position);
            line.SetPosition(1, newNode.transform.position);
            return line;
        }
        return null;
    }

    bool canCreate()
    {
        bool canCreate = true;
        foreach (GameObject Node in Nodes)
        {
            Vector3 diff = Node.transform.position - PinchingPOS;
            if (diff.magnitude < distanse)
            {
                canCreate = false;
            }
        }
        return canCreate;
    }


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
        if(RightHand != null && LeftHand != null)
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
}
