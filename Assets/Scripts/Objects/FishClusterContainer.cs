using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FishClusterContainer : MonoBehaviour
{
    private float _timer = 0f;
    private List<Fish> _fishInContainer;
    private DirectionX _directionX;
    private DirectionY _directionY;
    private float _speedX;
    private float _speedY = 2f;
    private int _value;
    private FishType _type;
    private float _radius = 0.4f;
    private Vector2 _fishAreaX;

    private struct YRange
    {
        public float min;
        public float max;
    }
    private YRange _yRange;

    [SerializeField] private Vector2 textOffset = new Vector2(0.3f, 0.3f);
    public event Action<FishType, int> OnFishReeledIn;

    public void Initialize(FishData fishData, float startPosY, Vector2 fishAreaX, DirectionX directionX, DirectionY directionY)
    {
        _fishInContainer = new List<Fish>();
        _directionX = directionX;
        _directionY = directionY;
        _speedX = fishData.speedX;
        _value = fishData.amount;
        _type = fishData.type;

        _yRange.max = startPosY + 0.2f;
        _yRange.min = startPosY - 0.2f;
        _fishAreaX = fishAreaX;

        SpawnFishInContainer(fishData);
        SetupValueText(textOffset);
    }

    private void SpawnFishInContainer(FishData fishData)
    {
        int fishPlaced = 0;
        int ringIndex = 0;

        while (fishPlaced < fishData.amount)
        {
            int fishesInRing = (ringIndex == 0) ? 1 : 6 * ringIndex; // Fish count per ring
            float angleStep = 360f / fishesInRing;

            for (int i = 0; i < fishesInRing && fishPlaced < fishData.amount; i++)
            {
                // Calculate angle
                float angle = i * angleStep * Mathf.Deg2Rad; // Convert to radians

                // Calculate position
                float x = transform.position.x + _radius * ringIndex * Mathf.Cos(angle);
                float y = transform.position.y + _radius * ringIndex * Mathf.Sin(angle);
                Vector3 fishPosition = new Vector3(x, y, transform.position.z);

                // Instantiate fish
                var go = Instantiate(fishData.prefab, fishPosition, Quaternion.identity, transform);
                var fish = go.GetComponent<Fish>();
                fish.Initialize(_directionX);
                _fishInContainer.Add(fish);

                fishPlaced++;
            }

            ringIndex++; // Move to the next ring
        }
    }
    
    private void SetupValueText(Vector2 offset)
    {
        Vector2 mostRightFishLocalPos = new Vector2(0, 0);
        foreach (var fish in _fishInContainer)
        {
            if (fish.transform.localPosition.x + fish.transform.localPosition.y > mostRightFishLocalPos.x + mostRightFishLocalPos.y)
            {
                mostRightFishLocalPos = new Vector2(fish.transform.localPosition.x, fish.transform.localPosition.y);
            }
        }

        Canvas canvasValue = GetComponentInChildren<Canvas>();
        canvasValue.transform.localPosition = mostRightFishLocalPos + offset;
        
        TextMeshProUGUI textValue = GetComponentInChildren<TextMeshProUGUI>();
        switch (_type)
        {
            case FishType.Addition:
                textValue.text = "+" + _value;
                textValue.color = Color.red;
                break;
            case FishType.Substraction:
                textValue.text = "-" + _value;
                textValue.color = Color.blue;
                break;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPosition = transform.position;
        if (_directionX == DirectionX.Left)
        {
            newPosition += Vector3.left * Time.deltaTime * _speedX;
        }

        if (_directionX == DirectionX.Right)
        {
            newPosition += Vector3.right * Time.deltaTime * _speedX;
        }
        
        if (_directionY == DirectionY.Up)
        {
            newPosition += Vector3.up * Time.deltaTime * _speedY;
        }

        if (_directionY == DirectionY.Down)
        {
            newPosition += Vector3.down * Time.deltaTime * _speedY;
        }
        
        _timer += Time.deltaTime;

        float amplitude = (_yRange.max - _yRange.min) / 2f;
        float offset = (_yRange.max + _yRange.min) / 2f;
        float frequency = _speedY;   
        
        newPosition.y = amplitude * Mathf.Sin(frequency * _timer) + offset;

        transform.position = newPosition;

        if (ScreenData.CheckIfFishClusterNeedsToTurn(_directionX, transform.position, _fishAreaX))
        {
            _directionX = _directionX == DirectionX.Left ? DirectionX.Right : DirectionX.Left;
            foreach (var fish in _fishInContainer)
            {
                fish.FlipSprite();
            }
        }

        if (transform.position.y > _yRange.max && _directionY == DirectionY.Up) _directionY = DirectionY.Down;
        if (transform.position.y < _yRange.min && _directionY == DirectionY.Down) _directionY = DirectionY.Up;
    }

    public void GetReeledIn()
    {
        OnFishReeledIn?.Invoke(_type, _value);
        Destroy(gameObject);
    }
}
