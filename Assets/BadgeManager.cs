using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadgeManager : MonoBehaviour
{
    
    
    #region fields

    private static BadgeManager _badgeManager;
    private int _badge01,_badge02,_badge03,_badge04,_badge05,_badge06,_badge07,_badge08;
    public GameObject _badge01Rubble;
    public bool _resetBadges;

    private List<int> _badgesCompleted;
    
    #endregion
    
    // Start is called before the first frame update
    void Start()
    {
        if(_resetBadges)
            ResetBadges();
        _badgeManager = this;
        var badge01 = PlayerPrefs.GetInt($"badge01");
        if(badge01 == 1)
            SetBadgeComplete(1);
        else PlayerPrefs.SetInt("badge01",0);

        var badge02 = PlayerPrefs.GetInt($"badge02");
        if(badge02 == 1)
            SetBadgeComplete(2);
        else PlayerPrefs.SetInt("badge02",0);

        var badge03 = PlayerPrefs.GetInt($"badge03");
        if(badge03 == 1)
            SetBadgeComplete(3);
        else PlayerPrefs.SetInt("badge03",0);

        var badge04 = PlayerPrefs.GetInt($"badge04");
        if(badge04 == 1)
            SetBadgeComplete(4);
        else PlayerPrefs.SetInt("badge04",0);

        var badge05 = PlayerPrefs.GetInt($"badge05");
        if(badge05 == 1)
            SetBadgeComplete(5);
        else PlayerPrefs.SetInt("badge05",0);

        var badge06 = PlayerPrefs.GetInt($"badge06");
        if(badge06 == 1)
            SetBadgeComplete(6);
        else PlayerPrefs.SetInt("badge06",0);

        var badge07 = PlayerPrefs.GetInt($"badge07");
        if(badge07 == 1)
            SetBadgeComplete(7);
        else PlayerPrefs.SetInt("badge07",0);

        var badge08 = PlayerPrefs.GetInt($"badge08");
        if(badge08 == 1)
            SetBadgeComplete(8);
        else
            PlayerPrefs.SetInt("badge08",0);
    }

    public void SetBadgeComplete(int index)
    {
        switch (index)
        {
            case 1:
                if(_badge01Rubble)
                    _badge01Rubble.SetActive(false);
                PlayerPrefs.SetInt("badge01",1);
                break;
            case 2:
                PlayerPrefs.SetInt("badge02",1);

                break;
            case 3:
                PlayerPrefs.SetInt("badge03",1);

                break;
            case 4:
                PlayerPrefs.SetInt("badge04",1);

                break;
            case 5:
                PlayerPrefs.SetInt("badge05",1);

                break;
            case 6:
                PlayerPrefs.SetInt("badge06",1);

                break;
            case 7:
                PlayerPrefs.SetInt("badge07",1);

                break;
            case 8:
                PlayerPrefs.SetInt("badge08",1);

                break;
        }
    }

    private void ResetBadges()
    {
        PlayerPrefs.SetInt("badge01",0);
        PlayerPrefs.SetInt("badge02",0);
        PlayerPrefs.SetInt("badge03",0);
        PlayerPrefs.SetInt("badge04",0);
        PlayerPrefs.SetInt("badge05",0);
        PlayerPrefs.SetInt("badge06",0);
        PlayerPrefs.SetInt("badge07",0);
        PlayerPrefs.SetInt("badge08",0);
        _badge01Rubble.SetActive(true);
    }

    public static BadgeManager Instance => _badgeManager;
}
