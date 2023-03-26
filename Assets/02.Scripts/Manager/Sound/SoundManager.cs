using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();

    [Header("Audio Mixer")]
    [SerializeField] private AudioMixer audioMixer;

    [Header("Audio Source")]
    [SerializeField] public AudioSource bgmSource;
    [SerializeField] public AudioSource seSource;
    [SerializeField] public AudioSource playerSource;
    [SerializeField] public AudioSource enemySource;

    [SerializeField] public int playerSourceCount = 10;
    [SerializeField] private AudioMixerGroup playerMixer;
    private List<AudioSource> playerSources = new List<AudioSource>();

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

    public string BgmFolderPath = "Sounds/BGM";
    public string EffectFolderPath = "Sounds/Effect";

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

        bgmClips = Resources.LoadAll<AudioClip>(BgmFolderPath).ToList();
        seClips = Resources.LoadAll<AudioClip>(EffectFolderPath).ToList();

        for (int i = 0; i < playerSourceCount; i++)
        {
            var playerSE = Instantiate(playerSource);

            playerSources.Add(playerSE);
            playerSE.transform.parent = seSource.transform;
        }

        //load volumes on load
        LoadVolume();
        //PlayBGM(bgmName);
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

    //stop Bgm
    public void StopBGM()
    {
        bgmSource.Stop();
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

    //play SE of player
    public void PlayPlayerEffect(string name)
    {
        //search for not playing audio source
        foreach (var source in playerSources)
        {
            if (!source.isPlaying)
            {
                //search for sound clip
                foreach (var se in seClips)
                {
                    if (se.name.Equals(name))
                    {
                        Debug.Log(source.name);
                        source.clip = se;
                        source.Play();
                        return;

                    }

                }

#if UNITY_EDITOR
                Debug.Log("No sound Clip match");
#endif

                return;
            }
        }

#if UNITY_EDITOR
        Debug.Log("All player audio source playing");
#endif

        return;
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
