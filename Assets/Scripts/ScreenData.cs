using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class ScreenData : MonoBehaviour
{
    private struct ScreenDimensions
    {
        public float bottom;
        public float top;
        public float left;
        public float right;
    }
    private static ScreenDimensions _screenDimensions;
    
    private struct SeaDimensions
    {
        public float bottom;
        public float top;
        public float left;
        public float right;
    }
    private static SeaDimensions _seaDimensions;

    private Camera _mainCamera;

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    private void Start()
    {
        Vector3 bottomLeft = _mainCamera.ViewportToWorldPoint(new Vector3(0, 0, _mainCamera.nearClipPlane));
        Vector3 topRight = _mainCamera.ViewportToWorldPoint(new Vector3(1, 1, _mainCamera.nearClipPlane));

        _screenDimensions.bottom = bottomLeft.y;
        _screenDimensions.top = topRight.y;
        _screenDimensions.left = bottomLeft.x;
        _screenDimensions.right = topRight.x;

        _seaDimensions.bottom = _screenDimensions.bottom;
        _seaDimensions.top = 0;
        _seaDimensions.left = _screenDimensions.left;
        _seaDimensions.right = _screenDimensions.right;
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
        float spawnPositionX = _seaDimensions.left + (posInPercent.x / 100f) * (_seaDimensions.right - _seaDimensions.left);
        float spawnPositionY = _seaDimensions.top + (posInPercent.y / 100f) * (_seaDimensions.bottom - _seaDimensions.top);
        Vector2 spawnPosition = new Vector2(spawnPositionX, spawnPositionY);

        return spawnPosition;
    }

    public static bool CheckIfFishClusterNeedsToTurn(DirectionX currDirectionX, Vector3 pos)
    {
        if (pos.x < _seaDimensions.left && currDirectionX == DirectionX.Left) return true;
        if (pos.x > _seaDimensions.right && currDirectionX == DirectionX.Right) return true;
        return false;
    }
}
