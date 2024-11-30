using UnityEngine;

public enum FishType
{
    Addition,
    Subtraction,
    Multiplication,
    Division
}

[System.Serializable]
public class FishData
{
    public FishType type;
    public int amount;
    [Range(0, 1)] public float swimEdgeLeft;
    [Range(0, 1)] public float swimEdgeRight;
    [Range(0, 1)] public float spawnPosY;
    public float speedX;
}
