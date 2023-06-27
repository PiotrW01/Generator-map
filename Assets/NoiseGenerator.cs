using System;
using UnityEngine;

public static class NoiseGenerator
{
    public static float[,] GenerateNoiseMap(int chunkSize, float scale, Wave[] waves, Vector2 chunkStartPos, int seed)
    {
        float[,] noiseMap = new float[chunkSize, chunkSize];
        if (waves.Length == 0) return noiseMap;

        UnityEngine.Random.InitState(seed);

        int offsetX = UnityEngine.Random.Range(0, 99999);
        int offsetY = UnityEngine.Random.Range(0, 99999);

        for (int x = 0; x < chunkSize; x++)
        {
            for (int y = 0; y < chunkSize; y++)
            {
                float samplePosX = (chunkStartPos.x + x) * scale + offsetX;
                float samplePosY = (chunkStartPos.y + y) * scale + offsetY;

                float normalization = 0.0f;
                foreach (Wave wave in waves)
                {
                    noiseMap[x, y] += wave.amplitude * Mathf.PerlinNoise(samplePosX * wave.frequency
                                    + wave.seed, samplePosY * wave.frequency + wave.seed);
                    normalization += wave.amplitude;
                }
                noiseMap[x, y] /= normalization;
            }
        }

/*        // After generating the noiseMap
        float minValue = float.MaxValue;
        float maxValue = float.MinValue;

        // Find the minimum and maximum values
        for (int x = 0; x < chunkSize; x++)
        {
            for (int y = 0; y < chunkSize; y++)
            {
                float value = noiseMap[x, y];
                minValue = Mathf.Min(minValue, value);
                maxValue = Mathf.Max(maxValue, value);
            }
        }

        // Normalize the values using min-max scaling
        float range = maxValue - minValue;
        for (int x = 0; x < chunkSize; x++)
        {
            for (int y = 0; y < chunkSize; y++)
            {
                noiseMap[x, y] = (noiseMap[x, y] - minValue) / range;
            }
        }*/


        return noiseMap;
    }
}
