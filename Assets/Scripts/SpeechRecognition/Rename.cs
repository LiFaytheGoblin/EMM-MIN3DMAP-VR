using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>  
///  This class manage rename node 
/// </summary>  
public class Rename : MonoBehaviour {

    /// <summary>  
    /// SpeechRecognition instance
    /// </summary> 
    public SpeechRecognition SR;

    /// <summary>  
    ///  The Node Controller in the scene
    /// </summary>  
    public NodeController ND;

    /// <summary>  
    ///  The rename UI the first massage
    /// </summary>  
    public GameObject UIStage1;

    /// <summary>  
    ///  The rename UI the second massage
    /// </summary>  
    public GameObject UIStage2;

    /// <summary>  
    ///  The rename UI the third massage
    /// </summary>  
    public GameObject UIStage3;

    /// <summary>  
    ///  The rename UI if long text recorded
    /// </summary>  
    public GameObject UIStageLongText;

    /// <summary>  
    ///  The rename UI if error occurred
    /// </summary>  
    public GameObject UIStageError;

    /// <summary>  
    /// The rename UI the third message text component
    /// </summary>  
    public Text UIStage3Text;

    /// <summary>  
    /// The node text
    /// </summary>  
    public string text;

    /// <summary>  
    /// Renaming is running
    /// </summary>  
    bool Renaming = false;


    /// <summary>  
    /// Show Rename UI
    /// </summary> 
    public void ShowUIStage () {
        if (!Renaming && ND.selectedNode != null)
        {
            UIStage1.SetActive(true);
            Renaming = true;
        }
    }

    /// <summary>  
    /// Start Rename 
    /// </summary> 
    public void StartRename()
    {
        text = ND.selectedNode.GetComponent<Node>().data.text;
        UIStage1.SetActive(false);
        UIStage2.SetActive(true);
        SR.startRecognizer();
    }

    /// <summary>  
    /// Restart Rename
    /// </summary> 
    public void restartRename()
    {
        SR.stopRecognizer();
        UIStage1.SetActive(false);
        UIStage3.SetActive(false);
        UIStageLongText.SetActive(false);
        UIStageError.SetActive(false);
        UIStage2.SetActive(true);
        SR.startRecognizer();
    }

    /// <summary>  
    /// Get the recorded text
    /// </summary> 
    public void onResult(string newText)
    {
        UIStage1.SetActive(false);
        UIStage2.SetActive(false);
        UIStage3.SetActive(true);
        UIStageLongText.SetActive(false);
        UIStageError.SetActive(false);
        UIStage3Text.text = "The new name is (" + newText + "), " +
            "press restart to change the name.";
        text = newText;
    }

    /// <summary>  
    /// Change the node text with the new text and close renaming window
    /// </summary> 
    public void save()
    {
        Debug.Log("Save");
        UIStage1.SetActive(false);
        UIStage2.SetActive(false);
        UIStage3.SetActive(false);
        UIStageLongText.SetActive(false);
        UIStageError.SetActive(false);
        ND.selectedNode.GetComponent<Node>().data.text = text;
        ND.selectedNode.GetComponent<Node>().UIText.text = text;
        Renaming = false;
    }

    /// <summary>  
    /// if long text detected show the long text UI
    /// </summary> 
    public void LongText()
    {
        UIStage1.SetActive(false);
        UIStage2.SetActive(false);
        UIStage3.SetActive(false);
        UIStageLongText.SetActive(true);
        UIStageError.SetActive(false);
    }

    /// <summary>  
    /// if error detected show the error UI
    /// </summary> 
    public void error()
    {
        UIStage1.SetActive(false);
        UIStage2.SetActive(false);
        UIStage3.SetActive(false);
        UIStageLongText.SetActive(false);
        UIStageError.SetActive(true);
    }

    /// <summary>  
    /// close all renaming windows 
    /// </summary> 
    public void close()
    {
        UIStage1.SetActive(false);
        UIStage2.SetActive(false);
        UIStage3.SetActive(false);
        UIStageLongText.SetActive(false);
        UIStageError.SetActive(false);
        SR.stopRecognizer();
        Renaming = false ;
    }

}
