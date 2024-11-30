using System.Collections.Generic;
using UnityEngine;

public static class ScenePersistentData
{
    public static List<LevelType> ChosenLevelTypes { private set; get; } = new List<LevelType>();

    public static void SetChosenLevelTypes(bool addition, bool subtraction, bool multiplication, bool combined)
    {
        ChosenLevelTypes.Clear();
        
        if (addition) ChosenLevelTypes.Add(LevelType.Addition);
        if (subtraction) ChosenLevelTypes.Add(LevelType.Subtraction);
        if (multiplication) ChosenLevelTypes.Add(LevelType.Multiplication);
        if (combined) ChosenLevelTypes.Add(LevelType.Combined);
    }
}
