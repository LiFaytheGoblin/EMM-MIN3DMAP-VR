using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using Leap.Unity;

public class CreateNode : MonoBehaviour {

    public NodeController ND;

    public bool prefabCreated = false;
    public bool IsPinching = false;
    public Vector3 PinchingPOS;
    public float distanse = 1.0f;
    public float distanseBetweenFingers = 1.0f;

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
        
        }
    }



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
