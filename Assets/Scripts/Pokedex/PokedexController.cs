using System.Collections.Generic;
using System.Linq;
using Pakomen;
using UnityEngine;

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

    private Dictionary<string, PokedexPokemonBlueprint> basePokemonList;
    private Dictionary<string, PokedexPokemonBlueprint> shinyPokemonList;
    void Start()
    {
        Instance = this;
        basePokemonList = new Dictionary<string, PokedexPokemonBlueprint>();
        shinyPokemonList = new Dictionary<string, PokedexPokemonBlueprint>();
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
            if (shinyPokemonList.Keys.Contains(pokemonData.pokemonName))
            {
                shinyPokemonList[pokemonData.pokemonName].PokemonName.SetText(pokemonId);
                shinyPokemonList[pokemonData.pokemonName].PokemonImage.color = Color.white;
            }
        }
        else
        {
            pokemonData.isCaught = true;
            if (basePokemonList.Keys.Contains(pokemonData.pokemonName))
            {
                basePokemonList[pokemonData.pokemonName].PokemonName.SetText(pokemonId);
                basePokemonList[pokemonData.pokemonName].PokemonImage.color = Color.white;
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
            basePokemonList.Add(pokemonData.pokemonName,basePok);
            shinyPokemonList.Add(pokemonData.pokemonName,shinyPok);
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

    public static PokedexController Instance  { get; private set; }
}
