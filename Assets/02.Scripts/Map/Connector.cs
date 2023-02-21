using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Connector : MonoBehaviour
{
    [SerializeField]
    private string nextStageName;
    private bool isActive = true;

    private void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
            Debug.Log("player is null");
        else if (ConnectorManager.instance.GetPreviousMapName()!=null&&ConnectorManager.instance.GetPreviousMapName() == nextStageName)
        {
            player.transform.position = transform.position;
            isActive = false;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("enter");

        if (other.transform.tag == "Player" && nextStageName != null)
        {
            if (isActive)
            {
                ConnectorManager.instance.SetPreviousMapName(SceneManager.GetActiveScene().name);
                SceneManager.LoadScene(nextStageName);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        isActive = true;
    }

}
