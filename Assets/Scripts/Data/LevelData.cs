using UnityEngine;

[CreateAssetMenu(fileName = "NewLevelData", menuName = "FishingGame/LevelData")]
public class LevelData : ScriptableObject
{
    public int levelNumber;
    public int goalNumber;
    public int timeLimit; // in seconds
    public FishData[] availableFish; // Array of fish types available in this level
    public bool allowNegativeNumbers; // For advanced levels
    public bool includeMultiplication; // For advanced levels
}
