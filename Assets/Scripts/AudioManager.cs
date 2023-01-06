using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    private AudioSource _currentAudioSource;

    #region Methods

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

    public void PlayAreaAudio(AudioSource audio)
    {
        if(_currentAudioSource != null)_currentAudioSource.Stop();
        audio.Play();
        _currentAudioSource = audio;
    }

    #endregion Methods
    
    

    #region Properties

    public static AudioManager Instance { get; private set; }

    #endregion Properties
}
