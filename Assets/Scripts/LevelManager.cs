using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public LevelData currentLevelData;

    [SerializeField] private GameManager gameManager;
    [SerializeField] private Player player;
    [SerializeField] private GameObject fishClusterContainerPrefab;

    private int _currPoints;
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
        var fishAreaX = ScreenData.GetFishAreaX(fishData.swimEdgeLeft, fishData.swimEdgeRight);
        var startPos = ScreenData.GetFishSpawnPos(fishData.spawnPosY, fishAreaX);
        DirectionX directionX = ScreenData.CoinToss() ? DirectionX.Left : DirectionX.Right;
        DirectionY directionY = ScreenData.CoinToss() ? DirectionY.Up : DirectionY.Down;
        GameObject go = Instantiate(fishClusterContainerPrefab, startPos, Quaternion.identity);
        
        var fishCluster = go.GetComponent<FishClusterContainer>();
        fishCluster.Initialize(fishData, startPos.y, fishAreaX, directionX, directionY);
        fishCluster.onFishReeledIn += HandleFishReeledIn;
    }

    private void HandleFishReeledIn(int points)
    {
        IncreasePoints(points);
        player.ChangePlayerState(Player.PlayerState.Idle);
    }

    private void IncreasePoints(int points)
    {
        _currPoints += points;
        print(_currPoints);
        
        if (_currPoints == currentLevelData.goalNumber) print("WIN");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
