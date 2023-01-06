using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaSound : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource; // drag the music player here or...

    void OnTriggerEnter2D(Collider2D other)
    {
        AudioManager.Instance.PlayAreaAudio(_audioSource);
    }
    
}
