using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class Connector : MonoBehaviour
{    public enum DoorType
    {
        Walk,
        Portal,
    }

    [SerializeField]
    private GameObject nextStageRoomPrefab;
    [SerializeField]
    // private GameObject nextDoorPrefab;
    private bool isActive = false;
    public bool IsActive { set { isActive = value; } get { return isActive; } }
    public DoorType doortype;

    private void OnEnable()
    {
        isActive = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player") && nextStageRoomPrefab != null)
        {

            if (!nextStageRoomPrefab.active)
            {
                nextStageRoomPrefab.SetActive(true);
            }
            else if (nextStageRoomPrefab.active)
            {
                if (MapManager.instance.GetCurrentMapName().CompareTo(transform.parent.name) != 0)
                {
                    nextStageRoomPrefab.SetActive(false);
                    MapManager.instance.SetCurrentMapName(transform.parent.name);
                }

            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        isActive = true;

    }

}
