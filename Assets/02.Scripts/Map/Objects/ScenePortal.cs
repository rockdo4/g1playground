using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenePortal : MonoBehaviour
{
    [SerializeField] private string scene = "Scene02";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneLoader.Instance.LoadScene(scene);
            GameManager.instance.player.GetComponent<Status>().Restore();
            //SceneManager.LoadScene(scene);
        }
    }
}