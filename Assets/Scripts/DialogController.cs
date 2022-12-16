using System.Collections;
using Pakomen;
using TMPro;
using UnityEditor.Experimental;
using UnityEngine;
using UnityEngine.UI;

public class DialogController : MonoBehaviour
{

    [SerializeField] private GameObject _dialogUI;
    [SerializeField] private TextMeshProUGUI _dialogText;
    private static  DialogController _instance;
    private string[] _textQueue;
    private int _counter;
    
    public void Start()
    {
        _instance = this;
    }

    public void OpenDialog(string[] texts)
    {
        _textQueue = texts;
        _counter = 0;
        OpenDialog(_textQueue[_counter]);
    }

    public void OpenDialog(string text)
    {
        _dialogUI.SetActive(true);
        _dialogText.text = text;
        if (_textQueue != null)
            _counter++;
        IsOpen = true;
    }
    
    public void CloseDialog()
    {
        if (_textQueue != null && _textQueue.Length > _counter)
        {
            OpenDialog(_textQueue[_counter]);
            return;
        }
        _dialogUI.SetActive(false);
        _textQueue = null;
        _counter = 0;
        IsOpen = false;
    }

    public static DialogController Instance => _instance;
    public bool IsOpen;
}
