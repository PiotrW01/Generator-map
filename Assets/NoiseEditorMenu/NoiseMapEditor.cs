using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NoiseMapEditor : MonoBehaviour
{
    public NoiseMap noiseMapID;
    public GameObject LayerEditorPrefab;
    private List<Layer> layers;
    private List<GameObject> layerEditors;

    public void AddLayer()
    {
        layers.Add(new Layer());

        GameObject editor = Instantiate(LayerEditorPrefab);
        editor.transform.Find("FreqSlider").GetComponent<Slider>().value = 0;
        editor.transform.Find("AmpSlider").GetComponent<Slider>().value = 0;
        editor.transform.Find("LayerNumber").GetComponent<TextMeshProUGUI>().text = "#" + (layerEditors.Count + 1);
        
        var layerEditor = editor.transform.GetComponent<LayerEditor>();
        layerEditor.layerID = layerEditors.Count;
        layerEditor.noiseType = noiseMapID;
        layerEditor.layerReference = layers[layers.Count - 1];

        editor.transform.Find("Button").GetComponent<Button>().onClick.AddListener(() => RemoveLayer(editor));
        editor.transform.SetParent(transform, false);
        
        layerEditors.Add(editor);
    }

    public void LoadLayers()
    {
        switch (noiseMapID)
        {
            case NoiseMap.Continentality:
                layers = LayerManager.Instance.CLayers;
                break;
            case NoiseMap.Height:
                layers = LayerManager.Instance.HLayers;
                break;
            case NoiseMap.Temperature:
                layers = LayerManager.Instance.TLayers;
                break;
            case NoiseMap.Humidity:
                layers = LayerManager.Instance.HMLayers;
                break;
        }

        layerEditors = new List<GameObject>(layers.Count);
        int index = 0;

        foreach (var layer in layers)
        {
            GameObject editor = Instantiate(LayerEditorPrefab);
            editor.transform.Find("FreqSlider").GetComponent<Slider>().value = layer.frequency;
            editor.transform.Find("AmpSlider").GetComponent<Slider>().value = layer.amplitude;
            editor.transform.Find("LayerNumber").GetComponent<TextMeshProUGUI>().text = "#" + (index + 1);
            
            var layerEditor = editor.transform.GetComponent<LayerEditor>();
            layerEditor.layerID = index;
            layerEditor.noiseType = noiseMapID;
            layerEditor.layerReference = layer;

            editor.transform.Find("Button").GetComponent<Button>().onClick.AddListener(() => RemoveLayer(editor));
            editor.transform.SetParent(transform, false);

            layerEditors.Add(editor);
            index++;
        }
    }

    public void RemoveLayer(GameObject gm)
    {
        int i = layerEditors.FindIndex(obj => obj == gm);
        int index = 1;

        foreach (GameObject layer in layerEditors.GetRange(i + 1, layerEditors.Count - i - 1))
        {
            layer.transform.Find("LayerNumber").GetComponent<TextMeshProUGUI>().text = "#" + (i + index);
            layer.transform.GetComponent<LayerEditor>().layerID = i + index - 1;
            index++;
        }
        layers.RemoveAt(i);
        layerEditors.RemoveAt(i);
        Destroy(gm);
    }

    public void UnloadLayers()
    {
        foreach (GameObject editor in layerEditors)
        {
            Destroy(editor);
        }
        layerEditors.Clear();
    }
}
