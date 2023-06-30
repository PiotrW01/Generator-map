using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WaveEditor : MonoBehaviour
{
    public int waveID = 0;
    public NoiseMap noiseType;

    public Slider freqSlider;
    public Slider ampSlider;

    public TextMeshProUGUI freqText;
    public TextMeshProUGUI ampText;

    private Wave[] wavesReference;

    private void Awake()
    {
        switch (noiseType)
        {
            case NoiseMap.Continentality:
                wavesReference = WaveManager.Instance.CWaves;
                break;
            case NoiseMap.Height:
                wavesReference = WaveManager.Instance.CWaves;
                break;
            case NoiseMap.Temperature:
                wavesReference = WaveManager.Instance.CWaves;
                break;
            case NoiseMap.Humidity:
                wavesReference = WaveManager.Instance.CWaves;
                break;
        }
    }
    // Start is called before the first frame update
    void Start()
    {

        freqSlider.onValueChanged.AddListener(freq => {
            freqText.text = freq.ToString("0.0000");
            wavesReference[waveID].frequency = freq;
        });
        ampSlider.onValueChanged.AddListener(amp => {
            ampText.text = amp.ToString("0.0000");
            wavesReference[waveID].amplitude = amp;
        });


        freqText.text = freqSlider.value.ToString("0.0000");
        ampText.text = ampSlider.value.ToString("0.0000");
    }

    public void SetFrequency()
    {
        freqText.text = freqSlider.value.ToString("0.0000");
        wavesReference[waveID].frequency = freqSlider.value;
    }

    public void SetAmplitude()
    {
        ampText.text = ampSlider.value.ToString("0.0000");
        wavesReference[waveID].amplitude = ampSlider.value;
    }
}
