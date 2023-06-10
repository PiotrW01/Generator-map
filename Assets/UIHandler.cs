using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    
    public TMP_InputField width;
    public TMP_InputField height;
    //public TMP_InputField scale;
    public TMP_InputField offsetX;
    public TMP_InputField offsetY;
    public TMP_InputField seed;

    public Slider colorSlider;
    public Slider scaleSlider;
    public Slider freqSlider;
    public Slider ampSlider;
    public TextMeshProUGUI colorText;
    public TextMeshProUGUI scaleText;
    public TextMeshProUGUI freqText;
    public TextMeshProUGUI ampText;
    public TextMeshProUGUI currentWaveText;

    public GameObject NMG;
    private Wave[] waves;
    private NoiseMapGenerator gen;
    private int currentWave = 0;

    private bool skip1 = false;
    private bool skip2 = false;

    private void Start()
    {
        currentWaveText.text = "Aktualnie wybrana fala: 0";
        gen = NMG.GetComponent<NoiseMapGenerator>();
        waves = gen.heightWaves;

        width.text = gen.width.ToString();
        height.text = gen.height.ToString();
        offsetX.text = gen.offset.x.ToString();
        offsetY.text = gen.offset.y.ToString();
        seed.text = waves[currentWave].seed.ToString();
        colorText.text = gen.colorTreshold.ToString(".00");
        scaleText.text = gen.scale.ToString();
        scaleSlider.value = gen.scale;
        colorSlider.value = gen.colorTreshold;
        freqText.text = waves[currentWave].frequency.ToString("0.00");
        freqSlider.value = waves[currentWave].frequency;
        ampText.text = waves[currentWave].amplitude.ToString("0.00");
        ampSlider.value = waves[currentWave].amplitude;

        freqSlider.onValueChanged.AddListener((v) => {
            if (skip1)
            {
                skip1 = false;
                return;
            }
                
            freqText.text = v.ToString("0.000");
            waves[currentWave].frequency = v;
            gen.GenerateMap();
        });
        ampSlider.onValueChanged.AddListener((v) => {
            if (skip2)
            {
                skip2 = false;
                return;
            }

            ampText.text = v.ToString("0.000");
            waves[currentWave].amplitude = v;
            gen.GenerateMap();
        });

        scaleSlider.onValueChanged.AddListener((v) =>
        {
            scaleText.text = v.ToString("0.00");
            gen.scale = v;
            gen.GenerateMap();
        });

        colorSlider.onValueChanged.AddListener((v) => {
            colorText.text = v.ToString(".00");
            gen.colorTreshold = v;
            if (!gen.isColor()) return;
            gen.GenerateMap();
        });

        seed.onEndEdit.AddListener((v) =>
        {
            waves[currentWave].seed = int.Parse(v);
        });


        width.onEndEdit.AddListener((v) =>
        {
            gen.width = int.Parse(v);
        });

        height.onEndEdit.AddListener((v) =>
        {
            gen.height = int.Parse(v);
        });

        offsetX.onEndEdit.AddListener((v) =>
        {
            gen.offset.x = int.Parse(v);
        });

        offsetY.onEndEdit.AddListener((v) =>
        {
            gen.offset.y = int.Parse(v);
        });

    }

    public void randomValues()
    {
        skip1 = true;
        skip2 = true;

        float randomFloat = Random.Range(0.02f, 0.5f);
        freqText.text = randomFloat.ToString("0.000");
        waves[currentWave].frequency = randomFloat;
        freqSlider.value = randomFloat;

        randomFloat = Random.Range(0.01f, 1.0f);
        ampText.text = randomFloat.ToString("0.000");
        waves[currentWave].amplitude = randomFloat;
        ampSlider.value = randomFloat;

        int randomInt = Random.Range(1, 99000);
        seed.text = randomInt.ToString();
        waves[currentWave].seed = randomInt;

        randomInt = Random.Range(0, 400);
        gen.offset.x = randomInt;
        offsetX.text = randomInt.ToString();

        randomInt = Random.Range(0, 400);
        gen.offset.y = randomInt;
        offsetY.text = randomInt.ToString();

        gen.GenerateMap();
    }

    public void switchColor()
    {
        gen.setColor(!gen.isColor());
        gen.GenerateMap();
    }

    public void SelectNextWave()
    {
        skip1 = true;
        skip2 = true;

        if (currentWave+1 < waves.Length)
        {
            currentWave++;
            currentWaveText.text = "Aktualnie wybrana fala: " + currentWave.ToString();

            freqText.text = waves[currentWave].frequency.ToString("0.000");
            freqSlider.value = waves[currentWave].frequency;

            ampText.text = waves[currentWave].amplitude.ToString("0.000");
            ampSlider.value = waves[currentWave].amplitude;

            seed.text = waves[currentWave].seed.ToString();
        }
        else if (currentWave != 0)
        {
            currentWave = 0;
            currentWaveText.text = "Aktualnie wybrana fala: " + currentWave.ToString();

            freqText.text = waves[currentWave].frequency.ToString("0.000");
            freqSlider.value = waves[currentWave].frequency;

            ampText.text = waves[currentWave].amplitude.ToString("0.000");
            ampSlider.value = waves[currentWave].amplitude;

            seed.text = waves[currentWave].seed.ToString();
        }
    }

    public void AddOffsetX()
    {
        gen.offset.x += 1;
        offsetX.text = (gen.offset.x).ToString();
        gen.GenerateMap();
    }


    public void SubtractOffsetX()
    {
        gen.offset.x -= 1;
        offsetX.text = (gen.offset.x).ToString();
        gen.GenerateMap();
    }
    public void AddOffsetY()
    {
        gen.offset.y += 1;
        offsetY.text = (gen.offset.y).ToString();
        gen.GenerateMap();
    }

    public void SubtractOffsetY()
    {
        gen.offset.y -= 1;
        offsetY.text = (gen.offset.y).ToString();
        gen.GenerateMap();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
