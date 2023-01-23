using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleController : MonoBehaviour
{
    [SerializeField] private Animator _playerAnimator;

    private bool _isInteracting;

    
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
    
    public void CatchCompleted()
    {
        UIController.Instance.HandleCatchCompleted();
        
    }
    
    public void ThrowCompleted()
    {
        if (EncounterManager.Instance.CheckIfRun())
        {
            _playerAnimator.SetTrigger("ThrowRan");
            return;
        }
        _isInteracting = false;
        UIController.Instance.SetBattleButtons(true);
    }
    public void PlayCaught()
    {
        _playerAnimator.SetTrigger("Caught");
    }
    public void Caught()
    {
        UIController.Instance.StopEncounter();
        UIController.Instance.SetBattleButtons(true);
        _isInteracting = false;
    }
    
    public void PlayEscaped()
    {
        _playerAnimator.SetTrigger("Escaped");
    }
    public void Escaped()
    {
        UIController.Instance.SetBattleButtons(true);
        _isInteracting = false;
    }
    public void PlayFled()
    {
        _playerAnimator.SetTrigger("Fled");
    }
    public void Fled()
    {
        _isInteracting = false;
        UIController.Instance.SetBattleButtons(true);
        UIController.Instance.StopEncounter();
    }

    public void Catch()
    {
        if (_isInteracting) return;
        UIController.Instance.SetBattleButtons(false);
        _isInteracting = true;
        _playerAnimator.SetTrigger("Catch");
    }
    
    public void Throw()
    {
        if (_isInteracting) return;
        UIController.Instance.SetBattleButtons(false);
        _isInteracting = true;
        _playerAnimator.SetTrigger("Throw");
    }
    
    #region Properties

    public static BattleController Instance { get; private set; }

    #endregion Properties

}
