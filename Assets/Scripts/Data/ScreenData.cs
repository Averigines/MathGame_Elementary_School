using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class ScreenData : MonoBehaviour
{
    [SerializeField] private float seaHeight = 0.7f;
    [SerializeField] private float backgroundRatio;
    
    public struct ScreenDimensions
    {
        public float bottom;
        public float top;
        public float left;
        public float right;
        public float height;
        public float width;
    }
    public static ScreenDimensions screenDimensions;
    
    public struct SeaDimensions
    {
        public float bottom;
        public float top;
        public float left;
        public float right;
        public float height;
        public float width;
    }
    public static SeaDimensions seaDimensions;
    
    public struct BackgroundDimensions
    {
        public float bottom;
        public float top;
        public float left;
        public float right;
        public float height;
        public float width;
    }
    public static BackgroundDimensions backgroundDimensions;

    private Camera _mainCamera;

    private void Awake()
    {
        _mainCamera = Camera.main;
        CalculateScreenDimensions();
        CalculateSeaDimensions();
        CalculateBackgroundDimensions();
    }

    private void CalculateBackgroundDimensions()
    {
        backgroundDimensions.left = screenDimensions.left;
        backgroundDimensions.right = screenDimensions.right;
        backgroundDimensions.width = backgroundDimensions.right - backgroundDimensions.left;
        
        backgroundDimensions.bottom = seaDimensions.top;
        backgroundDimensions.top = screenDimensions.top;
        backgroundDimensions.height = backgroundDimensions.top - backgroundDimensions.bottom;
        
    }

    private void CalculateSeaDimensions()
    {
        seaDimensions.bottom = screenDimensions.bottom;
        seaDimensions.top = screenDimensions.bottom + seaHeight * screenDimensions.height;
        seaDimensions.left = screenDimensions.left;
        seaDimensions.right = screenDimensions.right;
        seaDimensions.height = seaDimensions.top - seaDimensions.bottom;
        seaDimensions.width = seaDimensions.right - seaDimensions.left;
    }

    private void CalculateScreenDimensions()
    {
        Vector3 bottomLeft = _mainCamera.ViewportToWorldPoint(new Vector3(0, 0, _mainCamera.nearClipPlane));
        Vector3 topRight = _mainCamera.ViewportToWorldPoint(new Vector3(1, 1, _mainCamera.nearClipPlane));
        
        screenDimensions.bottom = bottomLeft.y;
        screenDimensions.top = topRight.y;
        screenDimensions.left = bottomLeft.x;
        screenDimensions.right = topRight.x;
        screenDimensions.height = screenDimensions.top - screenDimensions.bottom;
        screenDimensions.width = screenDimensions.right - screenDimensions.left;
    }

    public static Vector2 GetRandomPosition(float xStart, float xEnd, float yStart, float yEnd)
    {
        float xPos = Random.Range(xStart, xEnd);
        float yPos = Random.Range(yStart, yEnd);
        Vector2 randomPos = new Vector2(xPos, yPos);

        return randomPos;
    }

    public static float GetRandomNumber(float first, float last)
    {
        float pos = Random.Range(first, last);
        return pos;
    }
    
    public static bool CoinToss()
    {
        return Random.Range(0, 2) == 0;
    }

    public static Vector2 GetFishSpawnPos(Vector2 posInPercent)
    {
        float spawnPositionX = seaDimensions.left + (posInPercent.x / 100f) * (seaDimensions.right - seaDimensions.left);
        float spawnPositionY = seaDimensions.top + (posInPercent.y / 100f) * (seaDimensions.bottom - seaDimensions.top);
        Vector2 spawnPosition = new Vector2(spawnPositionX, spawnPositionY);

        return spawnPosition;
    }

    public static bool CheckIfFishClusterNeedsToTurn(DirectionX currDirectionX, Vector3 pos)
    {
        if (pos.x < seaDimensions.left && currDirectionX == DirectionX.Left) return true;
        if (pos.x > seaDimensions.right && currDirectionX == DirectionX.Right) return true;
        return false;
    }
}
