using System.Collections;
using UnityEngine;

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
    [SerializeField] private LevelData[] allLevels;
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private TouchManager touchManager;
    [SerializeField] private ScreenFade screenFade;
    private int _currentLevelIndex = 0;

    private void OnEnable()
    {
        levelManager.OnLevelCompleted += OnLevelCompletedHandler;
        levelManager.OnCatchSequenceStart += DisablePlayerActions;
        levelManager.OnCatchSequenceEnd += EnablePlayerActions;
    }

    private void OnDisable()
    {
        levelManager.OnLevelCompleted -= OnLevelCompletedHandler;
        levelManager.OnCatchSequenceStart -= DisablePlayerActions;
        levelManager.OnCatchSequenceEnd -= EnablePlayerActions;
    }

    void Start()
    {
        LoadFirstLevel();
    }
    
    private void OnLevelCompletedHandler()
    {
        StartCoroutine(LevelCompletionSequence());
    }

    private void LoadFirstLevel()
    {
        _currentLevelIndex = 0;
        levelManager.InitializeLevel(allLevels[_currentLevelIndex]);
    }

    private IEnumerator LevelCompletionSequence()
    {
        DisablePlayerActions();
        
        yield return StartCoroutine(screenFade.FadeToBlack());
        yield return new WaitForSeconds(1);
        LoadNextLevel();
        yield return StartCoroutine(screenFade.FadeFromBlack());
        
        EnablePlayerActions();
    }
    
    private void LoadNextLevel()
    {
        if (_currentLevelIndex < allLevels.Length - 1)
        {
            _currentLevelIndex++;
            levelManager.InitializeLevel(allLevels[_currentLevelIndex]);
        }
        else
        {
            Debug.Log("All levels completed!");
        }
    }
    
    public void RestartLevel()
    {
        levelManager.StopAllCoroutines();
        StartCoroutine(RestartLevelSequence());
    }

    private IEnumerator RestartLevelSequence()
    {
        DisablePlayerActions();
        
        yield return StartCoroutine(screenFade.FadeToBlack());
        yield return new WaitForSeconds(1);
        levelManager.InitializeLevel(allLevels[_currentLevelIndex]);
        yield return StartCoroutine(screenFade.FadeFromBlack());
        
        EnablePlayerActions();
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
