using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NoiseMapEditor : MonoBehaviour
{
    public NoiseMap noiseMapID;
    //public int NoiseMapID = 0;
    public GameObject WaveEditorPrefab;
    private Wave[] waves;
    private List<Wave> waveList;

    public void AddWave()
    {
        // Rewrite
        GameObject newChild = Instantiate(WaveEditorPrefab);
        newChild.transform.SetParent(transform, false);
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

        waveList = new List<Wave>(waves);
        int index = 0;

        Debug.Log(waves[0].frequency);
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
            index++;
        }
    }

    public void RemoveWave(GameObject gm)
    {
        Destroy(gm);
    }
}
