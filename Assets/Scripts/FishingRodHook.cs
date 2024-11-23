using System;
using System.Collections.Generic;
using UnityEngine;

public class FishingRodHook : MonoBehaviour
{
    private float _maxDepth;
    private float _minDepth;
    [SerializeField] private float speed = 2;

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
    }

    public void Initialize(float minDepth, float maxDepth)
    {
        _minDepth = minDepth;
        _maxDepth = maxDepth;
        transform.position = new Vector3(transform.position.x, minDepth, transform.position.z);
    }

    private void Update()
    {
        transform.Translate(_hookDirectionMap[_currHookDirection] * speed * Time.deltaTime);
        if (transform.position.y <= _maxDepth) _currHookDirection = HookDirection.Up;
        if (transform.position.y >= _minDepth) _currHookDirection = HookDirection.Down;
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
