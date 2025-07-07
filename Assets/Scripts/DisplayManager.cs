using System;
using UnityEngine;

public class DisplayManager : MonoBehaviour
{
    public static DisplayManager Instance { get; private set; }

    void Awake()
    {
        if (Instance is not null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetResolution(Resolution res)
    {
        Screen.SetResolution(res.width, res.height, GetCurrentScreenMode());
        PlayerPrefs.SetInt("ResolutionWidth", res.width);
        PlayerPrefs.SetInt("ResolutionHeight", res.height);
    }

    private FullScreenMode GetCurrentScreenMode()
    {
        return (FullScreenMode)PlayerPrefs.GetInt("FullScreenMode", 0);
    }

    public void SetScreenMode(int modeIndex)
    {
        // 0 = Exclusive Fullscreen, 1 = Windowed, 2 = Borderless (FullscreenWindow)
        Screen.fullScreenMode = modeIndex switch
        {
            0 => FullScreenMode.ExclusiveFullScreen,
            1 => FullScreenMode.Windowed,
            2 => FullScreenMode.FullScreenWindow,
            _ => Screen.fullScreenMode
        };
        PlayerPrefs.SetInt("FullScreenMode", modeIndex);
    }

    public void SetQuality(int qualityIndex)
    {
        // 0 = HIGH, 1 = MEDIUM, 2 = LOW

        if (qualityIndex < 0 || qualityIndex >= QualitySettings.names.Length)
            return;

        QualitySettings.SetQualityLevel(qualityIndex, true);
        PlayerPrefs.SetInt("QualityLevel", qualityIndex);
    }

    public void SetVSync(bool isOn)
    {
        QualitySettings.vSyncCount = isOn ? 1 : 0;
        PlayerPrefs.SetInt("VSync", isOn ? 1 : 0);
    }
}
