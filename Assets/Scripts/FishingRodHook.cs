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
    [SerializeField] private float accelerationPerTap = 1f;
    [SerializeField] private float timeUntilDecelerate = 1f;
    private List<float> _accelerationTimeStamps;

    private CircleCollider2D _collider;

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
        _collider = GetComponent<CircleCollider2D>();
        _currSpeed = baseSpeed;
        _accelerationTimeStamps = new List<float>();
    }

    public void Initialize(float minDepth, float maxDepth)
    {
        _minDepth = minDepth;
        _maxDepth = maxDepth;
        transform.position = new Vector3(transform.position.x, minDepth, transform.position.z);
    }

    private void Update()
    {
        CheckForDeceleratingHook();

        transform.Translate(_hookDirectionMap[_currHookDirection] * _currSpeed * Time.deltaTime);
        if (transform.position.y <= _maxDepth) _currHookDirection = HookDirection.Up;
        if (transform.position.y >= _minDepth) _currHookDirection = HookDirection.Down;
    }

    public void AccelerateHook()
    {
        if (_currSpeed < maxSpeed) _accelerationTimeStamps.Add(Time.time);
        _currSpeed = Mathf.Max(_currSpeed + accelerationPerTap, maxSpeed);
    }
    
    private void CheckForDeceleratingHook()
    {
        for (int i = _accelerationTimeStamps.Count - 1; i >= 0; i--)
        {
            if (Time.time - _accelerationTimeStamps[i] > timeUntilDecelerate)
            {
                DecelerateHook();
                _accelerationTimeStamps.RemoveAt(i);
            }
        }
    }

    private void DecelerateHook()
    {
        _currSpeed = Mathf.Min(_currSpeed - accelerationPerTap, baseSpeed);
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
