using System;
using System.Collections;
using System.Numerics;
using Pakomen;
using UnityEditor;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Random = System.Random;
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
    public int encounterRate = 10;
    public int shinyRate = 100;
    public LayerMask movementBlock;
    public LayerMask actionLayer;
    public LayerMask encounterLayer;
    public Vector2 yourPosition;
    private Transform _begintransform;
    private UnityEngine.InputSystem.PlayerInput _playerInput;
    public Animator playerAnimator;
    public UIController _UIController;
    private Directions _lookatDirection;
    
    

    private bool _inputActive;
    private Random _random = new Random();
    
    //States
    private Directions _directionFacing;

    private void Awake()
    {
        _playerInput = GetComponent<UnityEngine.InputSystem.PlayerInput>();
    }

    private void Start()
    {
        var positionX = PlayerPrefs.GetInt("positionX",0);
        var positionY = PlayerPrefs.GetInt("positionY",0);
        transform.SetPositionAndRotation(new Vector3(positionX,positionY),Quaternion.identity);
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("positionX",Convert.ToInt16(transform.position.x));
        PlayerPrefs.SetInt("positionY",Convert.ToInt16(transform.position.y));
    }

    public void OnAbuttonPressed()
    {
        if (DialogController.Instance.IsOpen)
        {
            DialogController.Instance.CloseDialog();
            return;
        }
        var interactable = CheckActionInteractable();
        if(interactable)
        {
            switch (interactable.InteractableType)
            {
                case Interactable.interactableType.dialog:
                    DialogController.Instance.OpenDialog(interactable.DialogText);
                    break;
                case Interactable.interactableType.item:
                    break;
                default:
                    break;
            }
            
        }
    }


    private void FixedUpdate()
    {
        if (_isMoving || _UIController.InEncounter || DialogController.Instance.IsOpen) return;
        var input = _playerInput.actions["Move"].ReadValue<Vector2>();
        if (input == Vector2.zero)
        {
            _inputActive = false;
            playerAnimator.SetBool("IsMoving",false);
            return;
        }

        _inputActive = true;
        playerAnimator.SetBool("IsMoving",true);
        var axis = Math.Abs(input.x) > Math.Abs(input.y) ? Axis.Horizontal : Axis.Vertical;
        if (axis == Axis.Horizontal)
        {
            if (input.x > 0)
            {
                playerAnimator.SetFloat("MoveX",1);
                playerAnimator.SetFloat("MoveY",0);
                if (CheckMovementBlock(new Vector2(transform.position.x+gridSize,transform.position.y))) return;
                MovementButtonPressed(Directions.Right);
            }
            else
            {
                playerAnimator.SetFloat("MoveX",-1);
                playerAnimator.SetFloat("MoveY",0);
                if (CheckMovementBlock(new Vector2(transform.position.x-gridSize,transform.position.y))) return;
                MovementButtonPressed(Directions.Left);
            }
        }
        else
        {
            if (input.y > 0)
            {
                playerAnimator.SetFloat("MoveX",0);
                playerAnimator.SetFloat("MoveY",1);
                if (CheckMovementBlock(new Vector2(transform.position.x,transform.position.y+gridSize))) return;
                MovementButtonPressed(Directions.Up);
            }
            else
            {
                playerAnimator.SetFloat("MoveX",0);
                playerAnimator.SetFloat("MoveY",-1);
                if (CheckMovementBlock(new Vector2(transform.position.x,transform.position.y-gridSize))) return;
                MovementButtonPressed(Directions.Down);
            }
        }
        PlayerPrefs.SetInt("positionX",Convert.ToInt16(transform.position.x));
        PlayerPrefs.SetInt("positionY",Convert.ToInt16(transform.position.y));
    }

    public void MovementButtonPressed(Directions direction)
    {
        switch (direction)
        {
            case Directions.Up:
                StartCoroutine(MovePlayer(Vector3.up));
                _lookatDirection = Directions.Up;
                break;
            case Directions.Down:
                StartCoroutine(MovePlayer(Vector3.down));
                _lookatDirection = Directions.Down;
                break;
            case Directions.Left:
                StartCoroutine(MovePlayer(Vector3.left));
                _lookatDirection = Directions.Left;
                break;
            case Directions.Right:
                StartCoroutine(MovePlayer(Vector3.right));
                _lookatDirection = Directions.Right;
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
            CheckForEncounters();
            yield return null;
    }

    private void CheckForEncounters()
    {
        var colliders = Physics2D.OverlapCircleAll(transform.position, blockRadius, encounterLayer);
        if (colliders.Length > 0)
        {
            
            int number = _random.Next(0, encounterRate);
            if (number <= 1)
            {
                playerAnimator.SetBool("IsMoving",false);
                Debug.Log("Encounter");
                var region = colliders[0].gameObject.GetComponentInParent<WildRegion>();
                var pokemonEncounter = region.GetPokemonEncounter();
                var isShiny = _random.Next(0, shinyRate) == 1;
                Debug.Log(pokemonEncounter.name);
                _UIController.StartEncounter(pokemonEncounter,isShiny);
            }
        }
    }


    private bool CheckMovementBlock(Vector2 targetPos)
    {
        var isBlockedByStatic = Physics2D.OverlapCircle(targetPos, blockRadius, movementBlock);
        if (isBlockedByStatic)
            return true;
        var isBlockedByActionItem = Physics2D.OverlapCircle(targetPos, blockRadius, actionLayer);
        if (isBlockedByActionItem)
            return true;
        return false;
    }

    private Interactable CheckActionInteractable()
    {
        RaycastHit2D hit = new RaycastHit2D();
        switch (_lookatDirection)
        {
            case Directions.Up:
                hit = Physics2D.Raycast(transform.position, new Vector2(transform.position.x,transform.position.y+gridSize),actionLayer);
                break;
            case Directions.Down:
                hit = Physics2D.Raycast(transform.position, new Vector2(transform.position.x,transform.position.y-gridSize),actionLayer);
                break;
            case Directions.Left:
                hit = Physics2D.Raycast(transform.position, new Vector2(transform.position.x+gridSize,transform.position.y),actionLayer);
                break;
            case Directions.Right:
                hit = Physics2D.Raycast(transform.position, new Vector2(transform.position.x-gridSize,transform.position.y),actionLayer);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        if (hit.collider)
        {
            return hit.transform.GetComponent<Interactable>();
        }
        return null;
    }
}
