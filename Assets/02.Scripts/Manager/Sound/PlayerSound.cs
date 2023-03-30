using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PlayerSound : MonoBehaviour
{
    public IObjectPool<PlayerSound> PlayerSoundPool { get; set; }

    public AudioSource audioSource;
    private float timeLeft;
    
    private float timer = 0f;

    private bool isPause = false;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void InitSound(AudioClip clip)
    {        
        isPause = false;
        timer = 0f;
        timeLeft = clip.length;
        
        audioSource.clip = clip;
        audioSource.loop = false;
        audioSource.Play();
    }

    public void InitLoopSound(AudioClip clip)
    {
        isPause = false;
        timer = 0f;
        timeLeft = clip.length;

        audioSource.clip = clip;
        audioSource.loop = true;
        audioSource.Play();
    }

    private void FixedUpdate()
    {
        if (audioSource.loop) 
        {
            return;
        }
        //Stop timer if clip is paused
        if (!isPause)
        {
            timer += Time.deltaTime;
        }

        //Release and reset when timer is passed 
        if (timer >= timeLeft) 
        {
            timer = 0f;
            audioSource.clip = null;
            PlayerSoundPool.Release(this);
        }
    }

    public void PauseAudio()
    {
        isPause = true;
        audioSource.Pause();
    }

    public void ResumeAudio()
    {
        isPause = false;
        audioSource.UnPause();
    }

    public void StopAudio()
    {
        isPause = true;
        audioSource.Stop();
    }

    public void Release()
    {
        timer = 0f;
        audioSource.clip = null;
        audioSource.Stop();
        PlayerSoundPool.Release(this);
    }
}
