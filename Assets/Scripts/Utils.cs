using System;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

public static class Utils
{
    public static bool CoinToss()
    {
        return Random.Range(0, 2) == 0;
    }

    public static float GetRandomNumber(float min, float max)
    {
        return Random.Range(min, max);
    }
    
    public static int GetRandomNumber(int min, int max)
    {
        return Random.Range(min, max + 1);
    }
    
    public static int GetRandomNumber(int min, int max, int exclude)
    {
        HashSet<int> validNumbers = new HashSet<int>();
        for (int i = min; i <= max; i++)
        {
            validNumbers.Add(i);
        }
        validNumbers.Remove(exclude);
        return validNumbers.Count == 0 ? GetRandomNumber(min, max) : GetRandomNumber(validNumbers);
    }
    
    public static int GetRandomNumber(int min, int max, HashSet<int> exclude)
    {
        HashSet<int> validNumbers = new HashSet<int>();
        for (int i = min; i <= max; i++)
        {
            validNumbers.Add(i);
        }

        foreach (var val in exclude)
        {
            validNumbers.Remove(val);
        }

        return validNumbers.Count == 0 ? GetRandomNumber(min, max) : GetRandomNumber(validNumbers);
    }

    public static int GetRandomNumber(HashSet<int> validNumbers)
    {
        int randomIndex = GetRandomNumber(0, validNumbers.Count - 1);
        return validNumbers.ElementAt(randomIndex);
    }

    public static int[] GenerateRandomNumbersToGetSum(int amountOfNumbers, int sum)
    {
        int[] numbers = new int[amountOfNumbers];
        int remainingSum = sum;
        
        // Generate each number except the last one
        for (int i = 0; i < amountOfNumbers - 1; i++)
        {
            // Ensure the number is reasonable (1 to remainingSum - remaining slots)
            int maxPossibleValue = remainingSum / 2; 
            numbers[i] = GetRandomNumber(1, maxPossibleValue); 
        
            remainingSum -= numbers[i];  // Reduce the remaining sum
        }

        // Assign the remaining sum to the last number
        numbers[amountOfNumbers - 1] = remainingSum; 

        return numbers;
    }

    public static float[] GetNumbersDistributedEvenly(int amountOfNums, float min, float max)
    {
        float[] result = new float[amountOfNums];

        // Calculate the step size (difference between each number)
        float step = (max - min) / (amountOfNums + 1);

        // Populate the result array with evenly distributed numbers
        for (int i = 0; i < amountOfNums; i++)
        {
            result[i] = min + (i + 1) * step;
        }

        return result;
    }
    
    public static float[] GetNumbersDistributedEvenly(int amountOfNums, float min, float max, float maxDeviation)
    {
        float[] result = new float[amountOfNums];

        // Calculate the step size (difference between each number)
        float step = (max - min) / (amountOfNums + 1);

        // Populate the result array with evenly distributed numbers
        for (int i = 0; i < amountOfNums; i++)
        {
            float deviation = GetRandomNumber(-maxDeviation, maxDeviation);
            result[i] = min + (i + 1) * step + deviation;
        }

        return result;
    }
    
    public static void ShuffleArray<T>(T[] array)
    {
        // Go through the array from the last index down to the second element
        for (int i = array.Length - 1; i > 0; i--)
        {
            // Generate a random index between 0 and i
            int j = GetRandomNumber(0, i);

            // Swap the elements at indices i and j
            (array[i], array[j]) = (array[j], array[i]);
        }
    }
}