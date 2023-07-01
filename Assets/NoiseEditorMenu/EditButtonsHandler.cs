using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class EditButtonsHandler : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler
{
    public TMP_Dropdown presetDropDown;
    public GameObject EditWindowContent;

    private TextMeshProUGUI buttonText;
    private string tempText;

    private bool confirmed = false;

    private void Start()
    {
        TMP_Dropdown.OptionData option = new("Default");
        presetDropDown.options.Add(option);

        var names = LayerManager.Instance.presetNames;
        foreach ( var name in names ) 
        {
            option = new(name);
            presetDropDown.options.Add(option);
        }

        option = new("Create new preset...");
        presetDropDown.options.Add(option);
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        buttonText = eventData.pointerEnter.GetComponent<TextMeshProUGUI>();
        tempText = buttonText.text;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        confirmed = false;
        buttonText.text = tempText;
    }

    public void ResetToDefault()
    {
        if (!confirmed)
        {
            SetToConfirm();
        } else
        {
            LayerManager.Instance.ResetToDefault();
            for (int i = 0; i < EditWindowContent.transform.childCount; i++)
            {
                EditWindowContent.transform.GetChild(i).GetComponent<NoiseMapEditor>().UnloadLayers();
                EditWindowContent.transform.GetChild(i).GetComponent<NoiseMapEditor>().LoadLayers();
            }
            confirmed = false;
            buttonText.text = tempText;
        }
    }

    public void SavePreset()
    {
        if (presetDropDown.value == 0) return;

        if (!confirmed)
        {
            SetToConfirm();
        }
        else
        {
            LayerManager.Instance.SavePreset(presetDropDown.options[presetDropDown.value].text);
            Debug.Log(presetDropDown.options[presetDropDown.value].text);
            confirmed = false;
            buttonText.text = tempText;
        }
    }

    public void LoadPreset()
    {
        if (!confirmed)
        {
            SetToConfirm();
        }
        else
        {
            if (presetDropDown.value == 0)
            {
                LayerManager.Instance.ResetToDefault();
                for (int i = 0; i < EditWindowContent.transform.childCount; i++)
                {
                    EditWindowContent.transform.GetChild(i).GetComponent<NoiseMapEditor>().UnloadLayers();
                    EditWindowContent.transform.GetChild(i).GetComponent<NoiseMapEditor>().LoadLayers();
                }
            } 
            else
            {
                LayerManager.Instance.LoadPreset(presetDropDown.options[presetDropDown.value].text);
                for (int i = 0; i < EditWindowContent.transform.childCount; i++)
                {
                    EditWindowContent.transform.GetChild(i).GetComponent<NoiseMapEditor>().UnloadLayers();
                    EditWindowContent.transform.GetChild(i).GetComponent<NoiseMapEditor>().LoadLayers();
                }
            }
            confirmed = false;
            buttonText.text = tempText;
        }
    }

    public void DeletePreset()
    {
        if (presetDropDown.value == 0) return;

        if(!confirmed)
        {
            SetToConfirm();
        } 
        else
        {
            LayerManager.Instance.RemovePreset(presetDropDown.options[presetDropDown.value].text);
            presetDropDown.options.RemoveAt(presetDropDown.value);
            presetDropDown.value = 0;
            
            confirmed = false;
            buttonText.text = tempText;
        }

    }

    public void OnDropDownSelect()
    {
        if (presetDropDown.options.Count > 30) return;

        if(presetDropDown.options.Count - 1 == presetDropDown.value)
        {
            presetDropDown.options[presetDropDown.value].text = CreatePresetName();


            LayerManager.Instance.presetNames.Add(presetDropDown.options[presetDropDown.value].text);

            TMP_Dropdown.OptionData option = new("Create new preset...");
            presetDropDown.options.Add(option);

            presetDropDown.value = 0;
            presetDropDown.value = presetDropDown.options.Count - 2;
            return;
        }
    }

    private void SetToConfirm()
    {
        buttonText.text = "Click to confirm";
        confirmed = true;
        return;
    }

    private bool IsPresetNameAvailable(string name)
    {
        var names = LayerManager.Instance.presetNames;
        return !names.Contains(name);
    }

    private string CreatePresetName()
    {
        string name = "preset#" + Random.Range(1, 1000);
        bool canCreate = false;

        while (!canCreate)
        {
            name = "preset#" + Random.Range(1, 1000);
            canCreate = IsPresetNameAvailable(name);
        }
        return name;
    }
}
