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
    [SerializeField] private AudioSource _pokemonSound01;
    [SerializeField] private AudioSource _pokemonSound02;
    [SerializeField] private AudioSource _wallBump;

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

    public void PlayWallBump()
    {
        if (_wallBump.isPlaying) return;
        _wallBump.Play();
    }

    public void PlayPokemonSounds(string sound1,string sound2)
    {
        _pokemonSound01.clip = _pokemonSound02.clip = null;
        AudioClip clip1 = Resources.Load<AudioClip>($"Cries/{sound1}");
        AudioClip clip2 = Resources.Load<AudioClip>($"Cries/{sound2}");
        if (clip1 != null)
        {
            //Debug.Log($"Playing sound 1 : {sound1}");
            _pokemonSound01.clip = clip1;
            _pokemonSound01.Play();
        }
        if (clip2 != null)
        {
            //Debug.Log($"Playing sound 2 : {sound2}");
            _pokemonSound02.clip = clip2;
            _pokemonSound02.Play();
        }
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
        if (!_currentAudioSource) return;
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
