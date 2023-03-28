using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ResetController : MonoBehaviour
{

    [SerializeField] private Transform _player;
    [SerializeField] private GameObject _entei;
    [SerializeField] public GameObject[] _points;
    [SerializeField] private Vector3[] ResetPoints;
    private static  ResetController _instance;


    public void Start()
    {
        _instance = this;
        var entei = PlayerPrefs.GetInt($"entei");
        if(entei == 1)
            EnableEntei();
        var point1 = PlayerPrefs.GetInt($"point1");
        if(point1 == 1)
            _points[0].SetActive(true);
        var point2 = PlayerPrefs.GetInt($"point2");
        if(point2 == 1)
            _points[1].SetActive(true);
        var point3 = PlayerPrefs.GetInt($"point3");
        if(point3 == 1)
            _points[2].SetActive(true);
        var point4 = PlayerPrefs.GetInt($"point4");
        if(point4 == 1)
            _points[3].SetActive(true);
        var point5 = PlayerPrefs.GetInt($"point5");
        if(point5 == 1)
            _points[4].SetActive(true);
        var point6 = PlayerPrefs.GetInt($"point6");
        if(point6 == 1)
            _points[5].SetActive(true);
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
                _player.SetPositionAndRotation(ResetPoints[0], quaternion.identity);
                break;
            case 1 :
                _player.SetPositionAndRotation(ResetPoints[1], quaternion.identity);
                break;
            case 2 :
                _player.SetPositionAndRotation(ResetPoints[2], quaternion.identity);
                break;
            case 3 :
                _player.SetPositionAndRotation(ResetPoints[3], quaternion.identity);
                break;
            case 4 :
                _player.SetPositionAndRotation(ResetPoints[4], quaternion.identity);
                break;
            case 5 :
                _player.SetPositionAndRotation(ResetPoints[5], quaternion.identity);
                break;
            case 6 : // Entei
                _player.SetPositionAndRotation(ResetPoints[6], quaternion.identity);
                break;
        }
        PlayerPrefs.SetInt("positionX",Convert.ToInt16(_player.position.x*10));
        PlayerPrefs.SetInt("positionY",Convert.ToInt16(_player.position.y*10));
    }

    public static ResetController Instance => _instance;
}
