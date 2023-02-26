using System.Collections.Generic;
using System.Linq;
using Pakomen;
using UnityEngine;
using UnityEngine.UI;

public class PokedexController : MonoBehaviour
{
    public class pokedexPokemon
    {
        public int pokemonID;
        public string pokemonName;
        public bool isCaught;
        public bool isCaughtShiny;
    }

    [SerializeField] private PokedexPokemonBlueprint _pokedexPakomenBlueprint;
    [SerializeField] private bool _initializePokedexData;
    [SerializeField] private GameObject _pokedexBase;
    [SerializeField] private Transform _pokedexBaseList;
    [SerializeField] private GameObject _pokedexShiny;
    [SerializeField] private Transform _pokedexShinyList;
    [SerializeField] private PokemonBase[] _pokemons;
    
    private Dictionary<string, PokemonBase> _pokemonDictionary;
    private Dictionary<string, PokedexPokemonBlueprint> _basePokemonList;
    private Dictionary<string, PokedexPokemonBlueprint> _shinyPokemonList;
    void Start()
    {
        Instance = this;
        _pokemons = Resources.LoadAll<PokemonBase>($"Pokemons");
        _pokemonDictionary = new Dictionary<string, PokemonBase>();
        _basePokemonList = new Dictionary<string, PokedexPokemonBlueprint>();
        _shinyPokemonList = new Dictionary<string, PokedexPokemonBlueprint>();
        if(_initializePokedexData)
            PlayerPrefs.SetInt("PokedexIsInitialized",0);
        if (PlayerPrefs.GetInt("PokedexIsInitialized") != 1)
        {
            ResetPokedex();
        }

        PopulatePokedex();
    }

    // Update is called once per frame

    public void ResetPokedex()
    {
        foreach (var pokemon in _pokemons)
        {
            var pokedexItem = new pokedexPokemon();
            pokedexItem.pokemonID = int.Parse(pokemon.name.Substring(0, 3));
            pokedexItem.pokemonName =  pokemon.PokemonName;
            pokedexItem.isCaught = false;
            pokedexItem.isCaughtShiny = false;
            string json = JsonUtility.ToJson(pokedexItem);
            PlayerPrefs.SetString($"{pokedexItem.pokemonName}",json);
        }
        PlayerPrefs.SetInt("PokedexIsInitialized",1);
    }

    public void CatchPokemon(string pokemonName, bool isShiny)
    {
        var data = PlayerPrefs.GetString($"{pokemonName}");
        var pokemonData = JsonUtility.FromJson<pokedexPokemon>(data);
        var pokemonId = pokemonData.pokemonID.ToString().PadLeft(3, '0') + $"_{pokemonData.pokemonName}";
        if (isShiny)
        {
            pokemonData.isCaughtShiny = true;
            if (_shinyPokemonList.Keys.Contains(pokemonData.pokemonName))
            {
                _shinyPokemonList[pokemonData.pokemonName].PokemonName.SetText(pokemonId);
                _shinyPokemonList[pokemonData.pokemonName].PokemonImage.color = Color.white;
            }
        }
        else
        {
            pokemonData.isCaught = true;
            if (_basePokemonList.Keys.Contains(pokemonData.pokemonName))
            {
                _basePokemonList[pokemonData.pokemonName].PokemonName.SetText(pokemonId);
                _basePokemonList[pokemonData.pokemonName].PokemonImage.color = Color.white;
            }

        }
        PlayerPrefs.SetString($"{pokemonName}",JsonUtility.ToJson(pokemonData));
    }

    public void PopulatePokedex()
    {
        foreach (var pokemon in _pokemons)
        {
            var basePok = Instantiate(_pokedexPakomenBlueprint,_pokedexBaseList);
            var shinyPok = Instantiate(_pokedexPakomenBlueprint, _pokedexShinyList);
            var data = PlayerPrefs.GetString($"{pokemon.PokemonName}");
            var pokemonData = JsonUtility.FromJson<pokedexPokemon>(data);
            if (pokemonData == null)
            {
                PlayerPrefs.SetString($"{pokemonData.pokemonName}",JsonUtility.ToJson(pokemonData));
            }
            _pokemonDictionary.Add(pokemonData.pokemonName,pokemon);
            _basePokemonList.Add(pokemonData.pokemonName,basePok);
            _shinyPokemonList.Add(pokemonData.pokemonName,shinyPok);
            basePok.PokemonImage.sprite = pokemon.DefaultSprite;
            if (!pokemonData.isCaught)
            {
                basePok.PokemonImage.color = Color.black;
                basePok.PokemonName.SetText("???");
            }
            else
            {
                basePok.PokemonImage.color = Color.white;
                basePok.PokemonName.SetText(pokemon.name);  
            }
            
            shinyPok.PokemonImage.sprite = pokemon.ShinySprite;

            if (!pokemonData.isCaughtShiny)
            {
                shinyPok.PokemonImage.color = Color.black;
                shinyPok.PokemonName.SetText("???");
            }
            else
            {
                shinyPok.PokemonImage.color = Color.white;
                shinyPok.PokemonName.SetText(pokemon.name);  
            }
        }
    }

    public Sprite GetPokemonSprite(string Name, bool isShiny)
    {
        if (isShiny)
            return _shinyPokemonList[Name].PokemonImage.sprite;
        return _basePokemonList[Name].PokemonImage.sprite;
    }

    public Dictionary<string, PokedexPokemonBlueprint> BaseList => _basePokemonList;
    public Dictionary<string, PokedexPokemonBlueprint> ShinyList => _shinyPokemonList;
    public Dictionary<string, PokemonBase> PokemonBaseList => _pokemonDictionary;
    public static PokedexController Instance  { get; private set; }
}
