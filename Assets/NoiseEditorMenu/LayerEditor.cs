using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LayerEditor : MonoBehaviour
{
    public int layerID = 0;
    public NoiseMap noiseType;
    public Layer layerReference;

    public Slider freqSlider;
    public Slider ampSlider;

    public TextMeshProUGUI freqText;
    public TextMeshProUGUI ampText;


    void Start()
    {

        freqSlider.onValueChanged.AddListener(freq => {
            freqText.text = freq.ToString("0.0000");
            layerReference.frequency = freq;
        });
        ampSlider.onValueChanged.AddListener(amp => {
            ampText.text = amp.ToString("0.0000");
            layerReference.amplitude = amp;
        });


        freqText.text = freqSlider.value.ToString("0.0000");
        ampText.text = ampSlider.value.ToString("0.0000");
    }

    public void SetFrequency()
    {
        freqText.text = freqSlider.value.ToString("0.0000");
        layerReference.frequency = freqSlider.value;
    }

    public void SetAmplitude()
    {
        ampText.text = ampSlider.value.ToString("0.0000");
        layerReference.amplitude = ampSlider.value;
    }
}
