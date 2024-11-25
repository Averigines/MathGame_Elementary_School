using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.PlayerLoop;
using Random = UnityEngine.Random;
using TouchPhase = UnityEngine.TouchPhase;

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
    [SerializeField] private ScreenFade screenFade;
    private int _currentLevelIndex = 0;

    private void OnEnable()
    {
        levelManager.onLevelCompleted += OnLevelCompletedHandler;
    }

    private void OnDisable()
    {
        levelManager.onLevelCompleted -= OnLevelCompletedHandler;
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
        yield return StartCoroutine(screenFade.FadeToBlack());

        yield return new WaitForSeconds(1);
        
        LoadNextLevel();
        yield return StartCoroutine(screenFade.FadeFromBlack());
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
}
