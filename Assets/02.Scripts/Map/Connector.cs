using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class Connector : MonoBehaviour
{
    [SerializeField]
    private GameObject nextStageRoomPrefab;
    [SerializeField]
    private GameObject nextDoorPrefab;
    private bool isActive = true;
    private void Update()
    {
       
        if (Input.GetKeyDown(KeyCode.P))
        {

            GameManager.instance.Respawn();
            return;
        }
    }   

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player") && nextStageRoomPrefab != null)
        {
            other.GetComponent<NavMeshAgent>().ResetPath();
            nextStageRoomPrefab.SetActive(false);

            if (isActive)
            {
                other.GetComponent<NavMeshAgent>().SetDestination(nextDoorPrefab.transform.position);
                MapManager.instance.SetCurrentMapName(nextStageRoomPrefab.transform.name);
                nextDoorPrefab.GetComponent<Connector>().isActive = false;
                nextStageRoomPrefab.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        isActive = true;
    }

}
