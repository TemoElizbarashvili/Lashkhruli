using UnityEngine;

public static class Helpers
{
    public static void PlayAudioSafely(AudioSource audio)
    {
        if (audio.isPlaying)
        {
            audio.Stop();
        }
        audio.Play();
    }
    
}
