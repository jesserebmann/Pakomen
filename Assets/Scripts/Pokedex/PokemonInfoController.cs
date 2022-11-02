using System;
using System.Collections.Generic;
using Pakomen;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PokemonInfoController : MonoBehaviour
{
    [SerializeField] private Image _pokemonThumb;
    [SerializeField] private Image _pokemonParty1Thumb;
    [SerializeField] private Image _pokemonParty2Thumb;
    [SerializeField] private Image _pokemonParty3Thumb;
    [SerializeField] private Image _pokemonParty4Thumb;
    [SerializeField] private Image _pokemonParty5Thumb;
    [SerializeField] private Image _pokemonParty6Thumb;
    [SerializeField] private Image _type1;
    [SerializeField] private Image _type2;
    [SerializeField] private TextMeshProUGUI _nature;
    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private List<Sprite> _typeSprites;

    public static PokemonInfoController Instance;
    private Dictionary<string, Sprite> _types;  
    
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
        var data = PlayerPrefs.GetString($"{pokemonId.Substring(4)}"); 
        var pokemonData = JsonUtility.FromJson<PokedexController.pokedexPokemon>(data);
        //_pokemonThumb.sprite = pokemonData.sprite
        _pokemonThumb.color = Color.white;
            var pokemonResource = (PokemonBase)Resources.Load($"Pokemons/{pokemonId}");
            _pokemonThumb.sprite = isShiny ? pokemonResource.ShinySprite : pokemonResource.DefaultSprite;
            var test = _types[pokemonResource.Type1.ToString()];
            var test2 = _types[pokemonResource.Type2.ToString()];
            _name.text = pokemonResource.PokemonName;
            //_type1 = pokemonResource.Type1;
            //_pokemonThumb.sprite = (isShiny) ? pokemonData.
    }


}
