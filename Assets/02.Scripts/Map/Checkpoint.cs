using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private string effectName = "Area_star_ellow";
    private GameObject effect;

    private void Awake()
    {
        //GameManager.instance.effectManager.ReturnEffect(effectName, effect);

    }

    private void OnDisable()
    {
        if (effect != null)
        {
            GameManager.instance.effectManager.ReturnEffect(effectName, effect);
            effect = null;
        }        
    }

    private void OnTriggerEnter(Collider other)
    {        
        
        if (other.CompareTag("Player"))
        {
            if (PlayerDataManager.instance != null)
            {
                PlayerDataManager.instance.SaveLastPos(MapManager.instance.GetCurrentMapName(),
                    MapManager.instance.GetCurrentChapterName(), transform.position);
            }
            
            //set effect
            if (effect != null)
            {
                return;
            }
            effect = GameManager.instance.effectManager.GetEffect(effectName);
            effect.transform.position = transform.position;
            //float destroyTime = effect.GetComponent<ParticleSystem>().main.duration;
            
        }
    }

}
