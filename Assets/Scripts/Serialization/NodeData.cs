using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

[Serializable]
public class NodeData{
    /*
     The NodeData class is a serializable object that
     can hold data in a specified and serializable form.
     This way, data that defines the Min3dmap can be
     saved to a file and be converted back into a Min3dmap.
     */
    public int id = 0;
    public string text = "Unity";
    public float xPos;
    public float yPos;
    public float zPos;
    public int parentId = -1;
    //public List<int> childrenIds = new List<int>();
}
