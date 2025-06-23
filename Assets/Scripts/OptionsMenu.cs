using UnityEngine;

public class OptionsMenu : MonoBehaviour
{
    public void OnMasterVolumeChanged(float value)
        => AudioManager.Instance.SetMasterVolume(value);

    public void OnMusicVolumeChanged(float value)
        => AudioManager.Instance.SetMusicVolume(value);

    public void OnSFXVolumeChanged(float value)
        => AudioManager.Instance.SetSFXVolume(value);

    public void OnMusicToggleChanged(bool isOn)
        => AudioManager.Instance.ToggleMusic(isOn);
}
