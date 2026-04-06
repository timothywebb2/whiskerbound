using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Audio;

public class Options : MonoBehaviour
{
    public AudioMixer Mixer;
   	AudioManager audioManager;
	AudioSource musicSource;
   	AudioSource sfxSource;

   	public Slider masterSlider;
   	public Slider musicSlider;
   	public Slider sfxSlider;

    public Slider brightSlider;
    public VolumeProfile profile;
    private LiftGammaGain brightness;

	private void Start()
	{
        //Audio Sources
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        musicSource = audioManager.transform.GetChild(0).gameObject.GetComponent<AudioSource>();
        sfxSource = audioManager.transform.GetChild(1).gameObject.GetComponent<AudioSource>();

        //Sliders
        masterSlider.value = PlayerPrefs.GetFloat("Volume", 0.75f);
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0.75f);

        //brightSlider.value = PlayerPrefs.GetFloat("Brightness", 1f);

        //NOTE: SET PLAYER PREFS ON SCENE START (PROBABLY IN PAUSE MANAGER)
        //SO THAT SET OPTIONS DO NOT ACTIVATE ONLY WHEN PAUSE MENU IS OPENED

        profile.TryGet<LiftGammaGain>(out brightness);
	}

    public void ChangeBrightness(float value)
    {
        brightness.gain.Override(new Vector4(1f, 1f, 1f, value));
        PlayerPrefs.SetFloat("Brightness", value);
    }

    public void ChangeMasterAudio(float value)
    {
        Mixer.SetFloat("Volume", Mathf.Log10(value) * 20);

        PlayerPrefs.SetFloat("Volume", value);
        PlayerPrefs.Save();
    }

    public void ChangeMusicAudio(float value)
    {
        musicSource.volume = value;

        PlayerPrefs.SetFloat("MusicVolume", value);
        PlayerPrefs.Save();
    }
    public void ChangeSFXAudio(float value)
    {

        sfxSource.volume = value;

        PlayerPrefs.SetFloat("SFXVolume", value);
       	PlayerPrefs.Save();
    }
}
