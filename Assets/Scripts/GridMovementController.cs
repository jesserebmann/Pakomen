using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using UnityEngine;
using UnityEngine.InputSystem;
using Debug = UnityEngine.Debug;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class GridMovementController : MonoBehaviour
{
    public enum Directions
    {
        Up,
        Down,
        Left,
        Right
    }
    public enum Axis
    {
        Horizontal,
        Vertical
    }
    
    private bool _isMoving;
    private Vector3 _origPos, _targetPos;
    private float _timeToMove = 0.2f;
    public float gridSize;
    public float blockRadius = 0.01f;
    public LayerMask movementBlock;
    public Vector2 yourPosition;
    private Transform _begintransform;
    private UnityEngine.InputSystem.PlayerInput _playerInput;
    public Animator playerAnimator;
    private bool _inputActive;
    
    //States
    private Directions _directionFacing;

    private void Awake()
    {
        _playerInput = GetComponent<UnityEngine.InputSystem.PlayerInput>();
    }

    private void FixedUpdate()
    {
        if (_isMoving) return;
        var input = _playerInput.actions["Move"].ReadValue<Vector2>();
        if (input == Vector2.zero)
        {
            _inputActive = false;
            playerAnimator.SetBool("IsMoving",false);
            return;
        }

        _inputActive = true;
        var axis = Math.Abs(input.x) > Math.Abs(input.y) ? Axis.Horizontal : Axis.Vertical;
        if (axis == Axis.Horizontal)
        {
            if (input.x > 0)
            {
                if (CheckMovementBlock(new Vector2(transform.position.x+gridSize,transform.position.y))) return;
                playerAnimator.SetFloat("MoveX",1);
                playerAnimator.SetFloat("MoveY",0);
                MovementButtonPressed(Directions.Right);
            }
            else
            {
                if (CheckMovementBlock(new Vector2(transform.position.x-gridSize,transform.position.y))) return;
                playerAnimator.SetFloat("MoveX",-1);
                playerAnimator.SetFloat("MoveY",0);
                MovementButtonPressed(Directions.Left);
            }
        }
        else
        {
            if (input.y > 0)
            {
                if (CheckMovementBlock(new Vector2(transform.position.x,transform.position.y+gridSize))) return;
                playerAnimator.SetFloat("MoveX",0);
                playerAnimator.SetFloat("MoveY",1);
                MovementButtonPressed(Directions.Up);
            }
            else
            {
                if (CheckMovementBlock(new Vector2(transform.position.x,transform.position.y-gridSize))) return;
                playerAnimator.SetFloat("MoveX",0);
                playerAnimator.SetFloat("MoveY",-1);
                MovementButtonPressed(Directions.Down);
            }
        }

        /*if (_isMoving) return;
        if (Gamepad.current.dpad.right.isPressed)
        {
            if (CheckMovementBlock(transform.position)) return;
            MovementButtonPressed("Right");
            return;
        }
        if (Gamepad.current.dpad.left.isPressed)
        {
            if (CheckMovementBlock(transform.position)) return;
            MovementButtonPressed("Left");
            return;
        }
        if (Gamepad.current.dpad.down.isPressed)
        {
            if (CheckMovementBlock(transform.position)) return;
            MovementButtonPressed("Down");
            return;
        }
        if (Gamepad.current.dpad.up.isPressed)
        {
            if (CheckMovementBlock(transform.position)) return;
            MovementButtonPressed("Up");
            return;
        }*/
    }

    public void MovementButtonPressed(Directions direction)
    {
        switch (direction)
        {
            case Directions.Up:
                StartCoroutine(MovePlayer(Vector3.up));
                break;
            case Directions.Down:
                StartCoroutine(MovePlayer(Vector3.down));
                break;
            case Directions.Left:
                StartCoroutine(MovePlayer(Vector3.left));
                break;
            case Directions.Right:
                StartCoroutine(MovePlayer(Vector3.right));
                break;
            
        }
    }

    private IEnumerator MovePlayer(Vector3 direction)
    {
        _isMoving = true;
        playerAnimator.SetBool("IsMoving",true);
        _begintransform = transform;
        float elapsedTime = 0;
        _origPos = transform.position;
        _targetPos = _origPos + (direction*gridSize);

        while (elapsedTime < _timeToMove)
            {
                transform.position = Vector3.Lerp(_origPos, _targetPos, (elapsedTime / _timeToMove));
                elapsedTime += Time.deltaTime;
                yield return null;
                
            }
            transform.position = _targetPos;
            _isMoving = false;
            yield return null;
    }
    
    
    private bool CheckMovementBlock(Vector2 targetPos)
    {
        return Physics2D.OverlapCircle(targetPos, blockRadius, movementBlock);
    }
}
