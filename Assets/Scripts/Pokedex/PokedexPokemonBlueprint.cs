using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PokedexPokemonBlueprint : MonoBehaviour
{

    [SerializeField] private Image _pokemonImage;
    [SerializeField] private TextMeshProUGUI _pokemonName;

    public void OpenPokemonDetails()
    {
        UIController.Instance.TogglePokemonInfoUI(true,_pokemonName.text);
    }

    public Image PokemonImage => _pokemonImage;
    public TextMeshProUGUI PokemonName => _pokemonName;
    
    
}
