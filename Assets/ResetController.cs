using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ResetController : MonoBehaviour
{

    [SerializeField] private Transform _player;
    [SerializeField] private GameObject _entei;
    private static  ResetController _instance;


    public void Start()
    {
        _instance = this;
        var entei = PlayerPrefs.GetInt($"entei");
        if(entei == 1)
            EnableEntei();
    }

    public void EnableEntei()
    {
        _entei.SetActive(true);
        PlayerPrefs.SetInt("entei",1);
    }

    public void ResetLegendary(GameObject go)
    {
        go.SetActive(true);
    }

    public void ResetPLayer(int index)
    {
        switch (index)
        {
            case 0 :
                _player.SetPositionAndRotation(Vector3.zero, quaternion.identity);
                break;
            case 1 :
                _player.SetPositionAndRotation(new Vector3(14,15,0), quaternion.identity);
                break;
            case 2 :
                _player.SetPositionAndRotation(new Vector3(14,15,0), quaternion.identity);
                break;
            case 3 :
                _player.SetPositionAndRotation(new Vector3(14,15,0), quaternion.identity);
                break;
            case 4 :
                _player.SetPositionAndRotation(new Vector3(14,15,0), quaternion.identity);
                break;
            case 5 :
                _player.SetPositionAndRotation(new Vector3(14,15,0), quaternion.identity);
                break;
            case 6 : // Entei
                _player.SetPositionAndRotation(new Vector3(-12,12.5f,0), quaternion.identity);
                break;
        }
        PlayerPrefs.SetInt("positionX",Convert.ToInt16(_player.position.x*10));
        PlayerPrefs.SetInt("positionY",Convert.ToInt16(_player.position.y*10));
    }

    public static ResetController Instance => _instance;
}
