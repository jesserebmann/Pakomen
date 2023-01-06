using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] private string[] _dialogText;
    [SerializeField] private interactableType _interactableType;
    public enum interactableType
    {
        dialog,
        item,
        destructable,
        input
    }

    public void TriggerInteraction()
    {
        switch (InteractableType)
        {
            case interactableType.dialog:
                if (_dialogText.Length == 0) return;
                    DialogController.Instance.OpenDialog(_dialogText);
                break;
            case interactableType.item:
                break;
            case interactableType.destructable:
                if (_dialogText.Length == 0) return;
                    DialogController.Instance.OpenDialog(_dialogText);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public string[] DialogText => _dialogText;
    public interactableType InteractableType => _interactableType;
}
