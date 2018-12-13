using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;
using UnityEngine.UI;

/*
 The speech recognition was implemented with help of the article "Speech Recognition in Unity3d" by 
 Gyanendu Shekhar on  "Gyanendu Shekhar's Blog" https://gyanendushekhar.com/2016/10/11/speech-recognition-in-unity3d-windows-speech-api/ 
 (published 11.10.2016, accessed 13.12.2018)
 */

public class SpeechRecognition : MonoBehaviour {
    /*
     * The SpeechRecognition engine class provides everything necessary for
     * nodes to use speech recognition. We use dictation recognition, that means
     * we search for a word or sentence the user said. What they say is up to
     * them and does not matter. (Other recognizers might need the user to say one
     * of several specific words in order to execute commands.)
     * The recognizer can be started, stopped, sends results and errors to
     * the node that is using it.
     */

    public static SpeechRecognition Instance;

    private DictationRecognizer recognizer;
    private Node node;

    void Awake () {
        node = this.gameObject.GetComponent<Node>();

        recognizer = new DictationRecognizer();
        recognizer.AutoSilenceTimeoutSeconds = 5;

        //subscribe to listeners
        recognizer.DictationResult += onDictationResult;
        recognizer.DictationComplete += onDictationComplete;
        recognizer.DictationError += onDictationError;
    }

    public void startRecognizer()
    {
        /*
         This function starts the recognizer.
         */
        recognizer.Start();
    }

    public void stopRecognizer()
    {
        /*
         This function stops the recognizer if one is running.
         */
        if (recognizer != null  && recognizer.Status == SpeechSystemStatus.Running) //Status can be "Running" (green), "Stopped" (grey), "Failed" (orange) -> Use for user feedback
        {
            recognizer.Stop();
        }
    }

    private void onDictationResult(string text, ConfidenceLevel confidence)
    {
        /*
         When dictation results have been received, they are sent to the node
         */
        Debug.Log(text);
        node.send(text);
        // error handling?
    }

    private void onDictationComplete(DictationCompletionCause cause)
    {
        /*
         */
        if (cause != DictationCompletionCause.Complete)
            Debug.LogErrorFormat("Dictation completed unsuccessfully: {0}.", cause);
    }

    void onDictationError(string error, int hresult)
    {
        /*
         */
        Debug.Log(error);
    }


    private void OnApplicationQuit()
    {
        if (recognizer != null)
        {
            stopRecognizer();
            recognizer.Dispose(); //free up resources
        }
    }
}
