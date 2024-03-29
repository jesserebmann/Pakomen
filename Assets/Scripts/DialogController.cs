using System;
using System.Collections;
using Pakomen;
using TMPro;
using UnityEditor.Experimental;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DialogController : MonoBehaviour
{

    [SerializeField] private GameObject _dialogUI;
    [SerializeField] private TextMeshProUGUI _dialogText;
    [SerializeField] private TMP_InputField _inputField;
    [SerializeField] private AudioSource _dialogSound;
    [SerializeField] private AudioSource _dialogAudio;
    private static  DialogController _instance;
    private UnityEvent _closeEvent;
    private UnityEvent _completeEvent;
    private string[] _textQueue;
    private int _counter;
    private string _passCode;
    private bool _isBadge;
    private bool _isDialogAudio;
    private int _badgeIndex;

    private bool _dialogInProgress = false;
    
    public void Start()
    {
        _instance = this;
    }

    public void OpenBadgeDialog(string[] texts, string passcode,int badgeIndex)
    {
        _isBadge = true;
        _badgeIndex = badgeIndex;
        _passCode = passcode;
        _textQueue = texts;
        _counter = 0;
        OpenDialog(_textQueue[_counter]);
    }
    
    public void OpenBadgeDialog(string[] texts, string passcode,int badgeIndex, UnityEvent completeEvent)
    {
        _completeEvent = completeEvent;
        _isBadge = true;
        _badgeIndex = badgeIndex;
        _passCode = passcode;
        _textQueue = texts;
        _counter = 0;
        OpenDialog(_textQueue[_counter]);
    }
    
    public void OpenDialogAudio(string[] texts,AudioClip audioClip)
    {
        _textQueue = texts;
        _counter = 0;
        OpenDialog(_textQueue[_counter]);
        _dialogAudio.clip = audioClip;
        AudioManager.Instance.Pause();
        _dialogAudio.Play();
    }
    
    public void OpenDialog(string[] texts)
    {
        _textQueue = texts;
        _counter = 0;
        OpenDialog(_textQueue[_counter]);
    }
    
    public void OpenDialog(string[] texts, UnityEvent closeEvent)
    {
        _closeEvent = closeEvent;
        _textQueue = texts;
        _counter = 0;
        OpenDialog(_textQueue[_counter]);
    }

    public void OpenDialog(string text,bool isInput = false)
    {
        Debug.Log("Open Dialog");
        _dialogUI.SetActive(true);
        _dialogText.text = text;
        if (_textQueue != null)
            _counter++;
        IsOpen = true;
        _dialogSound.Play();
    }
    
    public void CloseDialog()
    {
        if (_textQueue != null && _textQueue.Length > _counter)
        {
            OpenDialog(_textQueue[_counter]);
            return;
        }

        if (_isBadge)
        {
            _inputField.gameObject.SetActive(true);
            _dialogText.gameObject.SetActive(false);
            _dialogAudio.Stop();
            return;
        }

        if (_closeEvent != null)
        {
            _closeEvent.Invoke();
        }

        _dialogUI.SetActive(false);
        _textQueue = null;
        _counter = 0;
        IsOpen = false;
        _dialogAudio.Stop();
        if (_closeEvent == null)AudioManager.Instance.Resume();
        else
            _closeEvent = null;
        
    }

    public void CheckBadgePass()
    {
        if (String.Equals(_inputField.text, _passCode, StringComparison.CurrentCultureIgnoreCase))
        {
            Debug.Log("Passed!");
            _inputField.gameObject.SetActive(false);
            _isBadge = false;
            OpenDialog("Passed!");
            _dialogText.gameObject.SetActive(true);
            BadgeManager.Instance.SetBadgeComplete(_badgeIndex);
            _badgeIndex = 0;
            if (_completeEvent == null) return;
            _completeEvent.Invoke();
            _completeEvent = null;
        }
        else
        {
            _inputField.gameObject.SetActive(false);
            _isBadge = false;
            OpenDialog("Nice try");
            _dialogText.gameObject.SetActive(true);
        }
    }

    public static DialogController Instance => _instance;
    public bool IsOpen;
    public Interactable CurrentInteractable;
}
