using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    public AudioMixer AudioMixer;
    public bool IsMusicOn
    {
        get => PlayerPrefs.GetInt("MusicOn", 1) == 1;
        set => PlayerPrefs.SetInt("MusicOn", value ? 1 : 0);
    }
    public float MasterVolume
    {
        get => PlayerPrefs.GetFloat("MasterVolume", 0.75f);
        set => PlayerPrefs.SetFloat("MasterVolume", value);
    }
    public float MusicVolume
    {
        get => PlayerPrefs.GetFloat("MusicVolume", 0.75f);
        set => PlayerPrefs.SetFloat("MusicVolume", value);
    }
    public float SFXVolume
    {
        get => PlayerPrefs.GetFloat("SFXVolume", 0.75f);
        set => PlayerPrefs.SetFloat("SFXVolume", value);
    }


    void Awake()
    {
        if (Instance is not null)
            return;

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    public void SetMusicVolume(float volume)
    {
        if (volume == 0f || !IsMusicOn)
        {
            SetVolume("MusicVolume", -80f, volume);
            return;
        }
        SetVolume("MusicVolume", Mathf.Log10(volume) * 20, volume);
    }

    public void SetMasterVolume(float volume)
    {
        if (volume == 0f)
        {
            SetVolume("MasterVolume", -80f, volume);
            return;
        }
        SetVolume("MasterVolume", Mathf.Log10(volume) * 20, volume);
    }

    public void SetSFXVolume(float volume)
    {
        if (volume == 0f)
        {
            SetVolume("SFXVolume", -80f, volume);
            return;
        }
        SetVolume("SFXVolume", Mathf.Log10(volume) * 20, volume);
    }

    private void SetVolume(string name, float value, float slideValue)
    {
        AudioMixer.SetFloat(name, value);
        PlayerPrefs.SetFloat(name, slideValue);
    }



    public void ToggleMusic(bool isOn)
    {
        PlayerPrefs.SetInt("MusicOn", isOn ? 1 : 0);
        if (isOn)
        {
            SetMusicVolume(MusicVolume);
        }
        else
        {
            AudioMixer.SetFloat("MusicVolume", -80f);
        }
    }
}
