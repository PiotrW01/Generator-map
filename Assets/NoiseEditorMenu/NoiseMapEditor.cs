using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NoiseMapEditor : MonoBehaviour
{
    public NoiseMap noiseMapID;
    public GameObject WaveEditorPrefab;
    private Wave[] waves;
    private List<GameObject> waveList;

    public void AddWave()
    {
        GameObject newChild = Instantiate(WaveEditorPrefab);
        newChild.transform.Find("FreqSlider").GetComponent<Slider>().value = 0;
        newChild.transform.Find("AmpSlider").GetComponent<Slider>().value = 0;
        newChild.transform.Find("WaveNumber").GetComponent<TextMeshProUGUI>().text = "#" + (waveList.Count + 1);

        var waveEditor = newChild.transform.GetComponent<WaveEditor>();
        waveEditor.waveID = waveList.Count;
        waveEditor.noiseType = noiseMapID;

        newChild.transform.Find("Button").GetComponent<Button>().onClick.AddListener(() => RemoveWave(newChild));
        newChild.transform.SetParent(transform, false);
        waveList.Add(newChild);
    }

    public void LoadWaves()
    {
        switch (noiseMapID)
        {
            case NoiseMap.Continentality:
                waves = WaveManager.Instance.CWaves;
                break;
            case NoiseMap.Height:
                waves = WaveManager.Instance.HWaves;
                break;
            case NoiseMap.Temperature:
                waves = WaveManager.Instance.TWaves;
                break;
            case NoiseMap.Humidity:
                waves = WaveManager.Instance.HMWaves;
                break;
        }

        waveList = new List<GameObject>(waves.Length);
        int index = 0;

        foreach (var wave in waves)
        {
            GameObject newChild = Instantiate(WaveEditorPrefab);
            newChild.transform.Find("FreqSlider").GetComponent<Slider>().value = wave.frequency;
            newChild.transform.Find("AmpSlider").GetComponent<Slider>().value = wave.amplitude;
            newChild.transform.Find("WaveNumber").GetComponent<TextMeshProUGUI>().text = "#" + (index + 1);
            
            var waveEditor = newChild.transform.GetComponent<WaveEditor>();
            waveEditor.waveID = index;
            waveEditor.noiseType = noiseMapID;

            newChild.transform.Find("Button").GetComponent<Button>().onClick.AddListener(() => RemoveWave(newChild));
            newChild.transform.SetParent(transform, false);

            waveList.Add(newChild);
            index++;
        }
    }

    public void RemoveWave(GameObject gm)
    {
        int i = waveList.FindIndex(obj => obj == gm);
        int index = 1;

        foreach (GameObject wave in waveList.GetRange(i + 1, waveList.Count - i - 1))
        {
            wave.transform.Find("WaveNumber").GetComponent<TextMeshProUGUI>().text = "#" + (i + index);
            wave.transform.GetComponent<WaveEditor>().waveID = i + index - 1;
            index++;
        }
        waveList.RemoveAt(i);
        Destroy(gm);
    }

    public void DeleteWaves()
    {
        foreach (GameObject wave in waveList)
        {
            Destroy(wave);
        }
        waveList.Clear();
    }

    public void UnloadWaves()
    {

    }
}
