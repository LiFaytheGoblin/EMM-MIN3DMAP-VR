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
    public bool listeningForName;

    // Use this for initialization
    void Start () {
        recorder = this.gameObject.GetComponent<SpeechRecognition>();
        listeningForName = false;
    }
	
	// Update is called once per frame
	void Update () {
        //for testing functions:
        if (Input.GetKeyDown("space"))
        {
            initRename();
        }
    }

    /* RENAME */
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
        listeningForName = false;
        bool textToLong = false;
        if(textToLong)
        {
            //Prompt: "Text too long!" O | X
            String answer = "retry";
            if (answer == "retry")
            {
                initRename();
            }
            else
            {
                exitRenameView();
                return;
            }
        }
        // Prompt: "your text" X | O | V
        string answer = "retry";
        if (answer = "ok")
        {
            text = t;
            stopRecognizer();
        }
        else if (answer = "retry")
        {
            initRename();
        } else {
            exitRenameView();
        }
    }

    public void send(string t)
    {
        /* Using this function others can send text to
         * this node. The node reacts depending on what it's
         * current state is. States can be:
         * listeningForName -> call rename
         */
        if (node.listeningForName)
        {
            rename(t);
        }
    }

    void goToPresentationMode()
    {
        /* Switch to the presentation mode:
         * position user directly in front of node (on same same height, x, ~1 meter y)
         */
        presentationMode = true;
    }

    void leavePresentationMode()
    {
        /* Leave the presentation mode:
         * transport user back to where they were
         */
        presentationMode = false;
    }

    void showRenameView()
    {
        /* Change look of the node that fits the renaming process.
         */
        goToPresentationMode;
        // show microphone symbol
    }

    void exitRenameView()
    {
        /* Change look back from rename view.
         */
        leavePresentationMode;
    }
}
