using System.Collections;
using System.Collections.Generic;
using Random = System.Random;
using UnityEngine;
using UnityEngine.UI;

namespace Pakomen
{
    public class WildRegion : MonoBehaviour
    {
        public enum WildeType
        {
            Grass,
            Water,
            Cave
        }
        [SerializeField] private WildBase _regionWild;
        [SerializeField] private WildeType _wildType;
        [SerializeField] private Sprite _background;

        private Random _random = new Random();
        
        public PokemonBase GetPokemonEncounter()
        {
            int number = _random.Next(0, 100); 
            
            switch (number)
            {
                case 1:
                    //lucky
                    int luckyIndex = _random.Next(0, _regionWild.EncounterListLucky.Length);
                    return _regionWild.EncounterListLucky[luckyIndex].Pokemon;
                    break;
                case <8:
                    //Rare
                    int rareIndex = _random.Next(0, _regionWild.EncounterListRare.Length);
                    return _regionWild.EncounterListRare[rareIndex].Pokemon;
                    break;
                case <30:
                    //Uncommon
                    int uncommonIndex = _random.Next(0, _regionWild.EncounterListUncommon.Length);
                    return _regionWild.EncounterListUncommon[uncommonIndex].Pokemon;
                    break;
                default:
                    //common
                    int commonIndex = _random.Next(0, _regionWild.EncounterListCommon.Length);
                    return _regionWild.EncounterListCommon[commonIndex].Pokemon;
                    break;
            }
        }

        public WildeType WildType => _wildType;
        public Sprite Background => _background;
    }
}
