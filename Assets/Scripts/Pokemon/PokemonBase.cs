using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pakomen
{
    

[CreateAssetMenu(fileName = "Pokemon", menuName = "Pokemon/Create new pokemon")]
public class PokemonBase : ScriptableObject
{
    [SerializeField] private string _name;

    [TextArea] 
    [SerializeField] private string _description;
    [SerializeField] private Sprite _sprite;
    [SerializeField] private Sprite _shinySprite;
    [SerializeField] private int _pokenumber1;
    [SerializeField] private int _pokenumber2;
    [SerializeField] private float _SpriteOffset;
    
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

    public string PokemonName => _name;
    public Sprite DefaultSprite => _sprite;
    public Sprite ShinySprite => _shinySprite;
    public float SpriteOffset => _SpriteOffset;
    public string Description => _description;
    public PokemonType Type1 => _type1;
    public PokemonType Type2 => _type2;
    public int PokeNumber1 => _pokenumber1;
    public int PokeNumber2 => _pokenumber2;
    public PokemonNature Nature => _nature;
}
}
