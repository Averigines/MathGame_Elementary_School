using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class ScreenData : MonoBehaviour
{
    [SerializeField] private float seaHeight = 0.7f;

    [SerializeField] private float fishSpaceToWaterEdgeX = 0.4f;
    [SerializeField] private float fishSpaceToWaterEdgeY = 0.4f;

    public struct ScreenArea
    {
        public float bottom;
        public float top;
        public float left;
        public float right;
        public float height;
        public float width;
    }
    public static ScreenArea screenArea;
    
    public struct SeaArea
    {
        public float bottom;
        public float top;
        public float left;
        public float right;
        public float height;
        public float width;
    }
    public static SeaArea seaArea;
    
    public struct BackgroundArea
    {
        public float bottom;
        public float top;
        public float left;
        public float right;
        public float height;
        public float width;
    }
    public static BackgroundArea backgroundArea;
    
    private struct FishArea
    {
        public float bottom;
        public float top;
        public float left;
        public float right;
        public float height;
        public float width;
    }
    private static FishArea fishArea;

    private Camera _mainCamera;

    private void Awake()
    {
        _mainCamera = Camera.main;
        CalculateScreenArea();
        CalculateSeaArea();
        CalculateBackgroundArea();
        CalculateFishArea();
    }

    private void CalculateBackgroundArea()
    {
        backgroundArea.left = screenArea.left;
        backgroundArea.right = screenArea.right;
        backgroundArea.width = backgroundArea.right - backgroundArea.left;
        
        backgroundArea.bottom = seaArea.top;
        backgroundArea.top = screenArea.top;
        backgroundArea.height = backgroundArea.top - backgroundArea.bottom;
        
    }

    private void CalculateSeaArea()
    {
        seaArea.bottom = screenArea.bottom;
        seaArea.top = screenArea.bottom + seaHeight * screenArea.height;
        seaArea.left = screenArea.left;
        seaArea.right = screenArea.right;
        seaArea.height = seaArea.top - seaArea.bottom;
        seaArea.width = seaArea.right - seaArea.left;
    }

    private void CalculateScreenArea()
    {
        Vector3 bottomLeft = _mainCamera.ViewportToWorldPoint(new Vector3(0, 0, _mainCamera.nearClipPlane));
        Vector3 topRight = _mainCamera.ViewportToWorldPoint(new Vector3(1, 1, _mainCamera.nearClipPlane));
        
        screenArea.bottom = bottomLeft.y;
        screenArea.top = topRight.y;
        screenArea.left = bottomLeft.x;
        screenArea.right = topRight.x;
        screenArea.height = screenArea.top - screenArea.bottom;
        screenArea.width = screenArea.right - screenArea.left;
    }

    private void CalculateFishArea()
    {
        fishArea.bottom = seaArea.bottom + fishSpaceToWaterEdgeY;
        fishArea.top = seaArea.top - fishSpaceToWaterEdgeY;
        fishArea.left = seaArea.left + fishSpaceToWaterEdgeX;
        fishArea.right = seaArea.right - fishSpaceToWaterEdgeX;
        fishArea.height = fishArea.top - fishArea.bottom;
        fishArea.width = fishArea.right - fishArea.left;
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
        float spawnPositionX = fishArea.left + (posInPercent.x / 100f) * (fishArea.right - fishArea.left);
        float spawnPositionY = fishArea.top + (posInPercent.y / 100f) * (fishArea.bottom - fishArea.top);
        Vector2 spawnPosition = new Vector2(spawnPositionX, spawnPositionY);

        return spawnPosition;
    }

    public static bool CheckIfFishClusterNeedsToTurn(DirectionX currDirectionX, Vector3 pos)
    {
        if (pos.x < fishArea.left && currDirectionX == DirectionX.Left) return true;
        if (pos.x > fishArea.right && currDirectionX == DirectionX.Right) return true;
        return false;
    }
}
