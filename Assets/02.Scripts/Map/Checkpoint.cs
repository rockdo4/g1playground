using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Checkpoint : MonoBehaviour
{
    private void Awake()
    {
        
       
    }

    private void OnTriggerEnter(Collider other)
    {        
        if (other.CompareTag("Player"))
        {
            PlayerDataManager.instance.SaveLastPos(MapManager.instance.GetCurrentMapName(),MapManager.instance.GetCurrentChapterName(), transform.position);
            

        }
    }

}
