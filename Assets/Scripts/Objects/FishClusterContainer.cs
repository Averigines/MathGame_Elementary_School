using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FishClusterContainer : MonoBehaviour
{
    [SerializeField] private Canvas textCanvas;
    [SerializeField] private TextMeshProUGUI textMesh;
    [SerializeField] private Vector2 textOffset = new Vector2(0.3f, 0.3f);

    [SerializeField] private FishingRange fishingRange;

    [SerializeField] private float fishMinScale = 0.7f;
    [SerializeField] private float fishMaxScale = 1.3f;

    [SerializeField] private GameObject fishAdditionPrefab;
    [SerializeField] private GameObject fishSubtractionPrefab;
    [SerializeField] private GameObject fishMultiplicationPrefab;

    private Dictionary<FishType, GameObject> _fishTypeToPrefabMap;

    [SerializeField] private Color textColorAddition;
    [SerializeField] private Color textColorSubstraction;
    [SerializeField] private Color textColorMultiplication;

    private Dictionary<FishType, Tuple<string, Color>> _fishTypeToFishTextMap;

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

    private CircleCollider2D _collider;

    private struct YRange
    {
        public float min;
        public float max;
    }
    private YRange _yRange;
    
    public event Action<FishType, int> OnFishReeledIn;

    private void Awake()
    {
        _collider = gameObject.GetComponent<CircleCollider2D>();
        _fishTypeToFishTextMap = new Dictionary<FishType, Tuple<string, Color>>()
        {
            { FishType.Addition, new Tuple<string, Color>("+", textColorAddition) },
            { FishType.Subtraction, new Tuple<string, Color>("-", textColorSubstraction) },
            { FishType.Multiplication, new Tuple<string, Color>("*", textColorMultiplication) },
        };
        _fishTypeToPrefabMap = new Dictionary<FishType, GameObject>()
        {
            { FishType.Addition, fishAdditionPrefab },
            { FishType.Subtraction, fishSubtractionPrefab },
            { FishType.Multiplication, fishMultiplicationPrefab },
        };
    }

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
        SetColliderSize();
        SetFishingRangeSize();
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
                
                //Calculate size
                float size = Utils.GetRandomNumber(fishMinScale, fishMaxScale);

                // Instantiate fish
                var go = Instantiate(_fishTypeToPrefabMap[fishData.type], fishPosition, Quaternion.identity, transform);
                var fish = go.GetComponent<Fish>();
                fish.Initialize(size, _directionX);
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
        
        textCanvas.transform.localPosition = mostRightFishLocalPos + offset;

        textMesh.text = _fishTypeToFishTextMap[_type].Item1 + _value;
        textMesh.color = _fishTypeToFishTextMap[_type].Item2;
    }

    private void SetColliderSize()
    {
        var edgeFish = _fishInContainer[^1];
        var edgeFishRenderer = edgeFish.renderer;
        var radius = edgeFish.transform.localPosition.magnitude + edgeFishRenderer.bounds.extents.magnitude;

        _collider.radius = radius < 0.5f ? 0.5f : radius;
    }

    private void SetFishingRangeSize()
    {
        var diameter = _collider.radius * 2;
        fishingRange.transform.localScale = new Vector3(diameter, diameter, diameter);
    }
    
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

        if (ScreenManager.CheckIfFishClusterNeedsToTurn(_directionX, transform.position, _fishAreaX))
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
    
    public bool CanGetReeledIn()
    {
        return fishingRange.CanBeReeledIn;
    }

    public void GetReeledIn()
    {
        OnFishReeledIn?.Invoke(_type, _value);
        Destroy(gameObject);
    }
    
}
