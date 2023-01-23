using System.Collections;
using System.Collections.Generic;
using Pakomen;
using UnityEngine;

public class EncounterManager : MonoBehaviour
{
    private PokemonBase _currentEncounter;
    [SerializeField] private int step = 10;
    [SerializeField] private int _catchRate = 60;
    [SerializeField] private int _runRate = 40;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public void SetEncounter(PokemonBase pokemonbase)
    {
        _catchRate = 60;
        _currentEncounter = pokemonbase;
    }

    public void Throw()
    {
        _catchRate += step;
        BattleController.Instance.Throw();
    }
    public bool TryCatching()
    {
        int randomNumber = Random.Range(0, 100);
        if (randomNumber <= _catchRate)
            return true;
        return false;
    }

    public bool CheckIfRun()
    {
        int randomNumber = Random.Range(0, 100);
        return randomNumber <= _runRate;
    }

    public void Ran()
    {
    }


    #region Properties

    public static EncounterManager Instance { get; private set; }

    #endregion Properties
    
    
}
