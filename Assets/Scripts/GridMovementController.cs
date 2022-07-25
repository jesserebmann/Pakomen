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
    private bool _isMoving;
    private Vector3 _origPos, _targetPos;
    private float _timeToMove = 0.2f;
    public float gridSize;
    public float blockRadius = 0.01f;
    public LayerMask movementBlock;
    public Collider2D[] colliders;
    public Vector2 yourPosition;
    private Transform _begintransform;

    private void Start()
    {
        
    }

    private void FixedUpdate()
    {
        if (_isMoving) return;
        if (Gamepad.current.dpad.right.isPressed)
        {
            MovementButtonPressed("Right");
            return;
        }
        if (Gamepad.current.dpad.left.isPressed)
        {
            MovementButtonPressed("Left");
            return;
        }
        if (Gamepad.current.dpad.down.isPressed)
        {
            MovementButtonPressed("Down");
            return;
        }
        if (Gamepad.current.dpad.up.isPressed)
        {
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
        RaycastHit2D hit = Physics2D.Raycast(_origPos, direction, gridSize);
        if (hit.collider != null)
        {
            //if raycast hits something at rand_pos, not empty
            yield return null;
        }

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
    
    
    private void CheckPositionInsideCollider()
    {
        colliders = Physics2D.OverlapCircleAll(yourPosition, 0.0f);
    }
}
