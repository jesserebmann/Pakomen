using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.InputSystem;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class GridMovementController : MonoBehaviour
{
    private enum Directions
    {
        Up,
        Down,
        Left,
        Right
    }
    private bool _isMoving;
    private Vector3 _origPos, _targetPos;
    private float _timeToMove = 0.2f;
    public float gridSize;
    public float blockRadius = 0.01f;
    public LayerMask movementBlock;
    public Vector2 yourPosition;
    private Transform _begintransform;
    
    //States
    private Directions _directionFacing;

    private void Start()
    {
        
    }

    private void FixedUpdate()
    {
        if (_isMoving) return;
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
        }
    }

    public void MovementButtonPressed(string direction)
    {
        switch (direction)
        {
            case "Up":
                StartCoroutine(MovePlayer(Vector3.up));
                break;
            case "Down":
                StartCoroutine(MovePlayer(Vector3.down));
                break;
            case "Left":
                StartCoroutine(MovePlayer(Vector3.left));
                break;
            case "Right":
                StartCoroutine(MovePlayer(Vector3.right));
                break;
            
        }
    }

    private IEnumerator MovePlayer(Vector3 direction)
    {
        _isMoving = true;
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
    
    
    private bool CheckMovementBlock(Vector3 targetPos)
    {
        return Physics2D.OverlapCircle(targetPos, 1f, movementBlock);
    }
}
