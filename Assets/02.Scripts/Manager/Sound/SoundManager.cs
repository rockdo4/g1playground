using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    [Header("Audio Mixer")]
    [SerializeField] private AudioMixer audioMixer;

    [Header("Audio Source")]
    [SerializeField] public AudioSource bgmSource;
    [SerializeField] public AudioSource seSource;

    [Header("BGMs")]
    //bgms
    [SerializeField] public List<AudioClip> bgmClips = new List<AudioClip>();

    [Header("Sound Effects")]
    //sound effects
    [SerializeField] public List<AudioClip> seClips = new List<AudioClip>();

    [Header("Start BGM")]
    [SerializeField] private string bgmName = "Playing";

    public const string Master_KEY = "MasterKey";
    public const string BGM_KEY = "BGMKey";
    public const string SE_KEY = "SEKey";    

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

        //load volumes on load
        LoadVolume();
        PlayBGM(bgmName);
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

    //play BGM by file name
    public void PlayBGM(string name)
    {
        foreach (var bgm in bgmClips)
        {
            if (bgm.name.Equals(name))
            {
                bgmSource.Stop();
                bgmSource.clip = bgm;
                bgmSource.Play();
            }
        }
    }


    //play SE by file name find in SE list
    public void PlaySoundEffect(string name)
    {
        foreach (var se in seClips)
        {
            if (se.name.Equals(name))
            {
                //seSource.Stop();
                seSource.PlayOneShot(se);
            }
        }
    }

    //play SE by audio clip
    public void PlaySoundEffect(AudioClip clip)
    {
        //seSource.Stop();
        seSource.PlayOneShot(clip);
        
    }

    //Pause playing SoundEffect
    public void PauseSoundEffect()
    {
        seSource.Pause();
    }

    //Resume paused SoundEffect
    public void UnPauseSoundEffect()
    {
        seSource.UnPause();
    }

    //Pause playing BGM
    public void PauseBGM()
    {
        bgmSource.Pause();
    }

    //Resume paused BGM
    public void UnPauseBGM()
    {
        bgmSource.UnPause();
    }
}
