using System.Collections.Generic;
using Unity.Mathematics.Geometry;
using UnityEditor.Animations;
using UnityEngine;

public class Player : MonoBehaviour
{
    private SpriteRenderer _renderer;
    private Animator _animator;
    [SerializeField] private GameObject playerModel;
    [SerializeField] private FishingRod fishingRod;
    
    [Header("Rod Strength Thresholds in Percent of the Total Screen Width normalized")]
    [Range(0, 1)][SerializeField] private float rodStrengthThreshold1;
    [Range(0, 1)][SerializeField] private float rodStrengthThreshold2;
    [Range(0, 1)][SerializeField] private float rodStrengthThreshold3;

    private enum RodStrengths
    {
        Threshold1,
        Threshold2,
        Threshold3,
    }
    private Dictionary<RodStrengths, float> _rodStrengthThresholds;

    private float _rodStrength;

    [SerializeField] private float speed = 2;
    private Vector3 _targetPosition;
    
    public enum PlayerState
    {
        Idle,
        AdjustingRodStrength,
        Fishing,
        Moving,
    }

    public PlayerState CurrPlayerState {get; private set;}
    public PlayerState[] ValidStatesForMoving {get; private set;}

    private bool _needsToTurnAfterMoving;

    void Start()
    {
        ValidStatesForMoving = new[] { PlayerState.Idle, PlayerState.Moving };
        CurrPlayerState = PlayerState.Idle;

        _rodStrengthThresholds = new Dictionary<RodStrengths, float>
        {
            { RodStrengths.Threshold1, rodStrengthThreshold1 },
            { RodStrengths.Threshold2, rodStrengthThreshold2 },
            { RodStrengths.Threshold3, rodStrengthThreshold3 }
        };

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
    
    public void StartRodUse()
    {
        ChangePlayerState(PlayerState.AdjustingRodStrength);
        _rodStrength = 0;
    }

    public void ChangeRodStrength(float strengthChange)
    {
        _rodStrength += strengthChange;

        if (_rodStrength < -_rodStrengthThresholds[RodStrengths.Threshold1])
        {
            _renderer.flipX = false;
        }
        if (_rodStrength > _rodStrengthThresholds[RodStrengths.Threshold1])
        {
            _renderer.flipX = true;
        }
        
    }
    
    public void ReleaseRod()
    {
        if (Mathf.Abs(_rodStrength) < _rodStrengthThresholds[RodStrengths.Threshold1])
        {
            ChangePlayerState(PlayerState.Idle);
        }
        // Add logic for throwing the line on every else if
        else if (Mathf.Abs(_rodStrength) < _rodStrengthThresholds[RodStrengths.Threshold2])
        {
            //For now Idle, should be fishing when implemented
            ChangePlayerState(PlayerState.Idle);
            //StateManager.ChangePlayerState(StateManager.PlayerState.Fishing);
        }
        else if (Mathf.Abs(_rodStrength) < _rodStrengthThresholds[RodStrengths.Threshold3])
        {
            //For now Idle, should be fishing when implemented
            ChangePlayerState(PlayerState.Idle);
            //StateManager.ChangePlayerState(StateManager.PlayerState.Fishing);
        }
        else
        {
            //For now Idle, should be fishing when implemented
            ChangePlayerState(PlayerState.Idle);
            //StateManager.ChangePlayerState(StateManager.PlayerState.Fishing);
        }
        
        _rodStrength = 0;
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
        fishingRod.AdjustRodPosition(fishingRodFlipped);
        
        float rodOffset = fishingRod.transform.localPosition.x;

        float targetParentX = rodPos - rodOffset;
        _targetPosition = new Vector3(targetParentX, transform.position.y, transform.position.z);
    }

    private void FlipPlayerModel(bool needsToBeFlipped)
    {
        _renderer.flipX = needsToBeFlipped;
    }

    private void ChangePlayerState(PlayerState state)
    {
        CurrPlayerState = state;
        print(CurrPlayerState);

        _animator.ResetTrigger("Idle");
        _animator.ResetTrigger("Rowing");
        
        //Change to correct animations when implemented
        switch (CurrPlayerState)
        {
            case PlayerState.Idle:
                _animator.SetTrigger("Idle");
                break;
            case PlayerState.Moving:
                _animator.SetTrigger("Rowing");
                break;
            case PlayerState.Fishing:
                _animator.SetTrigger("Idle");
                break;
            case PlayerState.AdjustingRodStrength:
                _animator.SetTrigger("Idle");
                break;
            default:
                _animator.SetTrigger("Idle");
                break;
        }
    }
}
