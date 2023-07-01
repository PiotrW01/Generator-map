using System.Collections.Generic;
using UnityEngine;

public enum NoiseMap
{
    Continentality,
    Height,
    Temperature,
    Humidity,
}

public class LayerManager : MonoBehaviour
{
    public static LayerManager Instance;

    private Layer[] defaultHLayers;
    private Layer[] defaultCLayers;
    private Layer[] defaultTLayers;
    private Layer[] defaultHMLayers;

    public List<string> presetNames;
    public List<Layer> CLayers;
    public List<Layer> HLayers;
    public List<Layer> TLayers;
    public List<Layer> HMLayers;


    private void Awake()
    {
        // Make singleton instance
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        // Load presetNames if they exist
        try
        {
            var jsonArray = PlayerPrefs.GetString("presetNames");
            PresetWrapper presetWrapper = JsonUtility.FromJson<PresetWrapper>(jsonArray);
            presetNames = presetWrapper.presetNames;
        }
        catch
        {
            Debug.Log("No saved presets.");
        }
    }

    private void Start()
    {
        // Set default values
        defaultCLayers = new Layer[]
        {
            new Layer(0.01f, 1f),
            new Layer(0.0363f, 0.09f)
        };

        defaultHLayers = new Layer[] {
            new Layer( 0.0134f, 0.37f),
            new Layer(0.0387f, 0.61f),
        };

        defaultTLayers = new Layer[]
        {
            new Layer(0.005f, 0.4f),
            new Layer(0.0276f, 0.09f)
        };

        defaultHMLayers = new Layer[]
        {
            new Layer(0.06f, 0.75f),
            new Layer(0.013f, 0.07f),
            new Layer( 0.013f, 0.17f),
        };

        // Assign default values to layers
        ResetToDefault();
    }

    public void ResetToDefault()
    {
        CLayers = Copy(defaultCLayers);
        HLayers = Copy(defaultHLayers);
        TLayers = Copy(defaultTLayers);
        HMLayers = Copy(defaultHMLayers);
    }

    // Return a layer list with copied values from a layer array
    public List<Layer> Copy(Layer[] layers)
    {
        List<Layer> copy = new();
        foreach (var layer in layers)
        {
            copy.Add(new Layer(layer.frequency, layer.amplitude));
        }
        return copy;
    }

    public void LoadPreset(string presetName)
    {
        string jsonArray;
        LayerWrapper layerWrapper;

        try {
            jsonArray = PlayerPrefs.GetString(presetName + "_C");
            layerWrapper = JsonUtility.FromJson<LayerWrapper>(jsonArray);
            CLayers = layerWrapper.layers;

            jsonArray = PlayerPrefs.GetString(presetName + "_H");
            layerWrapper = JsonUtility.FromJson<LayerWrapper>(jsonArray);
            HLayers = layerWrapper.layers;

            jsonArray = PlayerPrefs.GetString(presetName + "_T");
            layerWrapper = JsonUtility.FromJson<LayerWrapper>(jsonArray);
            TLayers = layerWrapper.layers;

            jsonArray = PlayerPrefs.GetString(presetName + "_HM");
            layerWrapper = JsonUtility.FromJson<LayerWrapper>(jsonArray);
            HMLayers = layerWrapper.layers;
        } catch
        {
            Debug.Log("Failed to load preset");
        }
    }

    public void SavePreset(string presetName)
    {
        string jsonArray;
        
        LayerWrapper layerWrapper = new() { layers = CLayers };
        jsonArray = JsonUtility.ToJson(layerWrapper, true);
        PlayerPrefs.SetString(presetName + "_C", jsonArray);

        layerWrapper.layers = HLayers;
        jsonArray = JsonUtility.ToJson(layerWrapper, true);
        PlayerPrefs.SetString(presetName + "_H", jsonArray);

        layerWrapper.layers = TLayers;
        jsonArray = JsonUtility.ToJson(layerWrapper, true);
        PlayerPrefs.SetString(presetName + "_T", jsonArray);

        layerWrapper.layers = HMLayers;
        jsonArray = JsonUtility.ToJson(layerWrapper, true);
        PlayerPrefs.SetString(presetName + "_HM", jsonArray);

        // If preset name doesn't exist add it as a new one
        if (!presetName.Contains(presetName))
        {
            presetNames.Add(presetName);
        }

        PresetWrapper presetWrapper = new() { presetNames = presetNames};
        jsonArray = JsonUtility.ToJson(presetWrapper, true);
        PlayerPrefs.SetString("presetNames", jsonArray);
        PlayerPrefs.Save();
    }

    public void RemovePreset(string presetName)
    {
        PlayerPrefs.DeleteKey(presetName + "_C");
        PlayerPrefs.DeleteKey(presetName + "_H");
        PlayerPrefs.DeleteKey(presetName + "_T");
        PlayerPrefs.DeleteKey(presetName + "_HM");
        presetNames.Remove(presetName);

        // Save presetNames without the removed preset
        PresetWrapper presetWrapper = new() { presetNames = presetNames };
        var jsonArray = JsonUtility.ToJson(presetWrapper, true);
        PlayerPrefs.SetString("presetNames", jsonArray);
        PlayerPrefs.Save();
    }
}

[System.Serializable]
public class Layer
{
    [Range(0.005f, 0.06f)]
    public float frequency;
    [Range(0.005f, 1f)]
    public float amplitude;
    public Layer(float frequency = 0.005f, float amplitude = 0.005f)
    {
        this.frequency = frequency;
        this.amplitude = amplitude;
    }

    public override string ToString()
    {
        return "Freq: " + frequency + " Amp:" + amplitude;
    }
}

// Wrappers to help save presets in PlayerPrefs
[System.Serializable]
public class LayerWrapper
{
    public List<Layer> layers;
}

[System.Serializable]
public class PresetWrapper
{
    public List<string> presetNames;
}