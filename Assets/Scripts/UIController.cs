using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private Camera _battleCamera;
    [SerializeField] private GameObject _battleUI;

    public void StartEncounter()
    {
        _mainCamera.gameObject.SetActive(false);
        _battleCamera.gameObject.SetActive(true);

        _battleUI.SetActive(true);
    }

    public void StopEncounter()
    {
        _mainCamera.gameObject.SetActive(true);
        _battleCamera.gameObject.SetActive(false);
        _battleUI.SetActive(false);
    }

    public async void Fade()
    {
        
    }
}
