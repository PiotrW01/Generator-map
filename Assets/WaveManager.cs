using UnityEngine;

public enum NoiseMap
{
    Continentality,
    Height,
    Temperature,
    Humidity,
}

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance;

    public Wave[] heightWaves;
    public Wave[] continentalityWaves;
    public Wave[] temperatureWaves;
    public Wave[] humidityWaves;
    int tempSeed;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        tempSeed = ChunkLoader.Instance.seed;
        Random.InitState(tempSeed);

        heightWaves = new Wave[] {
            new Wave(Random.Range(0, 9999), 0.0134f, 0.37f),
            new Wave(Random.Range(0, 9999), 0.0387f, 0.61f),
        };

        continentalityWaves = new Wave[]
        {
            new Wave(Random.Range(0, 9999), 0.01f, 1f),
            new Wave(Random.Range(0, 9999), 0.0363f, 0.09f)
        };

        temperatureWaves = new Wave[]
        {
            new Wave(Random.Range(0, 9999), 0.005f, 0.4f),
            new Wave(Random.Range(0, 9999), 0.0276f, 0.09f)
        };

        humidityWaves = new Wave[]
        {
            new Wave(Random.Range(0, 9999), 0.06f, 0.75f),
            new Wave(Random.Range(0, 9999), 0.013f, 0.07f),
            new Wave(Random.Range(0, 9999), 0.013f, 0.17f),
        };
    }

    private void Update()
    {
        if (tempSeed != ChunkLoader.Instance.seed) UpdateWaveSeeds();
    }

    public void UpdateWaveSeeds()
    {
        tempSeed = ChunkLoader.Instance.seed;
        Random.InitState(tempSeed);

        foreach (var wave in continentalityWaves)
        {
            wave.seed = Random.Range(0, 9999);
        }
        foreach (var wave in heightWaves)
        {
            wave.seed = Random.Range(0, 9999);
        }
        foreach (var wave in humidityWaves)
        {
            wave.seed = Random.Range(0, 9999);
        }
        foreach (var wave in temperatureWaves)
        {
            wave.seed = Random.Range(0, 9999);
        }
    }
}

[System.Serializable]
public class Wave
{
    public int seed;
    [Range(0.005f, 0.06f)]
    public float frequency;
    [Range(0.005f, 1f)]
    public float amplitude;
    public Wave(int seed, float frequency, float amplitude)
    {
        this.seed = seed;
        this.frequency = frequency;
        this.amplitude = amplitude;
    }
}
