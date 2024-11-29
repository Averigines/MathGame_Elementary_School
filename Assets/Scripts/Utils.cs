using UnityEngine;

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
}