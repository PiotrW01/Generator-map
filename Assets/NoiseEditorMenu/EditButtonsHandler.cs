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
        TMP_Dropdown.OptionData option;

        // Add presets in dropdown
        var names = LayerManager.Instance.presetNames;
        Debug.Log(names.Count);
        foreach ( var name in names ) 
        {
            option = new(name);
            presetDropDown.options.Add(option);
        }

        // Add Create new preset option in dropdown
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
            ReloadEditor();

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
                ReloadEditor();
            } 
            else
            {
                LayerManager.Instance.LoadPreset(presetDropDown.options[presetDropDown.value].text);
                ReloadEditor();
            }

            confirmed = false;
            buttonText.text = tempText;
        }
    }

    public void DeletePreset()
    {
        // Return if default is selected
        if (presetDropDown.value == 0) return;

        if(!confirmed)
        {
            SetToConfirm();
        } 
        else
        {
            // Remove preset from saved presets and from dropdown
            LayerManager.Instance.RemovePreset(presetDropDown.options[presetDropDown.value].text);
            presetDropDown.options.RemoveAt(presetDropDown.value);
            // Select default
            presetDropDown.value = 0;
            LayerManager.Instance.ResetToDefault();
            ReloadEditor();

            confirmed = false;
            buttonText.text = tempText;
        }

    }

    public void OnDropDownSelect()
    {
        // Max 30 presets (32 - default option - create new preset option)
        if (presetDropDown.options.Count > 32) return;

        // If create new preset is selected
        if(presetDropDown.options.Count - 1 == presetDropDown.value)
        {
            // Replace Create new preset text with new preset name
            presetDropDown.options[presetDropDown.value].text = CreatePresetName();

            // Add preset name to preset list
            LayerManager.Instance.presetNames.Add(presetDropDown.options[presetDropDown.value].text);

            // Add new create new preset option 
            TMP_Dropdown.OptionData option = new("Create new preset...");
            presetDropDown.options.Add(option);

            // Reselect the new preset
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

    // Reloads layers in each NoiseMapEditor
    public void ReloadEditor()
    {
        for (int i = 0; i < EditWindowContent.transform.childCount; i++)
        {
            EditWindowContent.transform.GetChild(i).GetComponent<NoiseMapEditor>().UnloadLayers();
            EditWindowContent.transform.GetChild(i).GetComponent<NoiseMapEditor>().LoadLayers();
        }
    }
}
