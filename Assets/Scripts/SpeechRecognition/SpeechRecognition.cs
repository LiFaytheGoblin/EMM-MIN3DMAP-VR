using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;
using UnityEngine.UI;

/*
 The speech recognition was implemented with help of the article "Speech Recognition in Unity3d" by 
 Gyanendu Shekhar on  "Gyanendu Shekhar's Blog" https://gyanendushekhar.com/2016/10/11/speech-recognition-in-unity3d-windows-speech-api/ 
 (published 11.10.2016, accessed 13.12.2018)

The pling sound is "Pling Sound" by KevanGC published under Public Domain on SoundBible http://soundbible.com/1645-Pling.html
(published 12.02.2010, accessed 17.12.2018)
 */

/// <summary>  
/// The SpeechRecognition engine class provides everything necessary for
/// nodes to use speech recognition. We use dictation recognition, that means
/// we search for a word or sentence the user said. What they say is up to
/// them and does not matter. (Other recognizers might need the user to say one
/// of several specific words in order to execute commands.)
/// The recognizer can be started, stopped, sends results and errors to
/// the node that is using it.
/// </summary>  
public class SpeechRecognition : MonoBehaviour
{


    public static SpeechRecognition Instance;

    public Rename R;

    private DictationRecognizer recognizer;

    private NodeController ND;

    private AudioSource pling;

    void Awake()
    {
        recognizer = new DictationRecognizer();
        recognizer.AutoSilenceTimeoutSeconds = 5;

        //subscribe to listeners
        recognizer.DictationResult += onDictationResult;
        recognizer.DictationComplete += onDictationComplete;
        recognizer.DictationError += onDictationError;

        pling = GetComponent<AudioSource>();
    }

    /// <summary>  
    /// This function starts the recognizer.
    /// </summary> 
    public void startRecognizer()
    {
        if (recognizer.Status != SpeechSystemStatus.Running)
        {
            pling.Play(0);
            recognizer.Start();
        }
    }

    /// <summary>  
    /// This function stops the recognizer if one is running.
    /// </summary> 
    public void stopRecognizer()
    {
        if (recognizer != null && recognizer.Status == SpeechSystemStatus.Running) //Status can be "Running" (green), "Stopped" (grey), "Failed" (orange) -> Use for user feedback
        {
            recognizer.Stop();
        }
    }

    /// <summary>  
    ///  When dictation results have been received, they are sent to the node.
    /// </summary> 
    private void onDictationResult(string text, ConfidenceLevel confidence)
    {
        rename(text);
    }

    /// <summary>  
    /// The actual renaming of the node,
    /// which will change the displayed text.
    /// It does create a few prompts for users
    /// to have full control over the renaming process.
    /// </summary> 
    public void rename(string t)
    {
        stopRecognizer();
        string newText = t;
        bool textToLong = newText.Length > 30; //TODO: set the actual length we want / need
        if (textToLong)
        {
            R.LongText();
        }
        else
        {
            R.onResult(t);
        }
    }


    /// <summary>  
    ///  If an error occurs, the node is being messaged about it.
    /// </summary> 
    private void onDictationComplete(DictationCompletionCause cause)
    {
        pling.Play(0);
        if (cause != DictationCompletionCause.Complete)
            R.error();
    }

    /// <summary>  
    ///  If an error occurs, the node is being messaged about it.
    /// </summary> 
    void onDictationError(string error, int hresult)
    {
        R.error();
    }


    /// <summary>  
    /// free up resources
    /// </summary> 
    private void OnApplicationQuit()
    {
        if (recognizer != null)
        {
            stopRecognizer();
            recognizer.Dispose(); 
        }
    }
}
