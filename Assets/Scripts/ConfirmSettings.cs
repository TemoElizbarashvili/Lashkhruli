using UnityEngine;
using UnityEngine.UI;

public class ConfirmSettings : MonoBehaviour
{
    public Slider MasterVolumeSlider;
    public Slider MusicVolumeSlider;
    public Slider SFXVolumeSlider;
    public Toggle MusicToggle;

    void Start()
    {
        MasterVolumeSlider.value = PlayerPrefs.GetFloat("MasterVolume", 0.75f);
        MusicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.75f);
        SFXVolumeSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0.75f);
        MusicToggle.isOn = PlayerPrefs.GetInt("MusicOn", 1) == 1;

        AudioManager.Instance.SetMasterVolume(MasterVolumeSlider.value);
        AudioManager.Instance.SetSFXVolume(SFXVolumeSlider.value);
        AudioManager.Instance.ToggleMusic(MusicToggle.isOn);
    }
}
