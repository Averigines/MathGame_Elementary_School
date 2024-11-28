using System;
using UnityEngine;

public class MenuPlayer : MonoBehaviour
{
    [SerializeField] private GameObject playerModel;
    private SpriteRenderer _renderer;
    
    [SerializeField] private float speed = 0.5f;

    private float _pointA;
    private float _pointB;
    private bool _movingToB = true;
    private Vector3 _targetPosition;

    private void Start()
    {
        _renderer = playerModel.GetComponent<SpriteRenderer>();
        _pointA = ScreenManager.seaArea.left + _renderer.bounds.size.x / 2;
        _pointB = ScreenManager.seaArea.right - _renderer.bounds.size.x / 2;
        _targetPosition = new Vector3(_pointB, transform.position.y, transform.position.z);
    }

    private void Update()
    {
        var currPos = transform.position;
        Vector3 direction = (_targetPosition - currPos).normalized;
        MoveToPosition(direction);
    }

    private void MoveToPosition(Vector3 direction)
    {
        float distanceToTarget = Vector3.Distance(_targetPosition,transform.position);
        float step = speed * Time.deltaTime;
        if (step >= distanceToTarget)
        {
            transform.position = _targetPosition;
            _movingToB = !_movingToB;
            if (_movingToB) _targetPosition = new Vector3(_pointB, transform.position.y, transform.position.z);
            else _targetPosition = new Vector3(_pointA, transform.position.y, transform.position.z);
            FlipPlayerModel(!_renderer.flipX);
        }
        else
        {
            transform.Translate(direction * speed * Time.deltaTime);
        }
    }

    private void FlipPlayerModel(bool needsToBeFlipped)
    {
        _renderer.flipX = needsToBeFlipped;
    }
}
