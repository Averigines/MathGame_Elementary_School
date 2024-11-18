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
    public LevelData[] allLevels;
    private int _currentLevelIndex = 0;

    public void LoadNextLevel()
    {
        
    }
    void Start()
    {
        
    }

    void Update()
    {
    }
}
