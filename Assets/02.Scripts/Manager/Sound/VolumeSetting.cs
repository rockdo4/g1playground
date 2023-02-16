using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class VolumeSetting : MonoBehaviour
{
    [Header("Audio Mixer")]
    [SerializeField] private AudioMixer audioMixer;
    [Header("Audio Mixer sliders")]
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider seSlider;
    [Header("Audio Mixer sliders")]
    [SerializeField] private Toggle muteToggle;
    //Mixer volume keys
    public const string MIXER_Master = "MasterVolume";
    public const string MIXER_BGM = "BGMVolume";
    public const string MIXER_SE = "SEVolume";


    private void Awake()
    {
        //add sliders set volume methods
        if (masterSlider != null)
        {
            masterSlider.onValueChanged.AddListener(SetMasterVolume);
        }
        if (bgmSlider != null)
        {
            bgmSlider.onValueChanged.AddListener(SetBgmVolume);
        }
        if (seSlider != null)
        {
            seSlider.onValueChanged.AddListener(SetSEVolume);
        }  
    }

    private void Start()
    {
        //Init slider value
        InitSliderValue();
    }

    //save volume values
    private void OnDisable()
    {
        if (masterSlider != null)
        {
            PlayerPrefs.SetFloat(SoundManager.Master_KEY, masterSlider.value);
        }
        if (bgmSlider != null)
        {
            PlayerPrefs.SetFloat(SoundManager.BGM_KEY, bgmSlider.value);
        }
        if (seSlider != null)
        {
            PlayerPrefs.SetFloat(SoundManager.SE_KEY, seSlider.value);
        }   
    }

    //Init slider value
    private void InitSliderValue()
    {
        InitMasterSlider();
        InitBGMSlider();
        InitSESlider();
    }

    //Init Master volume
    private void InitMasterSlider()
    {
        if (masterSlider == null)
        {
            return;
        }
        //set min value
        //if minvalue is under 0 volume goes back to 100%
        masterSlider.minValue = 0.0001f;
        masterSlider.value = PlayerPrefs.GetFloat(SoundManager.Master_KEY, 1f);
    }

    //Init BGM volume
    private void InitBGMSlider()
    {
        if (bgmSlider == null)
        {
            return;
        }
        //set min value
        //if minvalue is under 0 volume goes back to 100%
        bgmSlider.minValue = 0.0001f;
        bgmSlider.value = PlayerPrefs.GetFloat(SoundManager.BGM_KEY, 1f);
    }

    //Init SE volume
    private void InitSESlider()
    {
        if (seSlider == null)
        {
            return;
        }
        //set min value
        //if minvalue is under 0 volume goes back to 100%
        seSlider.minValue = 0.0001f;
        seSlider.value = PlayerPrefs.GetFloat(SoundManager.SE_KEY, 1f);
    }

    ////change volumes////
    public void SetMasterVolume(float value)
    {
        audioMixer.SetFloat(MIXER_Master, Mathf.Log10(value) * 20);
    }

    public void SetBgmVolume(float value)
    {
        audioMixer.SetFloat(MIXER_BGM, Mathf.Log10(value) * 20);
    }

    public void SetSEVolume(float value)
    {
        audioMixer.SetFloat(MIXER_SE, Mathf.Log10(value) * 20);
    }
    ///////////////////////

    //mute volume by toggle isOn
    public void Mute()
    {
        if (muteToggle == null)
        {
            return;
        }

        if (!muteToggle.isOn)
        {
            Debug.Log("sound on");
            SetMasterVolume(PlayerPrefs.GetFloat(SoundManager.Master_KEY, 1f));
            masterSlider.value = PlayerPrefs.GetFloat(SoundManager.Master_KEY, 1f);
        }
        else
        {
            Debug.Log("mute");
            PlayerPrefs.SetFloat(SoundManager.Master_KEY, masterSlider.value);
            SetMasterVolume(masterSlider.minValue);
            masterSlider.value = masterSlider.minValue;
        }
       
    }
}
