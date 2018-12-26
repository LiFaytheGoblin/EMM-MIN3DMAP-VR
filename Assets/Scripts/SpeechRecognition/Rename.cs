using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rename : MonoBehaviour {

    public SpeechRecognition SR;
    public NodeController ND;

    public GameObject UIStage1;
    public GameObject UIStage2;
    public GameObject UIStage3;
    public GameObject UIStageLongText;
    public GameObject UIStageError;
    public Text UIStage3Text;
    public string text;

    bool Renaming = false;

    public void ShowUIStage () {
        if (!Renaming && ND.selectedNode != null)
        {
            UIStage1.SetActive(true);
            Renaming = true;
        }
    }

    public void StartRename()
    {
        text = ND.selectedNode.GetComponent<Node>().data.text;
        UIStage1.SetActive(false);
        UIStage2.SetActive(true);
        SR.startRecognizer();
    }

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

    public void onResult(string newText)
    {
        UIStage1.SetActive(false);
        UIStage2.SetActive(false);
        UIStage3.SetActive(true);
        UIStageLongText.SetActive(false);
        UIStageError.SetActive(false);
        UIStage3Text.text = "The New Name is (" + newText + ") " +
            "press restart to change the Name";
        text = newText;
    }

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

    public void LongText()
    {
        UIStage1.SetActive(false);
        UIStage2.SetActive(false);
        UIStage3.SetActive(false);
        UIStageLongText.SetActive(true);
        UIStageError.SetActive(false);
    }

    public void error()
    {
        UIStage1.SetActive(false);
        UIStage2.SetActive(false);
        UIStage3.SetActive(false);
        UIStageLongText.SetActive(false);
        UIStageError.SetActive(true);
    }

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
