using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleController : MonoBehaviour
{
    [SerializeField] private Animator _playerAnimator;

    private bool _isCatching;

    public void CatchCompleted()
    {
        UIController.Instance.CatchPokemon();
        _isCatching = false;

    }

    public void Catch()
    {
        if (_isCatching) return;
        UIController.Instance.SetBattleButtons(false);
        _isCatching = true;
        _playerAnimator.SetTrigger("Catch");
    }

}
