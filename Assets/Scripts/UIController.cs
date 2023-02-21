using System;
using System.Collections;
using Pakomen;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private Camera _battleCamera;
    [SerializeField] private GameObject _battleUI;
    [SerializeField] private GameObject _pokedexUI;
    [SerializeField] private GameObject _pokemonInfoUI;
    [SerializeField] private GameObject _settingsUI;
    [SerializeField] private GameObject _pokemonPartyUI;
    [SerializeField] private GameObject _pokedexUIBase;
    [SerializeField] private GameObject _pokedexUIShiny;
    [SerializeField] private Image _screenTransition;
    [SerializeField] private Image _pokemon;
    [SerializeField] private SpriteRenderer _battleBG;
    [SerializeField] private Material _transition;
    [SerializeField] private TextMeshProUGUI _pokemonName;
    [SerializeField] private Animator _shiny;
    [SerializeField] private Button _catchButton;
    [SerializeField] private Button _runButton;
    private bool _isTransitioning;
    [SerializeField] private float _transitionTime = 1f;
    [SerializeField] private float _pokeloadTime = 1.5f;
    private float _currentFadeValue = 0f;
    private float _targetFadeValue = 0.8f;

    private bool _inEncounter;
    private bool _isShinyEncounter;
    public static UIController Instance;

    private string _lastOpenWindow;
    private PokemonBase _currentPokemon;

    public void Start()
    {
        Instance = this;
        _transition.SetFloat("_Transition",0 );
    }



    public void StartEncounter(PokemonBase pokemon,bool isShiny)
    {
        SetBattleButtons(false);
        _currentPokemon = pokemon;
        _isShinyEncounter = isShiny;
        _inEncounter = true;
        _pokemonName.text = pokemon.PokemonName;
        _pokemon.sprite = (isShiny) ? pokemon.ShinySprite : pokemon.DefaultSprite;
        _pokemon.rectTransform.localPosition = new Vector3(_pokemon.rectTransform.localPosition.x, pokemon.SpriteOffset, _pokemon.rectTransform.localPosition.z);
        _pokemon.rectTransform.ForceUpdateRectTransforms();
        StartCoroutine(FadeScreenIn());
    }

    public void StopEncounter()
    {
        _transition.SetFloat("_Transition",0f );
        _mainCamera.gameObject.SetActive(true);
        _battleCamera.gameObject.SetActive(false);
        _battleUI.SetActive(false);
        _inEncounter = false;
        AudioManager.Instance.StopEncounterAudio();
        AudioManager.Instance.Resume();
        _currentPokemon = null;
    }

    public void ThrowAtPokemon()
    {
        EncounterManager.Instance.Throw();
    }

    public void HandleCatchCompleted()
    {
        if (EncounterManager.Instance.TryCatching())
        {
            SetBattleButtons(false);
            BattleController.Instance.PlayCaught();
            PokedexController.Instance.CatchPokemon(_pokemonName.text,_isShinyEncounter);
            PokemonPartyController.Instance.AddToParty(_pokemonName.text,_isShinyEncounter);
            Debug.Log("Catched it");
            return;
        }

        if (EncounterManager.Instance.CheckIfRun())
        {
            BattleController.Instance.PlayFled();
            return;
        }
        BattleController.Instance.Escaped();
    }

    public void SetBattleBackground(Sprite sprite)
    {
        _battleBG.sprite = sprite;
    }

    public void CatchPokemon()
    {
        BattleController.Instance.Catch();
    }

    public IEnumerator FadeScreenIn()
    {
        //if (_isTransitioning) yield return null;
        _currentFadeValue = 0f;
        _isTransitioning = true;
        float elapsedTime = 0;
        while (elapsedTime < _transitionTime)
        {
            _currentFadeValue = Mathf.Lerp(_currentFadeValue, _targetFadeValue, Time.deltaTime);
            _transition.SetFloat("_Transition",_currentFadeValue );
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        _isTransitioning = false;
        yield return null;
        _mainCamera.gameObject.SetActive(false);
        _battleCamera.gameObject.SetActive(true);
        _battleUI.SetActive(true);
        SetBattleButtons(false);
        StartCoroutine(FadeScreenOut());
    }
    
    public IEnumerator FadeScreenOut()
    {
        if (_isShinyEncounter) 
            PlayShiny();
        _currentFadeValue = 1f;
        float elapsedTime = 0;
        while (elapsedTime < _transitionTime)
        {
            _currentFadeValue = Mathf.Lerp(_currentFadeValue, 0, Time.deltaTime);
            _transition.SetFloat("_Transition",_currentFadeValue );
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        SetBattleButtons(true);
        AudioManager.Instance.PlayPokemonSounds(_currentPokemon.PokeNumber1.ToString(),_currentPokemon.PokeNumber2.ToString());
        yield return null;
    }

    public void OpenPokedex()
    {
        _pokedexUI.SetActive(true);
        _pokedexUIBase.SetActive(true);
        _pokedexUIShiny.SetActive(false);
        _pokemonInfoUI.SetActive(false);
        _pokemonPartyUI.SetActive(false);
    }
    
    public void OpenSettings()
    {
        _pokedexUI.SetActive(false);
        _pokedexUIBase.SetActive(false);
        _pokedexUIShiny.SetActive(false);
        _pokemonInfoUI.SetActive(false);
        _pokemonPartyUI.SetActive(false);
        _settingsUI.SetActive(true);
    }
    
    public void OpenPokemonParty()
    {
        if(!PokemonPartyController.Instance._isInitialized)
            PokemonPartyController.Instance.Initialize();
        _pokedexUI.SetActive(false);
        _pokedexUIBase.SetActive(false);
        _pokedexUIShiny.SetActive(false);
        _pokemonInfoUI.SetActive(false);
        _pokemonPartyUI.SetActive(true);
    }
    
    public void OpenPokedexShiny()
    {
        _pokedexUIBase.SetActive(false);
        _pokedexUIShiny.SetActive(true);
    }
    public void ClosePokedex()
    {
        _pokedexUI.SetActive(false);
        _pokedexUIBase.SetActive(false);
        _pokedexUIShiny.SetActive(false);
        _lastOpenWindow = string.Empty;

    }

    public void SetBattleButtons(bool isActive)
    {
        _runButton.interactable = isActive;
        _catchButton.interactable = isActive;
    }

    public void PlayShiny()
    {
        _shiny.SetTrigger("PlayShiny");
        AudioManager.Instance.PlayEncounterShinyAudio();
    }

    public void ClosePokemonInfoUI()
    {
        _pokemonInfoUI.SetActive(false);
        _pokedexUI.SetActive(false);
        _lastOpenWindow = string.Empty;
    }
    
    public void ClosePokemonPartyUI()
    {
        _lastOpenWindow = "PokemonParty";
        _pokemonPartyUI.SetActive(false);
    }
    
    public void BackPokemonInfoUI()
    {
        if (_lastOpenWindow == "PokemonParty")
            _pokemonPartyUI.SetActive(true);
        else
            _pokedexUI.SetActive(true);
        
        _pokemonInfoUI.SetActive(false);
        
    }
    
    
    public void TogglePokemonInfoUI(bool isOpen,string pokemonName,bool isShiny = false)
    {
        if(!PokemonPartyController.Instance._isInitialized)
            PokemonPartyController.Instance.Initialize();
        if (pokemonName == "???") return;
        PokemonInfoController.Instance.UpdatePartyItems();
        if (isOpen)
            PokemonInfoController.Instance.SetInfo(pokemonName,_pokedexUIShiny.activeInHierarchy ? true: isShiny);
        _pokemonInfoUI.SetActive(isOpen);
        _pokedexUI.SetActive(!isOpen);
    }

    public void CloseCurrent(GameObject current)
    {
        current.SetActive(false);
        _lastOpenWindow = string.Empty;
    }

    #region Events

    public void HandleOnATap()
    {
        if (DialogController.Instance.IsOpen)
        {
            DialogController.Instance.CloseDialog();
            return;
        }
    }
    
    public void HandleResetPlayer(int index)
    {
        ResetController.Instance.ResetPLayer(index);
    }

    #endregion Events

    public bool InEncounter => _inEncounter;
}
