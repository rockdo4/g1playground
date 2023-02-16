using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] public AudioSource bgm;
    [SerializeField] public AudioSource se;

    public const string Master_KEY = "MasterVolume";
    public const string BGM_KEY = "BGMVolume";
    public const string SE_KEY = "SEVolume";

    private static SoundManager m_instance;
    public static SoundManager instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<SoundManager>();
            }

            return m_instance;
        }
    }

    private void Awake()
    {
        //singleton
        if (instance != this)
        {
            
            Destroy(gameObject);
        }

        LoadVolume();

    }

    //load volume set on sliders
    private void LoadVolume()
    {
        float masterVolume = PlayerPrefs.GetFloat(Master_KEY, 1f);
        float bgmVolume = PlayerPrefs.GetFloat(BGM_KEY, 1f);
        float seVolume = PlayerPrefs.GetFloat(SE_KEY, 1f);

        audioMixer.SetFloat(VolumeSetting.MIXER_Master, Mathf.Log10(masterVolume) * 20);
        audioMixer.SetFloat(VolumeSetting.MIXER_BGM, Mathf.Log10(bgmVolume) * 20);
        audioMixer.SetFloat(VolumeSetting.MIXER_SE, Mathf.Log10(seVolume) * 20);
    }
}
