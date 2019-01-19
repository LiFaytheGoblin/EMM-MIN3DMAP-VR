using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

// Save and Load implemented following the tutorial on 
// https://www.sitepoint.com/saving-and-loading-player-game-data-in-unity/?fbclid=IwAR1eXRSoYcsvvfLzSFCiD8J_nX1_pKqhL3Z_DEvdeThe7VCdPOa74Ez9Uog
// https://github.com/Eudaimonium/SitepointExample_SavingData/tree/57e166c65e0d435db7fbc7d79bab078b829b402e
// "Saving and Loading Player Game Data in Unity" by Zdravko Jakupec, 21.10.2015
// Accessed 08.12.2018
// Save and Load is not only present in this class (DataController),
// but also in Map and NodeData.


/// <summary>  
///  The DataController functions as a global object
///  that persists over scenes.It's responsability
///  is to save the Min3dmap to and from a file.
/// </summary> 
public class DataController : MonoBehaviour
{

    public static DataController Instance;
    public List<NodeData> data = new List<NodeData>();
    public bool loading = false;

    void Awake()
    {
        Application.targetFrameRate = 144;

        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>  
    ///  This function will save the Min3dmap to a file "save.binary".
    /// </summary> 
    public void Save()
    {
        if (!Directory.Exists("Saves"))
            Directory.CreateDirectory("Saves");

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream saveFile = File.Create("Saves/save.binary");

        //data = Map.Instance.data;

        formatter.Serialize(saveFile, data);

        saveFile.Close();
    }

    /// <summary>  
    /// This function will load the Min3dmap from the file "save.binary".
    /// </summary> 
    public List<NodeData> Load()
    {
        if (File.Exists("Saves/save.binary"))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream saveFile = File.Open("Saves/save.binary", FileMode.Open);

            data = (List<NodeData>)formatter.Deserialize(saveFile);

            saveFile.Close();

        loading = true;
        return data;
        }
        else
        {
            return null;
        }
    }
}
