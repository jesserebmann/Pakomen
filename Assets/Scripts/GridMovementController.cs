using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class GridMovementController : MonoBehaviour
{
    private bool _isMoving;
    private Vector3 _origPos, _targetPos;
    private float _timeToMove = 0.2f;
    public float gridSize;


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
    }
}
