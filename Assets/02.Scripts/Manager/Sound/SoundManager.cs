using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Pool;
using UnityEngine.SceneManagement;


public class SoundManager : MonoBehaviour
{
    [Header("Audio Mixer")]
    [SerializeField] private AudioMixer audioMixer;

    [Header("Audio Source")]
    [SerializeField] public AudioSource bgmSource;
    [SerializeField] public AudioSource seSource;
    [SerializeField] public AudioSource playerSource;
    [SerializeField] public AudioSource enemySource;   

    [Header("BGMs")]
    [SerializeField] public List<AudioClip> bgmClips = new List<AudioClip>();

    [Header("Sound Effects")]
    [SerializeField] public List<AudioClip> seClips = new List<AudioClip>();

    [Header("Start BGM")]
    [SerializeField] private string bgmName = "Playing";

    public const string Master_KEY = "MasterKey";
    public const string BGM_KEY = "BGMKey";
    public const string SE_KEY = "SEKey";

    public string BgmFolderPath = "Sounds/BGM";
    public string EffectFolderPath = "Sounds/Effect";

    [Header("Player Sound Pool")]
    [SerializeField] private PlayerSound playerSound;
    public int maxPoolSize = 200;
    public int stackDefaultCapacity = 10;

    private IObjectPool<PlayerSound> playerSoundPool;
    public IObjectPool<PlayerSound> PlayerSoundPool
    {
        get
        {
            if (playerSoundPool == null)
                playerSoundPool =
                    new ObjectPool<PlayerSound>(
                        CreateSound,
                        OnTakeFromPool,
                        OnReturnedToPool,
                        OnDestroyPoolObject,
                        true,
                        stackDefaultCapacity,
                        maxPoolSize);
            return playerSoundPool;
        }
    }

    private PlayerSound CreateSound()
    {
        PlayerSound pSound = Instantiate(playerSound, seSource.transform);
        pSound.PlayerSoundPool = PlayerSoundPool;
        return pSound;
    }

    private void OnTakeFromPool(PlayerSound sound)
    {
        sound.gameObject.SetActive(true);

    }

    private void OnReturnedToPool(PlayerSound sound)
    {
        sound.gameObject.SetActive(false);

    }

    private void OnDestroyPoolObject(PlayerSound sound)
    {
        Destroy(sound.gameObject);
    }

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

    ///////////////////BGM/////////////////////////
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


    ///////////////////SE/////////////////////////
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

    //Play Player SoundEffect by searching sound clip
    public void PlayPlayerEffect(string name)
    {
        //search for sound clip
        foreach (var se in seClips)
        {
            if (se.name == (name))
            {
                var pSE = PlayerSoundPool.Get();
                pSE.InitSound(se);

                return;
            }
        }
    }

    //Pause playing SoundEffect
    public void PauseSoundEffect()
    {
        seSource.Pause();
        var playerSource = GetComponentsInChildren<PlayerSound>();
        foreach (var pSource in playerSource)
        {
            pSource.PauseAudio();
        }
    }

    //Pause Player SoundEffect by searching sound clip
    public void PausePlayerSound(string name)
    {
        var playerSource = GetComponentsInChildren<PlayerSound>();
        foreach (var pSource in playerSource)
        {
            if (pSource.audioSource.clip.name == name)
            {
                pSource.PauseAudio();
            }
            
        }
    }

    //Resume paused SoundEffect
    public void UnPauseSoundEffect()
    {
        seSource.UnPause();
        var playerSource = GetComponentsInChildren<PlayerSound>();
        foreach (var pSource in playerSource)
        {
            pSource.ResumeAudio();
        }
    }

    //Resume Player SoundEffect by searching sound clip
    public void UnPausePlayerSound(string name)
    {
        var playerSource = GetComponentsInChildren<PlayerSound>();
        foreach (var pSource in playerSource)
        {
            if (pSource.audioSource.clip.name == name)
            {
                pSource.ResumeAudio();
            }

        }
    }

    public void StopSoundEffect()
    {
        seSource.Stop();
        var playerSource = GetComponentsInChildren<PlayerSound>();
        foreach (var pSource in playerSource)
        {
            pSource.StopAudio();
        }
    }

    //Stop Player SoundEffect by searching sound clip
    public void StopPlayerSound(string name)
    {
        var playerSource = GetComponentsInChildren<PlayerSound>();
        foreach (var pSource in playerSource)
        {
            if (pSource.audioSource.clip.name == name)
            {
                pSource.StopAudio();
            }

        }
    }

    ////////////////////////////////////////////
    public void PauseAll()
    {
        PauseBGM();
        PauseSoundEffect();
    }

    public void ResumeAll()
    {
        UnPauseBGM();
        UnPauseSoundEffect();
    }

    public void StopAll()
    {
        StopBGM();
        StopSoundEffect();
    }

    /////////////////Audio Clip//////////////////////
    public AudioClip GetAudioClip(string name)
    {
        AudioClip audioClip = null;
        foreach (var clip in  seClips)
        {
            if (clip.name == name)
            {
                audioClip = clip;
                break;
            }
        }

        return audioClip;
    }
}
