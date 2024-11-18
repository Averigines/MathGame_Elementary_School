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
    public Vector2 spawnPos; //in percent from left/top
    public float speedX;
    public Sprite sprite;
}
