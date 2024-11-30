using UnityEngine;

public enum LevelType
{
    Addition,
    Subtraction,
    Multiplication,
    Combined
}

public enum LevelDifficulty
{
    Easy,
    Medium,
    Hard
}

[CreateAssetMenu(fileName = "NewLevelVariation", menuName = "FishingGame/LevelVariation")]
public class LevelVariation : ScriptableObject
{
    [SerializeField] public LevelType levelType;
    [SerializeField] public LevelDifficulty levelDifficulty;
    
    [SerializeField] public Vector2Int startNumberRange;
    [SerializeField] public Vector2Int goalNumberRange;
    [SerializeField] public Vector2Int fishForCorrectPathRange;
    [SerializeField] public Vector2Int additionalFishRange;


}
