using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;
using UnityEngine.UI;

/// <summary>  
/// The SpeechRecognition engine class provides everything necessary for
/// nodes to use speech recognition. We use dictation recognition, that means
/// we search for a word or sentence the user said. What they say is up to
/// them and does not matter. (Other recognizers might need the user to say one
/// of several specific words in order to execute commands.)
/// The recognizer can be started, stopped, sends results and errors to
/// the node that is using it.
/// 
///  The speech recognition was implemented with help of the article "Speech Recognition in Unity3d" by 
///  Gyanendu Shekhar on  "Gyanendu Shekhar's Blog" https://gyanendushekhar.com/2016/10/11/speech-recognition-in-unity3d-windows-speech-api/ 
///  (published 11.10.2016, accessed 13.12.2018)
///  
///  The pling sound is "Pling Sound" by KevanGC published under Public Domain on SoundBible http://soundbible.com/1645-Pling.html
///  (published 12.02.2010, accessed 17.12.2018)
/// </summary>  
public class SpeechRecognition : MonoBehaviour
{
    public static SpeechRecognition Instance; 
    public Rename R; //!< Rename manager
    private DictationRecognizer recognizer; //!< The speech recognizer
    private NodeController ND; //!< The node controller
    private AudioSource pling; //!< The audio played when recording starts and ends

    void Awake()
    {
        recognizer = new DictationRecognizer();
        recognizer.AutoSilenceTimeoutSeconds = 5;
        
        recognizer.DictationResult += onDictationResult;
        recognizer.DictationComplete += onDictationComplete;
        recognizer.DictationError += onDictationError;

        pling = GetComponent<AudioSource>();
    }

    /// <summary>  
    /// Starts the recognizer.
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
    ///  Stops the recognizer if one is running.
    /// </summary> 
    public void stopRecognizer()
    {
        if (recognizer != null && recognizer.Status == SpeechSystemStatus.Running)
        {
            recognizer.Stop();
        }
    }

    /// <summary>  
    ///  When dictation results have been received, they are used for the rename process.
    ///  
    /// @param[in]  text        the text that was interpreted from what the user said
    /// @param[in]  confidence  how confident the recognizer is that the user actually said that
    /// </summary> 
    private void onDictationResult(string text, ConfidenceLevel confidence)
    {
        rename(text);
    }

    /// <summary>  
    /// The actual renaming of the node,
    /// which will change the displayed text.
    /// It does create a few prompts for users
    /// so they have full control over the renaming process.
    /// 
    /// @param[in]  t   the text that should be used to rename the node
    /// </summary> 
    public void rename(string t)
    {
        stopRecognizer();
        string newText = t;
        bool textToLong = newText.Length > 30;
        if (textToLong) R.LongText();
        else R.onResult(t);
    }


    /// <summary>  
    ///  If an error occurs, the node is being messaged about it.
    ///  
    /// @param[in]  cause   the cause for the error
    /// </summary> 
    private void onDictationComplete(DictationCompletionCause cause)
    {
        pling.Play(0);
        if (cause != DictationCompletionCause.Complete) R.error();
    }

    /// <summary>  
    ///  If an error occurs, the node is being messaged about it.
    ///  
    /// @param[in]  error   the error that occurred
    /// @param[in]  hresult more information for the error message
    /// </summary> 
    void onDictationError(string error, int hresult)
    {
        R.error();
    }


    /// <summary>  
    ///  Frees up resources
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
