using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class LevelManager : MonoBehaviour
{
    private LevelData _currentLevelData;
    
    [SerializeField] private Player player;
    [SerializeField] private GameObject fishClusterContainerPrefab;
    [SerializeField] private Background background;

    private int _currPoints;
    private List<GameObject> _fishClusters;
    
    public delegate void OnLevelCompleted();
    public event OnLevelCompleted onLevelCompleted;

    public void InitializeLevel(LevelData levelData)
    {
        if (_fishClusters != null)
        {
            foreach (var cluster in _fishClusters)
            {
                Destroy(cluster);
            }
        }
        _fishClusters = new List<GameObject>();
        
        player.ResetPosition();
        player.ChangePlayerState(Player.PlayerState.Idle);
        _currPoints = 0;
        
        background.ChangeBackground();
        
        _currentLevelData = levelData;
        Debug.Log($"Level {_currentLevelData.levelNumber}");
        Debug.Log($"Goal Number: {_currentLevelData.goalNumber}");

        foreach (FishData fish in _currentLevelData.availableFish)
        {
            SpawnFishCluster(fish);
        }
    }
    
    private void SpawnFishCluster(FishData fishData)
    {
        var fishAreaX = ScreenData.GetFishAreaX(fishData.swimEdgeLeft, fishData.swimEdgeRight);
        var startPos = ScreenData.GetFishSpawnPos(fishData.spawnPosY, fishAreaX);
        DirectionX directionX = ScreenData.CoinToss() ? DirectionX.Left : DirectionX.Right;
        DirectionY directionY = ScreenData.CoinToss() ? DirectionY.Up : DirectionY.Down;
        
        GameObject go = Instantiate(fishClusterContainerPrefab, startPos, Quaternion.identity);
        _fishClusters.Add(go);
        
        var fishCluster = go.GetComponent<FishClusterContainer>();
        fishCluster.Initialize(fishData, startPos.y, fishAreaX, directionX, directionY);
        fishCluster.onFishReeledIn += HandleFishReeledIn;
    }

    private void HandleFishReeledIn(FishType type, int points)
    {
        switch (type)
        {
            case FishType.Addition:
                IncreasePoints(points);
                break;
            case FishType.Substraction:
                SubstractPoints(points);
                break;
            default:
                break;
        }
        
        player.ChangePlayerState(Player.PlayerState.Idle);
    }

    private void IncreasePoints(int points)
    {
        _currPoints += points;
        print(_currPoints);

        if (_currPoints == _currentLevelData.goalNumber)
        {
            CompleteLevel();
        }
    }

    private void SubstractPoints(int points)
    {
        _currPoints -= points;
        print(_currPoints);

        if (_currPoints == _currentLevelData.goalNumber)
        {
            CompleteLevel();
        }
    }
    
    private void CompleteLevel()
    {
        onLevelCompleted?.Invoke();
    }
}
