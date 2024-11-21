using System.Collections.Generic;
using Unity.Mathematics.Geometry;
using UnityEngine;

public class Player : MonoBehaviour
{
    private SpriteRenderer _renderer;

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

    void Start()
    {
        _rodStrengthThresholds = new Dictionary<RodStrengths, float>
        {
            { RodStrengths.Threshold1, rodStrengthThreshold1 },
            { RodStrengths.Threshold2, rodStrengthThreshold2 },
            { RodStrengths.Threshold3, rodStrengthThreshold3 }
        };

        _targetPosition = transform.position;

        _renderer = GetComponent<SpriteRenderer>();
    }
    
    void Update()
    {
        if (StateManager.CurrPlayerState == StateManager.PlayerState.Moving)
        {
            var currPos = transform.position;
            Vector3 direction = (_targetPosition - currPos).normalized;
            MoveToPosition(direction);
        }
    }
    
    public void StartRodUse()
    {
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
        print(_rodStrength);
        if (Mathf.Abs(_rodStrength) < _rodStrengthThresholds[RodStrengths.Threshold1])
        {
            StateManager.ChangePlayerState(StateManager.PlayerState.Idle);
        }
        // Add logic for throwing the line on every else if
        else if (Mathf.Abs(_rodStrength) < _rodStrengthThresholds[RodStrengths.Threshold2])
        {
            //For now Idle, should be fishing when implemented
            StateManager.ChangePlayerState(StateManager.PlayerState.Idle);
            //StateManager.ChangePlayerState(StateManager.PlayerState.Fishing);
        }
        else if (Mathf.Abs(_rodStrength) < _rodStrengthThresholds[RodStrengths.Threshold3])
        {
            //For now Idle, should be fishing when implemented
            StateManager.ChangePlayerState(StateManager.PlayerState.Idle);
            //StateManager.ChangePlayerState(StateManager.PlayerState.Fishing);
        }
        else
        {
            //For now Idle, should be fishing when implemented
            StateManager.ChangePlayerState(StateManager.PlayerState.Idle);
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
            StateManager.ChangePlayerState(StateManager.PlayerState.Idle);
        }
        else
        {
            var currPos = transform.position;
            transform.Translate(direction * speed * Time.deltaTime);
            var newPos = transform.position;
            print(newPos - currPos);
        }
    }

    public void SetTargetPosition(float posX)
    {
        _targetPosition = new Vector3(posX, transform.position.y, transform.position.z);
        if (_targetPosition.x < transform.position.x)
        {
            _renderer.flipX = true;
        }
        else
        {
            _renderer.flipX = false;
        }
    }
}
