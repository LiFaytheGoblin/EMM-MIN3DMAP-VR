using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
    /*
     Our user dialogs have been made following the tutorial "Making a generic modal window" Part 1 by Adam Buckner
     on Unity3d: https://unity3d.com/de/learn/tutorials/modules/intermediate/live-training-archive/modal-window
     (Shared 12.01.2015, Accessed 17.12.2018)
     */

public class ModalPanel : MonoBehaviour {
    public Button vButton;
    public Button xButton;
    public Button oButton;

    public GameObject modalPanelObject;

    private static ModalPanel modalPanel;

    public void Start()
    {
        closePanel();
    }

    public static ModalPanel Instance()
    {
        /* 
         * This script makes sure that the right amount of dialogue modals are there: 1
         */
        if (!modalPanel)
        {
            modalPanel = FindObjectOfType(typeof (ModalPanel)) as ModalPanel;
            if (!modalPanel) Debug.LogError("No active Modal Panel script in the scene");
        }
        return modalPanel;
    }

    public void choice(UnityAction vEvent, UnityAction xEvent, UnityAction oEvent)
    {
        /*
         This function registers the listeners for the buttons of this user dialogue
         */
        modalPanelObject.SetActive(true); //open panel

        vButton.onClick.RemoveAllListeners();
        xButton.onClick.RemoveAllListeners();
        oButton.onClick.RemoveAllListeners();

        if (oEvent != null)
        {
            oButton.onClick.AddListener(oEvent);
            oButton.onClick.AddListener(closePanel); //if any button is clicked, the panel should be closed.
            oButton.gameObject.SetActive(true);
        }
        
        if(vEvent != null)
        {
            vButton.onClick.AddListener(vEvent);
            vButton.onClick.AddListener(closePanel); 
            vButton.gameObject.SetActive(true);
        }
        
        if(xEvent != null)
        {
            xButton.onClick.AddListener(xEvent);
            xButton.onClick.AddListener(closePanel);
            xButton.gameObject.SetActive(true);
        }
    }

    public void closePanel()
    {
        /*
         This function closes the User Dialog panel
         */
        modalPanelObject.SetActive(false);
    }
}
