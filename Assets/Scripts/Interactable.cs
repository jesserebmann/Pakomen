using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] private string _dialogText;
    [SerializeField] private interactableType _interactableType;
    public enum interactableType
    {
        dialog,
        item
    }

    public string DialogText => _dialogText;
    public interactableType InteractableType => _interactableType;
}
