using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PokemonPartyController : MonoBehaviour
{
    public class StoredParty
    {
        public List<string> PokemonNames;
        public List<bool> IsShiny;
    }

    public static PokemonPartyController Instance;
    public PokemonPartyItem[] partyList;
    private StoredParty _storedParty;
    private int _partyCount;
    public bool _isInitialized;
    
    public void Initialize()
    {
        var data = PlayerPrefs.GetString($"storedparty");
        if (data == null || string.IsNullOrEmpty(data))
        {
            _storedParty = new StoredParty();
            _storedParty.PokemonNames = new List<string>();
            _storedParty.IsShiny = new List<bool>();
            return;
        }
        _storedParty = JsonUtility.FromJson<StoredParty>(data);
        foreach (var partyItem in _storedParty.PokemonNames)
        {
            partyList[_partyCount].PartyName.SetText(partyItem);
            partyList[_partyCount].PartySprite.sprite = PokedexController.Instance.GetPokemonSprite(partyItem,_storedParty.IsShiny[_partyCount]);
            partyList[_partyCount].IsShiny = _storedParty.IsShiny[_partyCount];
            _partyCount++;
        }

        _isInitialized = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }

    public void AddToParty(string PokemonName,bool isShiny)
    {
        if(!_isInitialized)
            Initialize();
        if (_partyCount == 6) return;
        partyList[_partyCount].PartyName.SetText(PokemonName);
        partyList[_partyCount].PartySprite.sprite = PokedexController.Instance.GetPokemonSprite(PokemonName,isShiny);
        _storedParty.PokemonNames.Add(PokemonName);
        _storedParty.IsShiny.Add(isShiny);
        _partyCount++;
        StoreParty();
    }
    
    public void AddToParty(int Position,string PokemonName,bool isShiny)
    {
        if(!_isInitialized)
            Initialize();
        partyList[Position].PartyName.SetText(PokemonName);
        partyList[Position].PartySprite.sprite = PokedexController.Instance.GetPokemonSprite(PokemonName,isShiny);
        _storedParty.PokemonNames[Position] = PokemonName;
        _storedParty.IsShiny[Position] = isShiny;
        StoreParty();
    }

    public void StoreParty()
    {
        PlayerPrefs.SetString($"storedparty",JsonUtility.ToJson(_storedParty));
    }
    
}
