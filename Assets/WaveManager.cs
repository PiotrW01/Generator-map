using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;

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
        // Set default 
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

        ResetToDefault();
        try
        {
            var jsonArray = PlayerPrefs.GetString("presetNames");
            PresetWrapper presetWrapper = JsonUtility.FromJson<PresetWrapper>(jsonArray);
            presetNames = presetWrapper.presetNames;
        } catch
        {
            Debug.Log("No saved presets.");
        }
    }

    public void ResetToDefault()
    {
        CLayers = Copy(defaultCLayers);
        HLayers = Copy(defaultHLayers);
        TLayers = Copy(defaultTLayers);
        HMLayers = Copy(defaultHMLayers);
    }

    public List<Layer> Copy(Layer[] layers)
    {
        List<Layer> copy = new List<Layer>();
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
        LayerWrapper layerWrapper = new()
        {
            layers = CLayers
        };
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

        if (!presetName.Contains(presetName))
        {
            presetNames.Add(presetName);
        }
        PresetWrapper presetWrapper = new() { presetNames = presetNames};
        jsonArray = JsonUtility.ToJson(presetWrapper, true);
        PlayerPrefs.SetString("presetNames", jsonArray);
        Debug.Log(jsonArray);
        PlayerPrefs.Save();
    }

    public void RemovePreset(string presetName)
    {
        PlayerPrefs.DeleteKey(presetName + "_C");
        PlayerPrefs.DeleteKey(presetName + "_H");
        PlayerPrefs.DeleteKey(presetName + "_T");
        PlayerPrefs.DeleteKey(presetName + "_HM");

        presetNames.Remove(presetName);
        PresetWrapper presetWrapper = new() { presetNames = presetNames };
        var jsonArray = JsonUtility.ToJson(presetWrapper, true);
        PlayerPrefs.SetString("presetNames", jsonArray);
        Debug.Log(jsonArray);
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
        return "freq: " + frequency + " amp:" + amplitude;
    }
}

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