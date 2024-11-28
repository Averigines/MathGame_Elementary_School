using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private ScreenManager screenManager;
    [SerializeField] private MenuLevelManager levelManager;

    [SerializeField] private LevelData menuLevel;
    
    [SerializeField] private ScreenFade screenFade;

    private void Start()
    {
        LoadMenuLevel();
    }

    private void LoadMenuLevel()
    {
        levelManager.InitializeMenuLevel(menuLevel);
    }
    
    public void StartGame()
    {
        StartCoroutine(StartGameSequence());
    }

    private IEnumerator StartGameSequence()
    {
        yield return StartCoroutine(screenFade.FadeToBlack());

        SceneManager.LoadScene(1);
    }
}
