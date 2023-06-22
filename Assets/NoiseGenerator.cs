using UnityEngine;

public static class NoiseGenerator
{
    public static float[,] GenerateNoiseMap(int chunkSize, float scale, Wave[] waves, Vector2 offset)
    {
        float[,] noiseMap = new float[chunkSize, chunkSize];

        for (int x = 0; x < chunkSize; ++x)
        {
            for (int y = 0; y < chunkSize; ++y)
            {
                float samplePosX = offset.x + x * scale;
                float samplePosY = offset.y + y * scale;

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
        return noiseMap;
    }
}
