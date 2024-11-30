using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private ScreenManager screenManager;
    [SerializeField] private MenuLevelManager levelManager;
    [SerializeField] private LevelData menuLevel;
    [SerializeField] private ScreenFade screenFade;

    [SerializeField] private Toggle includeAdditionToggle;
    [SerializeField] private Toggle includeSubstractionToggle;
    [SerializeField] private Toggle includeMultiplicationToggle;
    [SerializeField] private Toggle includeCombinedToggle;

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
        ScenePersistentData.SetChosenLevelTypes(includeAdditionToggle.isOn, includeSubstractionToggle.isOn, includeMultiplicationToggle.isOn, includeCombinedToggle.isOn);
        StartCoroutine(StartGameSequence());
    }

    private IEnumerator StartGameSequence()
    {
        yield return StartCoroutine(screenFade.FadeToBlack());

        SceneManager.LoadScene(1);
    }
}
