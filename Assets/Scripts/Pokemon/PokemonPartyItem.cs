using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class PokemonPartyItem : MonoBehaviour
{
    public TextMeshProUGUI PartyName;
    public Image PartySprite;
    public bool IsShiny;
}
