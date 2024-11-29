using UnityEngine;

public class MenuLevelManager : MonoBehaviour
{
    [SerializeField] private GameObject fishClusterContainerPrefab;

    public void InitializeMenuLevel(LevelData level)
    {
        foreach (FishData fish in level.availableFish)
        {
            SpawnFishCluster(fish);
        }
    }

    private void SpawnFishCluster(FishData fishData)
    {
        var fishAreaX = ScreenManager.GetFishAreaX(fishData.swimEdgeLeft, fishData.swimEdgeRight);
        var startPos = ScreenManager.GetFishSpawnPos(fishData.spawnPosY, fishAreaX);
        DirectionX directionX = Utils.CoinToss() ? DirectionX.Left : DirectionX.Right;
        DirectionY directionY = Utils.CoinToss() ? DirectionY.Up : DirectionY.Down;
        
        GameObject go = Instantiate(fishClusterContainerPrefab, startPos, Quaternion.identity);
        var fishCluster = go.GetComponent<FishClusterContainer>();
        fishCluster.Initialize(fishData, startPos.y, fishAreaX, directionX, directionY);
    }
}
