using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;


/// <summary>  
/// The NodeData class is a serializable object that
/// can hold data in a specified and serializable form.
/// This way, data that defines the Min3dmap can be
/// saved to a file and be converted back into a Min3dmap.
/// </summary> 
[Serializable]
public class NodeData{
    public int id = 0; //!< [0..n]. Unique Id for a node, 0 is the root node
    public string text = "..."; //!< Text displayed on a node
    public float xPos; //!< xPos, yPos and zPos hold the Positions
    public float yPos;
    public float zPos;
    public int parentId = -1; //!< [-1..n-1]. the Id of the parent node, -1 if there is no parent
}
