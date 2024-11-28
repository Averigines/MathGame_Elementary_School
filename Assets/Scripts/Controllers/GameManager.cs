using System.Collections;
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
    [SerializeField] private LevelData[] allLevels;
    [SerializeField] private ScreenManager screenManager;
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private TouchManager touchManager;
    [SerializeField] private ScreenFade screenFade;
    private int _currentLevelIndex = 0;

    private void OnEnable()
    {
        levelManager.OnLevelCompleted += OnLevelCompletedHandler;
        levelManager.OnResultSequenceStart += DisablePlayerActions;
        levelManager.OnResultSequenceEnd += EnablePlayerActions;
    }

    private void OnDisable()
    {
        levelManager.OnLevelCompleted -= OnLevelCompletedHandler;
        levelManager.OnResultSequenceStart -= DisablePlayerActions;
        levelManager.OnResultSequenceEnd -= EnablePlayerActions;
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

    public void RestartLevel(Button restartBtn)
    {
        restartBtn.enabled = false;
        
        levelManager.StopAllCoroutines();
        StartCoroutine(RestartLevelSequence(restartBtn));
    }

    private IEnumerator RestartLevelSequence(Button restartBtn)
    {
        DisablePlayerActions();
        
        yield return StartCoroutine(screenFade.FadeToBlack());
        yield return new WaitForSeconds(1);
        levelManager.InitializeLevel(allLevels[_currentLevelIndex]);
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
