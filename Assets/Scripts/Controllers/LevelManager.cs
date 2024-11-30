using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    private LevelData _currentLevelData;
    
    [SerializeField] private Player player;
    [SerializeField] private GameObject fishClusterContainerPrefab;
    [SerializeField] private Background background;

    private int _currPoints;
    private List<Tuple<FishType, int>> _calculationPath;
    private List<GameObject> _fishClusters;

    [SerializeField] private ResultUI _resultUI;
    [SerializeField] private NumbersUI _numbersUI;

    public event Action OnLevelCompleted;
    public event Action OnShowCalcPathStart;
    public event Action OnShowCalcPathEnd;
    public event Action OnIncorrectScoreSubmitted;

    private void OnEnable()
    {
        OnShowCalcPathStart += DisableButtonsWhenShowingCalcPath;
        OnShowCalcPathEnd += EnableButtonsWhenFinishShowingCalcPath;
    }

    private void OnDisable()
    {
        OnShowCalcPathStart -= DisableButtonsWhenShowingCalcPath;
        OnShowCalcPathEnd -= EnableButtonsWhenFinishShowingCalcPath;
    }
    
    private void DisableButtonsWhenShowingCalcPath()
    {
        _numbersUI.ChangeSubmitButton(false);
    }

    private void EnableButtonsWhenFinishShowingCalcPath()
    {
        _numbersUI.ResetSubmitButton();
    }

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

        background.ChangeBackground();

        _currentLevelData = levelData;
        
        _resultUI.ResetForNewLevel();
        _numbersUI.ResetForNewLevel(_currentLevelData.startNumber, _currentLevelData.goalNumber);

        _currPoints = _currentLevelData.startNumber;
        _calculationPath = new List<Tuple<FishType, int>> { new Tuple<FishType, int>(FishType.Addition, _currPoints) };
        StartCoroutine(ShowStartNumberSequence());

        foreach (FishData fish in _currentLevelData.availableFish)
        {
            SpawnFishCluster(fish);
        }
    }
    
    private void SpawnFishCluster(FishData fishData)
    {
        var fishAreaX = ScreenManager.GetFishAreaX(fishData.swimEdgeLeft, fishData.swimEdgeRight);
        var startPos = ScreenManager.GetFishSpawnPos(fishData.spawnPosY, fishAreaX);
        DirectionX directionX = Utils.CoinToss() ? DirectionX.Left : DirectionX.Right;
        DirectionY directionY = Utils.CoinToss() ? DirectionY.Up : DirectionY.Down;
        
        GameObject go = Instantiate(fishClusterContainerPrefab, startPos, Quaternion.identity);
        _fishClusters.Add(go);
        
        var fishCluster = go.GetComponent<FishClusterContainer>();
        fishCluster.Initialize(fishData, startPos.y, fishAreaX, directionX, directionY);
        fishCluster.OnFishReeledIn += HandleFishReeledIn;
    }

    private IEnumerator ShowStartNumberSequence()
    {
        OnShowCalcPathStart?.Invoke();
        yield return StartCoroutine(_resultUI.ShowStartNumber(_currentLevelData.startNumber.ToString()));
        yield return new WaitForSeconds(2);
        yield return StartCoroutine(_resultUI.HideUI());
        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(_resultUI.ShowGoalNumber(_currentLevelData.goalNumber.ToString()));
        yield return new WaitForSeconds(2);
        yield return StartCoroutine(_resultUI.HideUI());
        OnShowCalcPathEnd?.Invoke();
    }

    private void HandleFishReeledIn(FishType type, int points)
    {
        player.StopFishing(Player.PlayerState.Hooking);
        StartCoroutine(CatchFishSequence(type, points));
    }

    private IEnumerator CatchFishSequence(FishType type, int points)
    {
        switch (type)
        {
            case FishType.Addition:
                _calculationPath.Add(new Tuple<FishType, int>(FishType.Addition, points));
                break;
            case FishType.Subtraction:
                _calculationPath.Add(new Tuple<FishType, int>(FishType.Subtraction, points));
                break;
            case FishType.Multiplication:
                _calculationPath.Add(new Tuple<FishType, int>(FishType.Multiplication, points));
                break;
            default:
                break;
        }
        
        yield return StartCoroutine(ResultSequence(type, points));
    }

    private IEnumerator ResultSequence(FishType type, int points)
    {
        OnShowCalcPathStart?.Invoke();
        string calcPathString = GetStringSequenceForCalculationPath(false);
        
        yield return StartCoroutine(_resultUI.ShowResult(calcPathString, points));
        yield return new WaitForSeconds(2);
        yield return StartCoroutine(_resultUI.HideUI());
        OnShowCalcPathEnd?.Invoke();
        ChangePoints(type, points, calcPathString);
    }
    
    private IEnumerator ResultSequence(bool correctResult)
    {
        OnShowCalcPathStart?.Invoke();
        string calcPathString = GetStringSequenceForCalculationPath(true);
        
        yield return StartCoroutine(_resultUI.ShowResult(calcPathString, correctResult));
        yield return new WaitForSeconds(2);
        yield return StartCoroutine(_resultUI.HideUI());
        OnShowCalcPathEnd?.Invoke();
    }
    
    private void ChangePoints(FishType type, int points, string calcPathString)
    {
        switch (type)
        {
            case FishType.Addition:
                IncreasePoints(points);
                break;
            case FishType.Subtraction:
                SubstractPoints(points);
                break;
            case FishType.Multiplication:
                MultiplyPoints(points);
                break;
            default:
                break;
        }

        _numbersUI.ChangeCalculationPath(calcPathString);
    }

    private void IncreasePoints(int points)
    {
        _currPoints += points;
    }

    private void SubstractPoints(int points)
    {
        _currPoints -= points;
    }

    private void MultiplyPoints(int points)
    {
        _currPoints *= points;
    }

    private string GetStringSequenceForCalculationPath(bool endResult)
    {
        bool firstCalc = true;
        string calcPathString = null;
        
        foreach (var calc in _calculationPath)
        {
            if (firstCalc)
            {
                firstCalc = false;
                calcPathString = calc.Item2.ToString();
                continue;
            }

            switch (calc.Item1)
            {
                case FishType.Addition:
                    calcPathString += " + " + calc.Item2;
                    break;
                case FishType.Subtraction:
                    calcPathString += " - " + calc.Item2;
                    break;
                case FishType.Multiplication:
                    calcPathString += " * " + calc.Item2;
                    break;
                default:
                    break;
            }
        }

        calcPathString ??= "0";

        if (endResult)
        {
            calcPathString += " = " + _currPoints;
        }

        return calcPathString;
    }

    public void SubmitScore(Button submitBtn)
    {
        if (_currPoints == _currentLevelData.goalNumber) StartCoroutine(HandleCorrectSubmit());
        else StartCoroutine(HandleIncorrectSubmit());

    }

    private IEnumerator HandleIncorrectSubmit()
    {
        OnIncorrectScoreSubmitted?.Invoke();
        _numbersUI.ChangeSubmitButton(false);
        yield return StartCoroutine(ResultSequence(false));
        _numbersUI.ResetSubmitButton();
    }

    private IEnumerator HandleCorrectSubmit()
    {
        _numbersUI.ChangeSubmitButton(true);
        yield return StartCoroutine(ResultSequence(true));
        OnLevelCompleted?.Invoke();
    }
}
