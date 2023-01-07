using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaSound : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource; // drag the music player here or...

    public AudioSource AudioSource => _audioSource;
}
