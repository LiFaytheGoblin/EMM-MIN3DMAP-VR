using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>  
///  This class manages renaming a node
/// </summary>  
public class Rename : MonoBehaviour {

    public SpeechRecognition SR; //!< SpeechRecognition instance
    public NodeController ND; //!< The Node Controller in the scene
    public GameObject UIStage1; //!< The rename UI of the first massage
    public GameObject UIStage2; //!< The rename UI of the second massage
    public GameObject UIStage3; //!< The rename UI of the third massage
    public GameObject UIStageLongText; //!< The rename UI if long text recorded
    public GameObject UIStageError; //!< The rename UI if error occurred
    public Text UIStage3Text; //!< The text component for the rename UI of the third message
    public string text; //!< The node text
    bool Renaming = false; //!< Renaming is running?

    /// <summary>  
    /// Shows the Rename UI
    /// </summary> 
    public void ShowUIStage () {
        if (!Renaming && ND.selectedNode != null)
        {
            UIStage1.SetActive(true);
            Renaming = true;
        }
    }

    /// <summary>  
    /// Starts the renaming process
    /// </summary> 
    public void StartRename()
    {
        text = ND.selectedNode.GetComponent<Node>().data.text;
        UIStage1.SetActive(false);
        UIStage2.SetActive(true);
        SR.startRecognizer();
    }

    /// <summary>  
    /// Restarts the renaming process
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
    /// Gets the recorded text and switches to the correct UI
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
    /// Replaces the node text with the new text and closes the renaming window, ending the rename process
    /// </summary> 
    public void save()
    {
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
    /// close all renaming windows and stop the rename process
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
