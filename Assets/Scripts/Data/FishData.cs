using UnityEngine;
using UnityEngine.Serialization;

public enum FishType
{
    Addition,
    Substraction,
    Multiplication,
    Division
}

[System.Serializable]
public class FishData
{
    public FishType type;
    public int amount;
    public float swimEdgeLeft; //in percent
    public float swimEdgeRight; //in percent
    public float spawnPosY; //in percent from top
    public float speedX;
    public GameObject prefab;
}
