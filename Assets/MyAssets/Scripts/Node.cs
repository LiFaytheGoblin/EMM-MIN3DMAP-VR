using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour {
    /*
     The Node object will manage the data saving to and received from
     the global DataController object. Managing includes converting
     data to the right format to save and setting local variables
     of this object to the correct data. This object also initiates 
     the saving and loading processes in the DataController.
     */

    public int id;
    public string text;
    //public Node parent;
    //public List<Node> children;

    // Use this for initialization
    void Start () {
        loadData();
    }
	
	void Update () {
		
	}

    void initSaving()
    {
        /*
         This function will initiate the copying of local data to the DataController
         and then initiate the saving process in DataContoller.
         */
        updateDataControllerData();
        DataController.Instance.Save();
    }

    void updateDataControllerData()
    {
        /*
         This function updates the data in DataController.
         */
        DataController.Instance.data.id = id;
        DataController.Instance.data.text = text;
        DataController.Instance.data.xPos = transform.position.x;
        DataController.Instance.data.yPos = transform.position.y;
        DataController.Instance.data.zPos = transform.position.z;
    }

    void loadData()
    {
        /*
         This function initiates the loading process by first
         starting the loading process in the DataController and then
         loading the received data into the Node object's property variables.
         */
        updateLocalData(DataController.Instance.Load()); //feeding the function as a param will hopefully make loadData wait for the results.
    }

    void updateLocalData(NodeData d)
    {
        /*
         This function loads the received data into the Node object's property variables.
         */
        if (DataController.Instance.loading)
        {
            transform.position = new Vector3(
                d.xPos,
                d.yPos,
                d.zPos);

            DataController.Instance.loading = false;
        }
    }
}
