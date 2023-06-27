using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    public TMP_InputField seed;
    public Slider renderDistanceSlider;
    public TextMeshProUGUI renderDistanceText;
    public TMP_Dropdown dropDown;

    public Slider freqSlider;
    public Slider ampSlider;
    public TextMeshProUGUI freqText;
    public TextMeshProUGUI ampText;
    //public TextMeshProUGUI currentWaveText;
    //private int currentWave = 0;

    private void Start()
    {
        //currentWaveText.text = "Aktualnie wybrana fala: 0";
        seed.text = ChunkLoader.Instance.seed.ToString();
        renderDistanceText.text = ChunkLoader.renderDistance.ToString();
        renderDistanceSlider.value = ChunkLoader.renderDistance;

        freqSlider.onValueChanged.AddListener((v) => {
              
            freqText.text = v.ToString("0.000");
        });
        ampSlider.onValueChanged.AddListener((v) => {
            ampText.text = v.ToString("0.000");
        });

        renderDistanceSlider.onValueChanged.AddListener((v) =>
        {
            renderDistanceText.text = v.ToString();
            ChunkLoader.renderDistance = (int)v;
            ChunkLoader.Instance.ReloadChunks();
        });

        seed.onEndEdit.AddListener((v) =>
        {
            try
            {
                int newSeed = int.Parse(v);
                ChunkLoader.Instance.seed = newSeed;
            }
            catch {
                ChunkLoader.Instance.seed = int.MaxValue;
                seed.text = int.MaxValue.ToString();
            }
        });
    }

    public void ChangeDisplayedNoiseMap()
    {
        switch (dropDown.value)
        {
            case 0:
                ChunkLoader.Instance.selectedNoiseMap = NoiseMap.Continentality;
                break;
            case 1:
                ChunkLoader.Instance.selectedNoiseMap = NoiseMap.Height;
                break;
            case 2:
                ChunkLoader.Instance.selectedNoiseMap = NoiseMap.Temperature;
                break;
            case 3:
                ChunkLoader.Instance.selectedNoiseMap = NoiseMap.Humidity;
                break;
        }
    }

    public void DisplayNoise()
    {
        ChunkLoader.Instance.SwitchNoise();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
