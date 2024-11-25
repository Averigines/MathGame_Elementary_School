using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private LevelData _currentLevelData;
    
    [SerializeField] private Player player;
    [SerializeField] private GameObject fishClusterContainerPrefab;
    [SerializeField] private Background background;

    private int _currPoints;
    private List<Tuple<FishType, int>> _calculationPath;
    private List<GameObject> _fishClusters;

    [SerializeField] private FishCatchUI _fishCatchUI;
    [SerializeField] private NumbersUI _numbersUI;
    
    public event Action OnLevelCompleted;
    public event Action OnCatchSequenceStart;
    public event Action OnCatchSequenceEnd;

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
        _calculationPath = new List<Tuple<FishType, int>>();
        
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

        _numbersUI.ResetCalculationPath();
        _numbersUI.ChangeGoalNumber(_currentLevelData.goalNumber);
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
        fishCluster.OnFishReeledIn += HandleFishReeledIn;
    }

    private void HandleFishReeledIn(FishType type, int points)
    {
        player.ChangePlayerState(Player.PlayerState.Idle);
        StartCoroutine(CatchFishSequence(type, points));
    }

    private void ChangePoints(FishType type, int points, string calcPathString)
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

        _numbersUI.ChangeCalculationPath(calcPathString);
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

    private IEnumerator CatchFishSequence(FishType type, int points)
    {
        OnCatchSequenceStart?.Invoke();
        
        string value = null;

        switch (type)
        {
            case FishType.Addition:
                _calculationPath.Add(new Tuple<FishType, int>(FishType.Addition, points));
                value = "+ " + points;
                break;
            case FishType.Substraction:
                _calculationPath.Add(new Tuple<FishType, int>(FishType.Substraction, points));
                value = "- " + points;
                break;
            default:
                break;
        }

        string calcPathString = GetStringSequenceForCalculationPath();
        
        yield return StartCoroutine(_fishCatchUI.ShowValue(value));
        yield return new WaitForSeconds(2);
        yield return StartCoroutine(_fishCatchUI.HideValue());
        yield return new WaitForSeconds(2);
        yield return StartCoroutine(_fishCatchUI.ShowResult(calcPathString));
        yield return new WaitForSeconds(2);
        yield return StartCoroutine(_fishCatchUI.HideResult());
        ChangePoints(type, points, calcPathString);
        
        OnCatchSequenceEnd?.Invoke();
    }

    private string GetStringSequenceForCalculationPath()
    {
        bool firstCalc = true;
        string calcPathString = null;
        
        foreach (var calc in _calculationPath)
        {
            if (firstCalc)
            {
                firstCalc = false;
                switch (calc.Item1)
                {
                    case FishType.Addition:
                        calcPathString = calc.Item2.ToString();
                        break;
                    case FishType.Substraction:
                        calcPathString = "-" + calc.Item2;
                        break;
                    default:
                        break;
                }
                continue;
            }

            switch (calc.Item1)
            {
                case FishType.Addition:
                    calcPathString += " + " + calc.Item2;
                    break;
                case FishType.Substraction:
                    calcPathString += " - " + calc.Item2;
                    break;
                default:
                    break;
            }
        }

        return calcPathString;
    }

    private void CompleteLevel()
    {
        OnLevelCompleted?.Invoke();
    }
}
