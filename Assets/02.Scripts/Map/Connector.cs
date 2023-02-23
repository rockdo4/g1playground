using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class Connector : MonoBehaviour
{
    public enum DoorType
    {
        Walk,
        Portal,
    }

    [SerializeField]
    private GameObject nextStageRoomPrefab;

    public GameObject NextStage { get { return nextStageRoomPrefab; } }
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
                    MapManager.instance.SetCurrentMapName(transform.parent.name);

                }

            }
        }
    }

    

}
