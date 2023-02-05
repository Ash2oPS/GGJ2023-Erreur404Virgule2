using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CS_AudioPlayer
{
    public static void PlayMusicWithFactor(this AudioSource audioSource, AudioClip audioClip)
    {
        if (CS_PlayerPrefs.Instance != null)
        {
            audioSource.volume = CS_PlayerPrefs.Instance.MusicFactor;
        }
        audioSource.loop = true;
        audioSource.clip = audioClip;
        audioSource.Play();
    }

    public static void PlaySoundWithFactor(this AudioSource audioSource, AudioClip audioClip)
    {
        if (CS_PlayerPrefs.Instance != null)
        {
            audioSource.volume = CS_PlayerPrefs.Instance.SoundFactor;
        }
        audioSource.clip = audioClip;
        audioSource.Play();
    }
}