using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [Header("--- Audio Source ---")]
    [SerializeField] AudioMixer Mixer;
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;


    private void Start()
    {
        Mixer.SetFloat("Volume", Mathf.Log10(PlayerPrefs.GetFloat("Volume", 0.75f)) * 20);
        musicSource.volume = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        SFXSource.volume = PlayerPrefs.GetFloat("SFXVolume", 0.75f);
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
}

