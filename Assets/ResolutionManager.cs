using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResolutionManager : MonoBehaviour
{
    public static ResolutionManager Instance;
    private Vector2Int[] resolutions;
    private int selectedRes = 2;
    private bool fullScreen = false;

    private void Awake()
    {
        // Make singleton instance
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
        resolutions = new Vector2Int[]
        {
            new Vector2Int(1024, 576),
            new Vector2Int(1280, 720),
            new Vector2Int(1600, 900),
            new Vector2Int(1920, 1080)
        };
        Screen.SetResolution(resolutions[selectedRes].x, resolutions[selectedRes].y, false);
    }

    public void SetResolution(int index)
    {
        selectedRes = index;
        if(!fullScreen) Screen.SetResolution(resolutions[selectedRes].x, resolutions[selectedRes].y, false);
    }

    public void ToggleFullscreen()
    {
        fullScreen = !fullScreen;
        if (fullScreen) Screen.SetResolution(1920, 1080, FullScreenMode.FullScreenWindow);
        else Screen.SetResolution(resolutions[selectedRes].x, resolutions[selectedRes].y, false);
    }
}
