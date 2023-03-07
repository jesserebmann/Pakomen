using System;
using System.Collections;
using System.Collections.Generic;
using Pakomen;
using UnityEngine;
using UnityEngine.Events;
using Random = System.Random;

public class Interactable : MonoBehaviour
{
    [SerializeField] private string[] _dialogText;
    [SerializeField] private interactableType _interactableType;
    [SerializeField] private string _badgeCode;
    [SerializeField] private AudioClip _audio;
    [SerializeField] private int _badgeIndex;
    [SerializeField] private string _pokemonPropertyName;
    [SerializeField] private PokemonBase _encounterPokemon;
    [SerializeField] private WildRegion _encounterRegion;
    [SerializeField] private UnityEvent _dialogEvent;
    [SerializeField] private UnityEvent _completeEvent;

    public enum interactableType
    {
        dialog,
        item,
        destructable,
        input,
        badge,
        encounter,
        dialogAudio,
        dialogEvent,
        dialogCloseEvent,
        badgeEvent
    }

    public void Start()
    {
        if (_encounterPokemon)
        {
            if (PlayerPrefs.GetInt(_pokemonPropertyName) == 1)
                gameObject.SetActive(false);
        }
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
            case interactableType.dialogAudio:
                if (_dialogText.Length == 0) return;
            {
                Debug.Log("Dialog Pressed");
                DialogController.Instance.OpenDialogAudio(_dialogText, _audio);
                if (_dialogEvent != null)
                    _dialogEvent.Invoke();
            }
                break;
            case interactableType.dialogEvent:
                if (_dialogText.Length == 0) return;
            {
                Debug.Log("Dialog Pressed");
                DialogController.Instance.OpenDialog(_dialogText);
            }
                _dialogEvent.Invoke();
                break;
            case interactableType.dialogCloseEvent:
                if (_dialogText.Length == 0) return;
            {
                Debug.Log("Dialog Pressed");
                DialogController.Instance.OpenDialog(_dialogText, _dialogEvent);
            }
                _dialogEvent.Invoke();
                break;
            case interactableType.item:
                break;
            case interactableType.destructable:
                if (_dialogText.Length == 0) return;
                DialogController.Instance.OpenDialog(_dialogText);
                break;
            case interactableType.badge:
                Debug.Log("Trigger BadgeDialog");
                DialogController.Instance.OpenBadgeDialog(_dialogText, _badgeCode, _badgeIndex);
                break;
            case interactableType.badgeEvent:
                Debug.Log("Trigger BadgeDialog");
                DialogController.Instance.OpenBadgeDialog(_dialogText, _badgeCode, _badgeIndex, _completeEvent);
                break;
            case interactableType.encounter:
                Encounter(_encounterPokemon, _encounterRegion);
                PlayerPrefs.SetInt(_pokemonPropertyName, 1);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void CompleteEncounter()
    {
        AudioManager.Instance.Pause();
        gameObject.SetActive(false);
        Random _random = new Random();
        UIController.Instance.SetBattleBackground(_encounterRegion.Background);
        var isShiny = _random.Next(0, 20) == 1;
        //Debug.Log(pokemonEncounter.name);
        UIController.Instance.StartEncounter(_encounterPokemon,isShiny);
        AudioManager.Instance.PlayEncounterAudio();
    }


    private void Encounter(PokemonBase pokemon,WildRegion region)
    {
        gameObject.SetActive(false);
        Random _random = new Random();
        UIController.Instance.SetBattleBackground(region.Background);
        var isShiny = _random.Next(0, 20) == 1;
        //Debug.Log(pokemonEncounter.name);
        UIController.Instance.StartEncounter(pokemon,isShiny);
        AudioManager.Instance.Pause();
        AudioManager.Instance.PlayEncounterAudio();
            
    }
    public string[] DialogText => _dialogText;
    public interactableType InteractableType => _interactableType;
}
