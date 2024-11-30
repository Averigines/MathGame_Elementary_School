using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum DirectionX
{
    Left,
    Right
}

public enum DirectionY
{
    Up,
    Down
}

public class GameManager : MonoBehaviour
{
    [SerializeField] private ScreenManager screenManager;
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private TouchManager touchManager;
    [SerializeField] private ScreenFade screenFade;
    
    [SerializeField] private LevelVariation[] allLevelVariations;
    private List<LevelVariation> _possibleLevelVariations;
    private LevelType[] _levelCycle;
    private int _currentCycleIndex = 0;
    private LevelVariation _currentLevelVariation;
    private LevelData _currentLevel;

    private Dictionary<LevelType, LevelDifficulty> _currentDifficultyPerType = new Dictionary<LevelType, LevelDifficulty>
    {
        { LevelType.Addition, LevelDifficulty.Easy },
        { LevelType.Subtraction, LevelDifficulty.Easy },
        { LevelType.Multiplication, LevelDifficulty.Easy },
        { LevelType.Combined, LevelDifficulty.Easy }
    };

    private int _currentRestarts = 0;

    private void OnEnable()
    {
        levelManager.OnLevelCompleted += OnLevelCompletedHandler;
        levelManager.OnShowCalcPathStart += DisablePlayerActions;
        levelManager.OnShowCalcPathEnd += EnablePlayerActions;
        levelManager.OnIncorrectScoreSubmitted += HandleIncorrectScoreSubmit;
    }

    private void OnDisable()
    {
        levelManager.OnLevelCompleted -= OnLevelCompletedHandler;
        levelManager.OnShowCalcPathStart -= DisablePlayerActions;
        levelManager.OnShowCalcPathEnd -= EnablePlayerActions;
        levelManager.OnIncorrectScoreSubmitted -= HandleIncorrectScoreSubmit;
    }

    void Start()
    {
        AssignPossibleLevelVariations();
        AssignLevelCycle();
        AssignNextLevelVariation();

        LoadFirstLevel();
    }

    private void AssignNextLevelVariation()
    {
        var currentLevelType = _levelCycle[_currentCycleIndex % _levelCycle.Length];
        _currentLevelVariation = _possibleLevelVariations.Find(v =>
            v.levelType == currentLevelType && v.levelDifficulty == _currentDifficultyPerType[currentLevelType]);
    }

    private void AssignLevelCycle()
    {
        _levelCycle = new LevelType[ScenePersistentData.ChosenLevelTypes.Count];
        for (int i = 0; i < _levelCycle.Length; i++)
        {
            _levelCycle[i] = ScenePersistentData.ChosenLevelTypes[i];
        }
    }

    private void AssignPossibleLevelVariations()
    {
        _possibleLevelVariations = new List<LevelVariation>();
        foreach (var variation in allLevelVariations)
        {
            if (ScenePersistentData.ChosenLevelTypes.Contains(variation.levelType))
            {
                _possibleLevelVariations.Add(variation);
            }
        }
    }

    private void OnLevelCompletedHandler()
    {
        AssignNextDifficulty();
        _currentRestarts = 0;
        _currentCycleIndex++;
        AssignNextLevelVariation();
        
        StartCoroutine(LevelCompletionSequence());
    }

    private void AssignNextDifficulty()
    {
        LevelDifficulty currDifficulty = _currentLevelVariation.levelDifficulty;
        switch (_currentRestarts)
        {
            case 0 when currDifficulty != LevelDifficulty.Hard:
                _currentDifficultyPerType[_currentLevelVariation.levelType] = IncreaseNextDifficulty(currDifficulty);
                break;
            case > 3 when currDifficulty != LevelDifficulty.Easy:
                _currentDifficultyPerType[_currentLevelVariation.levelType] = DecreaseNextDifficulty(currDifficulty);
                break;
        }
    }

    private LevelDifficulty IncreaseNextDifficulty(LevelDifficulty current)
    {
        switch (current)
        {
            case LevelDifficulty.Easy: return LevelDifficulty.Medium;
            case LevelDifficulty.Medium: return LevelDifficulty.Hard;
            default: return LevelDifficulty.Hard;
        }
    }
    
    private LevelDifficulty DecreaseNextDifficulty(LevelDifficulty current)
    {
        switch (current)
        {
            case LevelDifficulty.Medium: return LevelDifficulty.Easy;
            case LevelDifficulty.Hard: return LevelDifficulty.Medium;
            default: return LevelDifficulty.Easy;
        }
    }

    private void LoadFirstLevel()
    {
        _currentLevel = LevelGenerator.GenerateLevel(_currentLevelVariation);
        levelManager.InitializeLevel(_currentLevel);
    }

    private IEnumerator LevelCompletionSequence()
    {
        DisablePlayerActions();
        
        yield return StartCoroutine(screenFade.FadeToBlack());
        yield return new WaitForSeconds(1);
        LoadNextLevel();
        yield return StartCoroutine(screenFade.FadeFromBlack());
    }
    
    private void LoadNextLevel()
    {
        _currentLevel = LevelGenerator.GenerateLevel(_currentLevelVariation);
        levelManager.InitializeLevel(_currentLevel);
    }
    
    private void HandleIncorrectScoreSubmit()
    {
        _currentRestarts++;
    }

    public void RestartLevel(Button restartBtn)
    {
        restartBtn.enabled = false;
        _currentRestarts++;
        
        levelManager.StopAllCoroutines();
        StartCoroutine(RestartLevelSequence(restartBtn));
    }

    private IEnumerator RestartLevelSequence(Button restartBtn)
    {
        DisablePlayerActions();
        
        yield return StartCoroutine(screenFade.FadeToBlack());
        yield return new WaitForSeconds(1);
        levelManager.InitializeLevel(_currentLevel);
        yield return StartCoroutine(screenFade.FadeFromBlack());
        
        EnablePlayerActions();
        restartBtn.enabled = true;
    }

    public void GoToMenu()
    {
        StartCoroutine(GoToMenuSequence());
    }

    private IEnumerator GoToMenuSequence()
    {
        DisablePlayerActions();
        yield return StartCoroutine(screenFade.FadeToBlack());

        SceneManager.LoadScene(0);
    }


    private void DisablePlayerActions()
    {
        touchManager.gameObject.SetActive(false);
    }

    private void EnablePlayerActions()
    {
        touchManager.gameObject.SetActive(true);
    }
}
