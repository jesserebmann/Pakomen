using System;
using System.Collections;
using System.Collections.Generic;
using Pakomen;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private Camera _battleCamera;
    [SerializeField] private GameObject _battleUI;
    [SerializeField] private Image _screenTransition;
    [SerializeField] private Image _pokemon;
    [SerializeField] private Rect _pokemonRect;
    [SerializeField] private TextMeshProUGUI _pokemonName;

    private bool _isTransitioning;
    private float _transitionTime;
    private float _currentFadeValue = 0f;
    private float _targetFadeValue = 1f;
    public void StartEncounter(PokemonBase pokemon,bool isShiny)
    {
        _pokemonName.text = pokemon.PokemonName;
        _pokemon.sprite = (isShiny) ? pokemon.ShinySprite : pokemon.DefaultSprite;
        //_pokemon.gameObject.transform.SetPositionAndRotation(new Vector3(_pokemon.gameObject.transform.position.x,pokemon.SpriteOffset,0),quaternion.identity);
        //_pokemon.rectTransform.SetPositionAndRotation(new Vector3(_pokemon.rectTransform.anchoredPosition.x,pokemon.SpriteOffset,0),quaternion.identity);
        //FadeScreen(true);
        //RectTransform rt = _pokemon.GetComponent<RectTransform>();
        //_pokemon.rec.anchoredPosition += offset;

        _mainCamera.gameObject.SetActive(false);
        _battleCamera.gameObject.SetActive(true);
        _battleUI.SetActive(true);
    }

    public void StopEncounter()
    {
        _mainCamera.gameObject.SetActive(true);
        _battleCamera.gameObject.SetActive(false);
        _battleUI.SetActive(false);
    }

    public IEnumerator FadeScreen(bool isFadeIn)
    {
        if (_isTransitioning) yield return null;
        
        _isTransitioning = true;
        float elapsedTime = 0;
        _currentFadeValue = Mathf.Lerp(_currentFadeValue, _targetFadeValue, Time.deltaTime);
        while (elapsedTime < _transitionTime)
        {
            _screenTransition.material.SetFloat("Transition",_currentFadeValue);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        _isTransitioning = false;
        yield return null;
    }
}
