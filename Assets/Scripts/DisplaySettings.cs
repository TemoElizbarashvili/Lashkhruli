using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DisplaySettings : MonoBehaviour
{
    public TMP_Dropdown ResolutionDropdown;
    public TMP_Dropdown ScreenModeDropdown;
    public TMP_Dropdown QualityDropdown;
    public Toggle VSync;

    private Resolution[] resolutions;

    void Start()
    {
        // Resolution
        if (ResolutionDropdown.options.Count > 0)
            return; // Avoid re-initializing if already set

        resolutions = Screen.resolutions.Select(r => new Resolution { width = r.width, height = r.height }).Distinct().ToArray();
        ResolutionDropdown.ClearOptions();

        var currentResIndex = 0;
        var options = resolutions.Select((res, index) =>
        {
            if (res.width == Screen.currentResolution.width && res.height == Screen.currentResolution.height)
                currentResIndex = index;

            return res.width + " x " + res.height;
        }).ToList();

        ResolutionDropdown.AddOptions(options);
        ResolutionDropdown.value = currentResIndex;
        ResolutionDropdown.RefreshShownValue();

        // Screen Mode
        var currentScreenMode = PlayerPrefs.GetInt("FullScreenMode", 0);
        ScreenModeDropdown.ClearOptions();
        ScreenModeDropdown.AddOptions(new System.Collections.Generic.List<string> { "Fullscreen", "Windowed", "Borderless" });
        ScreenModeDropdown.value = currentScreenMode;
        ScreenModeDropdown.RefreshShownValue();

        // Quality
        var currentQuality = PlayerPrefs.GetInt("QualityLevel", 0);
        QualityDropdown.ClearOptions();
        QualityDropdown.AddOptions(QualitySettings.names.ToList());
        QualityDropdown.value = currentQuality;
        QualityDropdown.RefreshShownValue();

        // VSync
        var isVSyncOn = PlayerPrefs.GetInt("VSync", 1) == 1;
        VSync.isOn = isVSyncOn;
        DisplayManager.Instance.SetVSync(isVSyncOn);
    }

    public void OnResolutionChanged(int index)
    {
        if (resolutions == null || resolutions.Length == 0)
            return;
        
        // Ensure the index is within bounds
        if (index < 0 || index >= resolutions.Length)
            return;

        var selectedRes = resolutions[index];
        DisplayManager.Instance.SetResolution(selectedRes);
    }

    public void OnScreenModeChanged(int index)
    {
        DisplayManager.Instance.SetScreenMode(index);
    }

    public void OnQualityChanged(int index)
        => DisplayManager.Instance.SetQuality(index);

    public void SetVSync(bool isOn)
        => DisplayManager.Instance.SetVSync(isOn);
}
