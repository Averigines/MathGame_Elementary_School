using UnityEngine;

[CreateAssetMenu(fileName = "NewLevelData", menuName = "FishingGame/LevelData")]

public class LevelData : ScriptableObject
{
    public int startNumber;
    public int goalNumber;
    public FishData[] availableFish;
}
