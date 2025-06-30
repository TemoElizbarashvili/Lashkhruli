using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmSettings : MonoBehaviour
{
    //AUDIO
    public Slider MasterVolumeSlider;
    public Slider MusicVolumeSlider;
    public Slider SFXVolumeSlider;
    public Toggle MusicToggle;

    // DISPLAY
    public TMP_Dropdown ResolutionDropdown;
    public TMP_Dropdown ScreenModeDropdown;
    public TMP_Dropdown QualityDropdown;
    public Toggle VSync;

    void Start()
    {
        #region Audio
        MasterVolumeSlider.value = PlayerPrefs.GetFloat("MasterVolume", 0.75f);
        MusicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.75f);
        SFXVolumeSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0.75f);
        MusicToggle.isOn = PlayerPrefs.GetInt("MusicOn", 1) == 1;

        AudioManager.Instance.SetMasterVolume(MasterVolumeSlider.value);
        AudioManager.Instance.SetSFXVolume(SFXVolumeSlider.value);
        AudioManager.Instance.ToggleMusic(MusicToggle.isOn);
        #endregion

        #region Display

        var resolution = new Resolution
        {
            width = PlayerPrefs.GetInt("ResolutionWidth", Screen.currentResolution.width),
            height = PlayerPrefs.GetInt("ResolutionHeight", Screen.currentResolution.height)
        };

        ResolutionDropdown.value = Screen.resolutions.ToList()
            .FindIndex(x => x.width == resolution.width && x.height == resolution.height);
        ResolutionDropdown.RefreshShownValue();

        ScreenModeDropdown.value = PlayerPrefs.GetInt("FullScreenMode", 0);
        ScreenModeDropdown.RefreshShownValue();

        QualityDropdown.value = PlayerPrefs.GetInt("QualityLevel", 0);
        QualityDropdown.RefreshShownValue();

        VSync.isOn = PlayerPrefs.GetInt("VSync", 1) == 1;
        #endregion
    }
}
