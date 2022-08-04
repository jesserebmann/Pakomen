using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pokemon", menuName = "Pokemon/Create new pokemon")]
public class PokemonBase : ScriptableObject
{
    [SerializeField] private string name;

    [TextArea] 
    [SerializeField] private string _description;
    [SerializeField] private Sprite _sprite;
    [SerializeField] private Sprite _shinySprite;
    
    [SerializeField] private PokemonType _type1;
    [SerializeField] private PokemonType _type2;
    [SerializeField] private PokemonNature _nature;
    
    public enum PokemonType
    {
        None,
        Poison,
        Normal,
        Grass,
        Fire,
        Water,
        Electric,
        Ice,
        Fighting,
        Ground,
        Flying,
        Psychic,
        Bug,
        Rock,
        Ghost,
        Dragon,
        Dark,
        Fairy,
        Steel
    }
    public enum PokemonNature
    {
        Helpful,
        Dapper,
        Witty,
        Derp,
        Odd
    }
}
