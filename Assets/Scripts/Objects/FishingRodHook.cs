using System;
using System.Collections.Generic;
using UnityEngine;

public class FishingRodHook : MonoBehaviour
{
    private float _maxDepth;
    private float _minDepth;
    [SerializeField] private float baseSpeed = 2f;
    [SerializeField] private float maxSpeed = 10f;
    private float _currSpeed;
    [SerializeField] private float accelerationRate = 20f;
    private bool _isAccelerating = false;

    private enum HookDirection
    {
        Up,
        Down
    }
    private HookDirection _currHookDirection;
    private Dictionary<HookDirection, Vector3> _hookDirectionMap = new Dictionary<HookDirection, Vector3>()
    {
        { HookDirection.Up, Vector3.up },
        { HookDirection.Down, Vector3.down }
    };

    private void Start()
    {
        _currHookDirection = HookDirection.Down;
        _currSpeed = baseSpeed;
    }

    public void Initialize(float minDepth, float maxDepth)
    {
        _minDepth = minDepth;
        _maxDepth = maxDepth;
        transform.position = new Vector3(transform.position.x, minDepth, transform.position.z);
    }

    private void Update()
    {
        if (_isAccelerating)
        {
            _currSpeed = Mathf.Min(_currSpeed + accelerationRate * Time.deltaTime, maxSpeed);
        }
        else
        {
            _currSpeed = Mathf.Max(_currSpeed - accelerationRate * Time.deltaTime, baseSpeed);
        }
        
        transform.Translate(_hookDirectionMap[_currHookDirection] * _currSpeed * Time.deltaTime);
        if (transform.position.y <= _maxDepth) _currHookDirection = HookDirection.Up;
        if (transform.position.y >= _minDepth) _currHookDirection = HookDirection.Down;
    }

    public void AccelerateHook()
    {
        _isAccelerating = true;
    }
    
    public void DecelerateHook()
    {
        _isAccelerating = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Fish"))
        {
            if (other.gameObject.TryGetComponent<FishClusterContainer>(out FishClusterContainer fishCluster))
            {
                fishCluster.GetReeledIn();
            }
        }
    }
}
