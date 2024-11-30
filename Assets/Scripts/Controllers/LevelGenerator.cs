using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public static LevelData GenerateLevel(LevelVariation variation)
    {
        LevelData levelData = null;
        switch (variation.levelType)
        {
            case LevelType.Addition:
                levelData = GenerateAdditionLevel(variation);
                break;
            case LevelType.Subtraction:
                levelData = GenerateSubtractionLevel(variation);
                break;
            case LevelType.Multiplication:
                levelData = GenerateMultiplicationLevel(variation);
                break;
            case LevelType.Combined:
                //levelData = GenerateCombinedLevel(variation);
                break;
        }

        return levelData;
    }

    private static LevelData GenerateAdditionLevel(LevelVariation variation)
    {
        LevelData levelData = ScriptableObject.CreateInstance<LevelData>();

        levelData.startNumber = Utils.GetRandomNumber(variation.startNumberRange.x, variation.startNumberRange.y);
        var minGoalNumber = levelData.startNumber > variation.goalNumberRange.x
            ? levelData.startNumber + 1
            : variation.goalNumberRange.x;
        levelData.goalNumber = Utils.GetRandomNumber(minGoalNumber, variation.goalNumberRange.y);

        levelData.availableFish = GenerateFishDataAddition(FishType.Addition, levelData, variation);

        return levelData;
    }
    
    private static LevelData GenerateSubtractionLevel(LevelVariation variation)
    {
        LevelData levelData = ScriptableObject.CreateInstance<LevelData>();

        levelData.startNumber = Utils.GetRandomNumber(variation.startNumberRange.x, variation.startNumberRange.y);
        var maxGoalNumber = levelData.startNumber < variation.goalNumberRange.y
            ? levelData.startNumber - 1
            : variation.goalNumberRange.y;
        levelData.goalNumber = Utils.GetRandomNumber(variation.goalNumberRange.x, maxGoalNumber);

        levelData.availableFish = GenerateFishDataSubtraction(FishType.Subtraction, levelData, variation);

        return levelData;
    }
    
    private static LevelData GenerateMultiplicationLevel(LevelVariation variation)
    {
        LevelData levelData = ScriptableObject.CreateInstance<LevelData>();

        levelData.startNumber = Utils.GetRandomNumber(variation.startNumberRange.x, variation.startNumberRange.y, 10);
        int multFromStartToGoal;
        if (variation.levelDifficulty == LevelDifficulty.Hard)
        {
            int minMult;
            int maxMult;
            if (levelData.startNumber < 10)
            {
                minMult = 11;
                maxMult = 100 / levelData.startNumber;
                if (maxMult > variation.goalNumberRange.y) maxMult = variation.goalNumberRange.y;
            }
            else
            {
                maxMult = 100 / levelData.startNumber;
                minMult = variation.goalNumberRange.x;
            }

            multFromStartToGoal = Utils.GetRandomNumber(minMult, maxMult, 10);
        }
        else
        {
            multFromStartToGoal = Utils.GetRandomNumber(variation.goalNumberRange.x, variation.goalNumberRange.y);
        }
        levelData.goalNumber = levelData.startNumber * multFromStartToGoal;

        levelData.availableFish = GenerateFishDataMultiplication(FishType.Multiplication, levelData, variation);

        return levelData;
    }

    private static FishData[] GenerateFishDataAddition(FishType type, LevelData levelData, LevelVariation variation)
    {
        int fishForCorrectPath =
            Utils.GetRandomNumber(variation.fishForCorrectPathRange.x, variation.fishForCorrectPathRange.y);
        int totalFish = fishForCorrectPath + Utils.GetRandomNumber(variation.additionalFishRange.x, variation.additionalFishRange.y);

        int[] correctValues = Utils.GenerateRandomNumbersToGetSum(fishForCorrectPath, levelData.goalNumber - levelData.startNumber);

        int[] additionalValues = new int[totalFish - fishForCorrectPath];
        for (int i = 0; i < additionalValues.Length; i++)
        {
           additionalValues[i] = Utils.GetRandomNumber(1, levelData.goalNumber - 1);
        }

        int[] allValues = correctValues.Concat(additionalValues).ToArray();
        Utils.ShuffleArray(allValues);
        float[] allSpawnPosY = Utils.GetNumbersDistributedEvenly(allValues.Length, 0, 1, 0.1f);
        Vector2[] allSwimEdges = GetRandomSwimEdges(allValues.Length);

        List<FishData> fishList = new List<FishData>();
        for (int i = 0; i < allValues.Length; i++)
        {
            
            fishList.Add(new FishData
            {
                type = type,
                amount = allValues[i],
                swimEdgeLeft = allSwimEdges[i].x,
                swimEdgeRight = allSwimEdges[i].y,
                spawnPosY = allSpawnPosY[i],
                speedX = Utils.GetRandomNumber(0.3f, 1f),
            });
        }

        return fishList.ToArray();
    }
    
    private static FishData[] GenerateFishDataSubtraction(FishType type, LevelData levelData, LevelVariation variation)
    {
        int fishForCorrectPath =
            Utils.GetRandomNumber(variation.fishForCorrectPathRange.x, variation.fishForCorrectPathRange.y);
        int totalFish = fishForCorrectPath + Utils.GetRandomNumber(variation.additionalFishRange.x, variation.additionalFishRange.y);

        int[] correctValues = Utils.GenerateRandomNumbersToGetSum(fishForCorrectPath, levelData.startNumber - levelData.goalNumber);

        int[] additionalValues = new int[totalFish - fishForCorrectPath];
        for (int i = 0; i < additionalValues.Length; i++)
        {
            additionalValues[i] = Utils.GetRandomNumber(1, levelData.startNumber - 1);
        }

        int[] allValues = correctValues.Concat(additionalValues).ToArray();
        Utils.ShuffleArray(allValues);
        float[] allSpawnPosY = Utils.GetNumbersDistributedEvenly(allValues.Length, 0, 1, 0.1f);
        Vector2[] allSwimEdges = GetRandomSwimEdges(allValues.Length);

        List<FishData> fishList = new List<FishData>();
        for (int i = 0; i < allValues.Length; i++)
        {
            
            fishList.Add(new FishData
            {
                type = type,
                amount = allValues[i],
                swimEdgeLeft = allSwimEdges[i].x,
                swimEdgeRight = allSwimEdges[i].y,
                spawnPosY = allSpawnPosY[i],
                speedX = Utils.GetRandomNumber(0.3f, 1f),
            });
        }

        return fishList.ToArray();
    }
    
    private static FishData[] GenerateFishDataMultiplication(FishType type, LevelData levelData, LevelVariation variation)
    {
        int fishForCorrectPath = 1;
        int totalFish = fishForCorrectPath + Utils.GetRandomNumber(variation.additionalFishRange.x, variation.additionalFishRange.y);

        int correctValue = levelData.goalNumber / levelData.startNumber;

        int[] allValues = new int[totalFish];
        allValues[0] = correctValue;
        for (int i = 1; i < allValues.Length; i++)
        {
            allValues[i] = Utils.GetRandomNumber(2, variation.goalNumberRange.y);
        }
        
        Utils.ShuffleArray(allValues);
        float[] allSpawnPosY = Utils.GetNumbersDistributedEvenly(allValues.Length, 0, 1, 0.1f);
        Vector2[] allSwimEdges = GetRandomSwimEdges(allValues.Length);

        List<FishData> fishList = new List<FishData>();
        for (int i = 0; i < allValues.Length; i++)
        {
            
            fishList.Add(new FishData
            {
                type = type,
                amount = allValues[i],
                swimEdgeLeft = allSwimEdges[i].x,
                swimEdgeRight = allSwimEdges[i].y,
                spawnPosY = allSpawnPosY[i],
                speedX = Utils.GetRandomNumber(0.3f, 1f),
            });
        }

        return fishList.ToArray();
    }

    private static Vector2[] GetRandomSwimEdges(int fishAmount)
    {
        Vector2 startRange = new Vector2(0, 0.6f);
        float minLength = 0.4f;
        Vector2[] swimEdges = new Vector2[fishAmount];

        for (int i = 0; i < swimEdges.Length; i++)
        {
            float edgeLeft = Utils.GetRandomNumber(startRange.x, startRange.y);
            float edgeRight = Utils.GetRandomNumber(edgeLeft + minLength, 1);
            swimEdges[i] = new Vector2(edgeLeft, edgeRight);
        }

        return swimEdges;
    }
}
