using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundPlayer : MonoBehaviour
{
    private AudioSource audioSource;
    private Animator animator;
    private GroundType.Type type = GroundType.Type.None;

    [SerializeField] private PlayerFootSound grassFootSound;
    [SerializeField] private PlayerFootSound sandFootSound;
    [SerializeField] private PlayerFootSound villageFootSound;

    [SerializeField] private PlayerJumpSound grassJumpSound;
    [SerializeField] private PlayerJumpSound sandJumpSound;
    [SerializeField] private PlayerJumpSound villageJumpSound;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
    }

    public void SetType(GroundType.Type type)
    {
        if (this.type != type)
        {
            this.type = type;
        }
        
    }

    private void Run()
    {
        int index = 0;
        string clip = null;
        switch (type)
        {
            
            case GroundType.Type.Grass:
                index = Random.Range(0, grassFootSound.soundClips.Length);
                clip = grassFootSound.soundClips[index];
                break;
            case GroundType.Type.Sand:
                index = Random.Range(0, sandFootSound.soundClips.Length);
                clip = sandFootSound.soundClips[index];
                break;
            case GroundType.Type.Village:
                index = Random.Range(0, villageFootSound.soundClips.Length);
                clip = villageFootSound.soundClips[index];
                break;
        }
       
        audioSource.clip = SoundManager.instance.GetAudioClip(clip);
        audioSource.Play();
    }

    private void StartJump()
    {
        audioSource.Stop();
        string clip = null;
        switch (type)
        {

            case GroundType.Type.Grass:

                clip = grassJumpSound.jumpClip;
                break;
            case GroundType.Type.Sand:

                clip = sandJumpSound.jumpClip;
                break;
            case GroundType.Type.Village:

                clip = villageJumpSound.jumpClip;
                break;
        }

        audioSource.clip = SoundManager.instance.GetAudioClip(clip);
        audioSource.Play();
    }

    public void Landing()
    {
        audioSource.Stop();
        string clip = null;
        switch (type)
        {

            case GroundType.Type.Grass:

                clip = grassJumpSound.landingClip;
                break;
            case GroundType.Type.Sand:

                clip = sandJumpSound.landingClip;
                break;
            case GroundType.Type.Village:

                clip = villageJumpSound.landingClip;
                break;
        }

        audioSource.clip = SoundManager.instance.GetAudioClip(clip);
        audioSource.Play();
    }
}
