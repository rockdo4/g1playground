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

   // private List<GameObject> nearStageRoomPrefab;

    //public List<GameObject> nextStage { get { return nearStageRoomPrefab; } }

    [SerializeField]
    // private GameObject nextDoorPrefab;
    private bool isActive = false;
    public bool IsActive { set { isActive = value; } get { return isActive; } }
    public DoorType doortype;

    private void OnEnable()
    {
        isActive = false;
    }

   


}
