using System;
using System.Collections.Generic;
using Pakomen;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PokemonInfoController : MonoBehaviour
{
    [SerializeField] private Image _pokemonThumb;
    [SerializeField] private Image[] _pokemonPartyThumbs;
    [SerializeField] private Image _type1;
    [SerializeField] private Image _type2;
    [SerializeField] private TextMeshProUGUI _nature;
    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private TextMeshProUGUI _description;
    [SerializeField] private List<Sprite> _typeSprites;

    public static PokemonInfoController Instance;
    private Dictionary<string, Sprite> _types;
    private bool _isShiny;
    
    public void Start()
    {
        Instance = this;
        _types = new Dictionary<string, Sprite>();
        foreach (var typeSprite in _typeSprites)
        {
            _types[typeSprite.name.Substring(6)] = typeSprite;
        }
    }

    public void SetInfo(string pokemonId, bool isShiny)
    {
        var name = pokemonId;
        if (pokemonId.Split('_').Length > 1)
            name = pokemonId.Split('_')[1];
        var data = PlayerPrefs.GetString($"{name}"); 
        var pokemonData = JsonUtility.FromJson<PokedexController.pokedexPokemon>(data);
        //_pokemonThumb.sprite = pokemonData.sprite
        _pokemonThumb.color = Color.white;
            var pokemonResource = PokedexController.Instance.PokemonBaseList[name];
            _pokemonThumb.sprite = isShiny ? pokemonResource.ShinySprite : pokemonResource.DefaultSprite;
            var test = _types[pokemonResource.Type1.ToString()];
            if (pokemonResource.Type2 != PokemonBase.PokemonType.None)
            {
                var test2 = _types[pokemonResource.Type2.ToString()];
                _type2.sprite = test2;
            }
            else
            {
                var test2 = _types["Normal"];
                _type2.sprite = test2;
            }
            _type1.sprite = test;
            _name.text = pokemonResource.PokemonName;
            _isShiny = isShiny;
            _description.SetText(pokemonResource.Description);
            _nature.SetText(pokemonResource.Nature.ToString());
            //_type1 = pokemonResource.Type1;
            //_pokemonThumb.sprite = (isShiny) ? pokemonData.
    }

    public void AddToParty(int partyPosition)
    {
        PokemonPartyController.Instance.AddToParty(partyPosition,_name.text,_isShiny);
        UpdatePartyItem(partyPosition);
    }

    public void SetPartyItem(int position, Sprite sprite)
    {
        _pokemonPartyThumbs[position].sprite = sprite;
    }

    public void UpdatePartyItem(int index)
    {
        _pokemonPartyThumbs[index].sprite = PokemonPartyController.Instance.partyList[index].PartySprite.sprite;
    }
    
    public void UpdatePartyItems()
    {
        for (int i = 0; i < PokemonPartyController.Instance.partyList.Length; ++i)
        {
            UpdatePartyItem(i);
        }
    }


}
