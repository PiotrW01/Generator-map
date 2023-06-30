using System.Collections.Generic;
using System.IO;
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

    private Wave[] defaultHWaves;
    private Wave[] defaultCWaves;
    private Wave[] defaultTWaves;
    private Wave[] defaultHMWaves;

    public string[] presetNames;
    public Wave[] HWaves;
    public Wave[] CWaves;
    public Wave[] TWaves;
    public Wave[] HMWaves;
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

        defaultHWaves = new Wave[] {
            new Wave(Random.Range(0, 9999), 0.0134f, 0.37f),
            new Wave(Random.Range(0, 9999), 0.0387f, 0.61f),
        };

        defaultCWaves = new Wave[]
        {
            new Wave(Random.Range(0, 9999), 0.01f, 1f),
            new Wave(Random.Range(0, 9999), 0.0363f, 0.09f)
        };

        defaultTWaves = new Wave[]
        {
            new Wave(Random.Range(0, 9999), 0.005f, 0.4f),
            new Wave(Random.Range(0, 9999), 0.0276f, 0.09f)
        };

        defaultHMWaves = new Wave[]
        {
            new Wave(Random.Range(0, 9999), 0.06f, 0.75f),
            new Wave(Random.Range(0, 9999), 0.013f, 0.07f),
            new Wave(Random.Range(0, 9999), 0.013f, 0.17f),
        };


        CWaves = defaultCWaves;
        HWaves = defaultHWaves;
        TWaves = defaultTWaves;
        HMWaves = defaultHMWaves;
        try
        {
            presetNames = JsonHelper.FromJson<string>("presetNames");
        } catch
        {
            presetNames = new string[10];
            Debug.Log("No saved presets.");
        }
}

    private void Update()
    {
        if (tempSeed != ChunkLoader.Instance.seed) UpdateWaveSeeds();
    }

    public void UpdateWaveSeeds()
    {
        tempSeed = ChunkLoader.Instance.seed;
        Random.InitState(tempSeed);

        foreach (var wave in defaultCWaves)
        {
            wave.seed = Random.Range(0, 9999);
        }
        foreach (var wave in defaultHWaves)
        {
            wave.seed = Random.Range(0, 9999);
        }
        foreach (var wave in defaultHMWaves)
        {
            wave.seed = Random.Range(0, 9999);
        }
        foreach (var wave in defaultTWaves)
        {
            wave.seed = Random.Range(0, 9999);
        }
    }

    public void ResetToDefault()
    {
        HWaves = defaultHWaves;
        CWaves = defaultCWaves;
        TWaves = defaultTWaves;
        HMWaves = defaultHMWaves;
    }

    public void LoadPreset(int i)
    {
        string jsonArray;
        try {
        jsonArray = PlayerPrefs.GetString(presetNames[i] + "_C");
        CWaves = JsonHelper.FromJson<Wave>(jsonArray);        
        jsonArray = PlayerPrefs.GetString(presetNames[i] + "_H");
        HWaves = JsonHelper.FromJson<Wave>(jsonArray);
        jsonArray = PlayerPrefs.GetString(presetNames[i] + "_T");
        TWaves = JsonHelper.FromJson<Wave>(jsonArray);
        jsonArray = PlayerPrefs.GetString(presetNames[i] + "_HM");
        HMWaves = JsonHelper.FromJson<Wave>(jsonArray);
        } catch
        {
            Debug.Log("Failed to load preset");
        }
    }

    public void SavePreset(string presetName)
    {
        string jsonArray;
        jsonArray = JsonHelper.ToJson(CWaves, true);
        PlayerPrefs.SetString(presetName + "_C", jsonArray);
        jsonArray = JsonHelper.ToJson(HWaves, true);
        PlayerPrefs.SetString(presetName + "_H", jsonArray);
        jsonArray = JsonHelper.ToJson(TWaves, true);
        PlayerPrefs.SetString(presetName + "_T", jsonArray);
        jsonArray = JsonHelper.ToJson(HMWaves, true);
        PlayerPrefs.SetString(presetName + "_HM", jsonArray);

        for (int i = 0; i < presetNames.Length; i++)
        {
            if (presetNames[i] == null || presetNames[i] == presetName)
            {
                presetNames[i] = presetName;
                break;
            }
        }
        jsonArray = JsonHelper.ToJson(presetNames, true);
        PlayerPrefs.SetString("presetNames", jsonArray);

        PlayerPrefs.Save();
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

    public override string ToString()
    {
        return "seed: " + seed + " freq: " + frequency + " amp:" + amplitude;
    }
}
