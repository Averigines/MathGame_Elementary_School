using UnityEngine;

public class Player : MonoBehaviour
{
    private SpriteRenderer _renderer;
    private Animator _animator;
    [SerializeField] private GameObject playerModel;
    [SerializeField] private GameObject fishingRod;

    [SerializeField] private float speed = 2;
    private Vector3 _startPosition;
    private Vector3 _targetPosition;
    
    public enum PlayerState
    {
        Idle,
        Fishing,
        Moving,
    }

    public PlayerState CurrPlayerState {get; private set;}
    public PlayerState[] ValidStatesForMoving {get; private set;}
    public PlayerState[] ValidStatesForFishing {get; private set;}

    private bool _needsToTurnAfterMoving;
    
    [SerializeField] private GameObject fishingRodHookPrefab;
    private FishingRodHook _activeFishingHook;

    void Start()
    {
        ValidStatesForMoving = new[] { PlayerState.Idle, PlayerState.Moving, PlayerState.Fishing };
        ValidStatesForFishing = new[] { PlayerState.Idle, PlayerState.Moving };
        CurrPlayerState = PlayerState.Idle;

        _startPosition = transform.position;
        _targetPosition = transform.position;

        _renderer = playerModel.GetComponent<SpriteRenderer>();
        _animator = playerModel.GetComponent<Animator>();
    }
    
    void Update()
    {
        if (CurrPlayerState == PlayerState.Moving)
        {
            var currPos = transform.position;
            Vector3 direction = (_targetPosition - currPos).normalized;
            MoveToPosition(direction);
        }
    }

    private void MoveToPosition(Vector3 direction)
    {
        float distanceToTarget = Vector3.Distance(_targetPosition,transform.position);
        float step = speed * Time.deltaTime;
        if (step >= distanceToTarget)
        {
            transform.position = _targetPosition;
            if (_needsToTurnAfterMoving) FlipPlayerModel(!_renderer.flipX);
            ChangePlayerState(PlayerState.Idle);
        }
        else
        {
            transform.Translate(direction * speed * Time.deltaTime);
        }
    }

    public void SetTargetPosition(float rodPos)
    {
        if (CurrPlayerState != PlayerState.Moving) ChangePlayerState(PlayerState.Moving);
        
        bool needsToFlip;
        if (rodPos < transform.position.x - playerModel.transform.lossyScale.x / 2)
        {
            needsToFlip = true;
            _needsToTurnAfterMoving = false;
        }
        else if (rodPos > transform.position.x + playerModel.transform.lossyScale.x / 2)
        {
            needsToFlip = false;
            _needsToTurnAfterMoving = false;
        }
        else if (rodPos < transform.position.x)
        {
            needsToFlip = false;
            _needsToTurnAfterMoving = true;
        }
        else
        {
            needsToFlip = true;
            _needsToTurnAfterMoving = true;
        }

        FlipPlayerModel(needsToFlip);
        
        bool fishingRodFlipped = _needsToTurnAfterMoving ? !needsToFlip : needsToFlip;
        AdjustRodPosition(fishingRodFlipped);
        
        float rodOffset = fishingRod.transform.localPosition.x;

        float targetParentX = rodPos - rodOffset;
        _targetPosition = new Vector3(targetParentX, transform.position.y, transform.position.z);
    }

    private void FlipPlayerModel(bool needsToBeFlipped)
    {
        _renderer.flipX = needsToBeFlipped;
    }
    
    private void AdjustRodPosition(bool needsToBeFlipped)
    {
        var localPos = fishingRod.transform.localPosition;
        var fishingRodPosAbs = Mathf.Abs(localPos.x);
        fishingRod.transform.localPosition = needsToBeFlipped ? new Vector3(-fishingRodPosAbs, localPos.y, localPos.z) : new Vector3(fishingRodPosAbs, localPos.y, localPos.z);
    }
    
    public void StartFishing()
    {
        ChangePlayerState(PlayerState.Fishing);
        var go = Instantiate(fishingRodHookPrefab, fishingRod.transform);
        var minFishingHookDepth = ScreenData.seaArea.top;
        var maxFishingHookDepth = ScreenData.seaArea.bottom;
        _activeFishingHook = go.GetComponent<FishingRodHook>();
        _activeFishingHook.Initialize(minFishingHookDepth, maxFishingHookDepth);
    }

    private void StopFishing()
    {
        Destroy(_activeFishingHook.gameObject);
    }

    public void StartAcceleratingFishingRod()
    {
        _activeFishingHook.AccelerateHook();
    }
    
    public void StartDeceleratingFishingRod()
    {
        _activeFishingHook.DecelerateHook();
    }
    
    public void ResetPosition()
    {
        transform.position = _startPosition;
    }
    
    public void ChangePlayerState(PlayerState state)
    {
        if (state == CurrPlayerState) return;
        
        switch (CurrPlayerState)
        {
            case PlayerState.Idle:
                break;
            case PlayerState.Moving:
                break;
            case PlayerState.Fishing:
                StopFishing();
                break;
            default:
                break;
        }
        
        CurrPlayerState = state;

        _animator.ResetTrigger("Idle");
        _animator.ResetTrigger("Rowing");
        _animator.ResetTrigger("Fishing");
        
        switch (CurrPlayerState)
        {
            case PlayerState.Idle:
                _animator.SetTrigger("Idle");
                break;
            case PlayerState.Moving:
                _animator.SetTrigger("Rowing");
                break;
            case PlayerState.Fishing:
                _animator.SetTrigger("Fishing");
                break;
            default:
                _animator.SetTrigger("Idle");
                break;
        }
    }
}
