using UnityEngine;
using UnityEngine.Audio;

public class BackGroundMusic : MonoBehaviour
{
    public static BackGroundMusic Instance { get; private set; }

    public AudioSource AudioSource;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            AudioSource = GetComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Mute(bool isMuted)
    {
        if (AudioSource != null)
            AudioSource.mute = isMuted;
    }
}
