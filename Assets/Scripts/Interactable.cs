using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] private string[] _dialogText;
    [SerializeField] private interactableType _interactableType;
    [SerializeField] private string _badgeCode;
    [SerializeField] private int _badgeIndex;
    public enum interactableType
    {
        dialog,
        item,
        destructable,
        input,
        badge
    }

    public void TriggerInteraction()
    {
        Debug.Log("Trigger action");

        switch (InteractableType)
        {
            case interactableType.dialog:
                if (_dialogText.Length == 0) return;
            {
                Debug.Log("Dialog Pressed");
                DialogController.Instance.OpenDialog(_dialogText);
            }
                break;
            case interactableType.item:
                break;
            case interactableType.destructable:
                if (_dialogText.Length == 0) return;
                    DialogController.Instance.OpenDialog(_dialogText);
                break;
            case interactableType.badge:
                Debug.Log("Trigger BadgeDialog");
                DialogController.Instance.OpenBadgeDialog(_dialogText,_badgeCode,_badgeIndex);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public string[] DialogText => _dialogText;
    public interactableType InteractableType => _interactableType;
}
