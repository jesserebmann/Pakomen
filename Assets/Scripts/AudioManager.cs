using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    private AudioSource _currentAudioSource;
    private AreaSound _currentAreaSound;

    [SerializeField] private AudioSource _encounterAudioSource;
    [SerializeField] private AudioSource _shinyAudioSource;

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

    public void PlayEncounterAudio()
    {
        _encounterAudioSource.Play();
    }
    
    public void PlayEncounterShinyAudio()
    {
        _shinyAudioSource.Play();
    }

    public void StopEncounterAudio()
    {
        _encounterAudioSource.Stop();
    }

    public void PlayAreaAudio(AudioSource audio)
    {
        if(_currentAudioSource != null)_currentAudioSource.Stop();
        audio.Play();
        _currentAudioSource = audio;
    }

    public void UpdateAreaAudio(AreaSound audio)
    {
        if (_currentAreaSound is not null && _currentAreaSound == audio) return;
        _currentAreaSound = audio;
        PlayAreaAudio(audio.AudioSource);
    }

    public void Pause()
    {
        _currentAudioSource.Pause();
    }

    public void Resume()
    {
        _currentAudioSource.Play();
    }

    #endregion Methods
    
    

    #region Properties

    public static AudioManager Instance { get; private set; }
    public AreaSound Type { get; set; }

    #endregion Properties
}
