using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public LevelData currentLevelData;

    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject fishClusterContainerPrefab;
    [SerializeField] private GameObject fishPrefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (currentLevelData != null)
        {
            InitializeLevel();
        }
        else
        {
            Debug.LogError("No level data assigned!");
        }
    }

    private void InitializeLevel()
    {
        Debug.Log($"Starting Level {currentLevelData.levelNumber}");
        Debug.Log($"Goal Number: {currentLevelData.goalNumber}");

        // Spawn fish based on the level data
        foreach (FishData fish in currentLevelData.availableFish)
        {
            SpawnFishCluster(fish);
        }
    }
    
    private void SpawnFishCluster(FishData fishData)
    {
        var startPos = ScreenData.GetFishSpawnPos(fishData.spawnPos);
        DirectionX directionX = ScreenData.CoinToss() ? DirectionX.Left : DirectionX.Right;
        DirectionY directionY = ScreenData.CoinToss() ? DirectionY.Up : DirectionY.Down;

        GameObject fishClusterContainer = Instantiate(fishClusterContainerPrefab, startPos, Quaternion.identity);
        fishClusterContainer.GetComponent<FishClusterContainer>().Initialize(fishData, fishPrefab, startPos.y, directionX, directionY);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
