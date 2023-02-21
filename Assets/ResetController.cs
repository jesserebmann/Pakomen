using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ResetController : MonoBehaviour
{

    [SerializeField] private Transform _player;
    private static  ResetController _instance;
    
    
    public void Start()
    {
        _instance = this;
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
        }
    }

    public static ResetController Instance => _instance;
}
