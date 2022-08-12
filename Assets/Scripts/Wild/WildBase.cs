using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

namespace Pakomen
{
    

[CreateAssetMenu(fileName = "WildEncounter", menuName = "WildEncounter/Create an Encounter")]

public class WildBase : ScriptableObject
{
    public enum Rate
    {
        Common,  //100
        Uncommon, //30
        Rare, //10
        Lucky //1
    }
    [SerializeField] private string name;

    [System.Serializable]
    public class WildEncounter
    {
        [SerializeField] private PokemonBase _pokemon;
        [Tooltip("value from 1 to 100")]

        public PokemonBase Pokemon   
        {
            get { return _pokemon; }
        }
    }

    [SerializeField] public WildEncounter[] EncounterListCommon;
    [SerializeField] public WildEncounter[] EncounterListUncommon;
    [SerializeField] public WildEncounter[] EncounterListRare;
    [SerializeField] public WildEncounter[] EncounterListLucky;


}
}
