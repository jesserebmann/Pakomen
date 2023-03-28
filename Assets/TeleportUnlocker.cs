using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportUnlocker : MonoBehaviour
{

    [SerializeField] private int index;
    
    public void UnlockTeleportPoint()
    {
        PlayerPrefs.SetInt($"point{index}",1);
        ResetController.Instance._points[index].SetActive(true);
    }
}
