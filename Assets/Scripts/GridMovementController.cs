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
    public LayerMask audioLayer;
    public LayerMask waterLayer;
    public Vector2 yourPosition;
    private Transform _begintransform;
    private UnityEngine.InputSystem.PlayerInput _playerInput;
    public Animator playerAnimator;
    public UIController _UIController;
    public SpriteRenderer _playerSprite;
    public ParticleSystem _grassParticle;
    private Directions _lookatDirection;
    private AudioSource _source1;
    private AudioSource _source2;

    [SerializeField] private Collider2D _playerCollider;

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
        var audioAreaItem = Physics2D.OverlapCircle(transform.position, blockRadius, audioLayer)?.GetComponent<AreaSound>();
        if(audioAreaItem) AudioManager.Instance.UpdateAreaAudio(audioAreaItem);
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
        if(interactable) interactable.TriggerInteraction();
    }


    private void Update()
    {
        if (_playerCollider.IsTouchingLayers(waterLayer))
        {
            //Change playersprite
        }
        else
        {
            
        }
        
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
                _lookatDirection = Directions.Right;
                if (CheckMovementBlock(new Vector2(transform.position.x+gridSize,transform.position.y))) return;
                MovementButtonPressed(Directions.Right);
            }
            else
            {
                playerAnimator.SetFloat("MoveX",-1);
                playerAnimator.SetFloat("MoveY",0);
                _lookatDirection = Directions.Left;
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
                _lookatDirection = Directions.Up;
                if (CheckMovementBlock(new Vector2(transform.position.x,transform.position.y+gridSize))) return;
                MovementButtonPressed(Directions.Up);
            }
            else
            {
                playerAnimator.SetFloat("MoveX",0);
                playerAnimator.SetFloat("MoveY",-1);
                _lookatDirection = Directions.Down;
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
        var audioAreaItem = Physics2D.OverlapCircle(transform.position, blockRadius, audioLayer)?.GetComponent<AreaSound>();
        if(audioAreaItem != null) AudioManager.Instance.UpdateAreaAudio(audioAreaItem);
        yield return null;
    }

    private void CheckForEncounters()
    {
        var colliders = Physics2D.OverlapCircleAll(transform.position, blockRadius, encounterLayer);
        if (colliders.Length > 0)
        {
            var region = colliders[0].gameObject.GetComponentInParent<WildRegion>();

            switch (region.WildType)
            {
                case WildRegion.WildeType.Grass:
                    _playerSprite.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
                    _grassParticle.gameObject.SetActive(true);
                    _grassParticle.Play();
                    break;
                case WildRegion.WildeType.Water:
                    playerAnimator.SetLayerWeight(1,1);
                    break;
                case WildRegion.WildeType.Cave:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            _UIController.SetBattleBackground(region.Background);
            if (region.WildType != WildRegion.WildeType.Water)
            {
                playerAnimator.SetLayerWeight(0,1);
            }
            int number = _random.Next(0, encounterRate);
            if (number <= 1)
            {
                playerAnimator.SetBool("IsMoving",false);
                //Debug.Log("Encounter");
                var pokemonEncounter = region.GetPokemonEncounter();
                var isShiny = _random.Next(0, shinyRate) == 1;
                //Debug.Log(pokemonEncounter.name);
                _UIController.StartEncounter(pokemonEncounter,isShiny);
                AudioManager.Instance.Pause();
                AudioManager.Instance.PlayEncounterAudio();
            }
        }
        else
        {
            playerAnimator.SetLayerWeight(1,0);
            _playerSprite.maskInteraction = SpriteMaskInteraction.None;
            _grassParticle.gameObject.SetActive(false);
        }

    }


    private bool CheckMovementBlock(Vector2 targetPos)
    {
        var isBlockedByStatic = Physics2D.OverlapCircle(targetPos, blockRadius, movementBlock);
        if (isBlockedByStatic)
        {
            AudioManager.Instance.PlayWallBump();
            return true;
        }
        var isBlockedByActionItem = Physics2D.OverlapCircle(targetPos, blockRadius, actionLayer);
        if (isBlockedByActionItem)
        {
            AudioManager.Instance.PlayWallBump();
            return true;
        }
        return false;
    }

    private Interactable CheckActionInteractable()
    {
        RaycastHit2D hit = new RaycastHit2D();
        Vector2 target = new Vector2();
        switch (_lookatDirection)
        {
            case Directions.Up:
                target = new Vector2(transform.position.x, transform.position.y + gridSize);
                hit = Physics2D.Linecast(transform.position, new Vector2(transform.position.x,transform.position.y+gridSize),actionLayer);
                break;
            case Directions.Down:
                target = new Vector2(transform.position.x, transform.position.y - gridSize);
                hit = Physics2D.Linecast(transform.position, new Vector2(transform.position.x,transform.position.y-gridSize),actionLayer);
                break;
            case Directions.Left:
                new Vector2(transform.position.x - gridSize, transform.position.y);
                hit = Physics2D.Linecast(transform.position, new Vector2(transform.position.x-gridSize,transform.position.y),actionLayer);
                break;
            case Directions.Right:
                target = new Vector2(transform.position.x+gridSize,transform.position.y);
                hit = Physics2D.Linecast(transform.position, new Vector2(transform.position.x+gridSize,transform.position.y),actionLayer);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        Debug.Log($"pos : {transform.position}   target : {target}   lookat: {_lookatDirection.ToString()}");
        if (hit.collider)
        {
            return hit.transform.GetComponent<Interactable>();
        }
        return null;
    }
}
