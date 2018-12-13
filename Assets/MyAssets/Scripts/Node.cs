using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;
using UnityEngine.UI;

public class Node : MonoBehaviour {

    public int id;
    public string text;
    public int parentId;
    public List<int> childrenIds;

    private SpeechRecognition recorder;
    public bool listeningForName; //helper to know when we give responsability to recorder

    // Use this for initialization
    void Start () {
        recorder = this.gameObject.GetComponent<SpeechRecognition>();
        listeningForName = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown("space")) initRename(); //TODO: remove after testing
    }

    void goToPresentationMode()
    {
        /* Switch to the presentation mode:
         * position user directly in front of node (on same same height, x, ~1 meter y)
         */
    }

    void leavePresentationMode()
    {
        /* Leave the presentation mode:
         * transport user back to where they were
         */
    }

    public void send(string t)
    {
        /* Using this function others can send text to
         * this node. The node reacts depending on what it's
         * current state is. States can be:
         * listeningForName -> call rename
         */
        if (listeningForName)
        {
            listeningForName = false;
            rename(t);
        }
    }

    public void error()
    {
        /* Using this function others can send errors to
         * this node. The node reacts depending on what it's
         * current state is. States can be:
         * listeningForName -> call handleRenameError
         */
        if (listeningForName)
        {
            listeningForName = false;
            handleRenameError("Sorry, that didn't work.");
        }
    }

    /* R E N A M E */
    public void initRename()
    {
        /* 
         * Inits the renaming process, that is, 
         * it switches to the right look
         * and starts the speech recognition.
         */
        if (!listeningForName) {
            showRenameView();
            listeningForName = true;
            recorder.startRecognizer();
        }
    }

    private void rename(string t)
    {
        /*
         * The actual renaming of the node,
         * which will change the displayed text.
         * It does create a few prompts for users 
         * to have full control over the renaming process.
         */
        bool textToLong = false;
        if (textToLong) handleRenameError("Text too long!");
        else
        {
            // TODO: createPrompt(t, options = ["abort", "retry", "ok"])
            string answer2 = "ok";
            if (answer2 == "ok")
            {
                text = t;
                recorder.stopRecognizer();
            }
            else if (answer2 == "retry")
            {
                recorder.stopRecognizer();
                initRename();
            }
            else exitRenameView();
        }
    }

    private void handleRenameError(string message)
    {
        /*
         * If something went wrong with renaming, for instance if the text was too long or 
         * speech recognition didn't work, display a text and a user dialog.
         */
        // TODO: createPrompt(message, options = ["retry", "abort"])
        string answer1 = "retry";
        if (answer1 == "retry")
        {
            recorder.stopRecognizer();
            initRename();
        }
        else exitRenameView();
    }
    
    void showRenameView()
    {
        /* Change look of the node that fits the renaming process.
         */
        goToPresentationMode();
        // show microphone symbol
    }

    void exitRenameView()
    {
        /* Change look back from rename view.
         */
        recorder.stopRecognizer();
        leavePresentationMode();
    }

    /* E N D   O F    R E N A M E */
}
