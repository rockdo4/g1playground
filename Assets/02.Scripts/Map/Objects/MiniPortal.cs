using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniPortal : MonoBehaviour
{
    [SerializeField] private GameObject spawnPoint;
    [SerializeField] private MiniPortal linkedPortal;
    public bool IsUsed { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        IsUsed = false;
    }

    public Vector3 GetSpawnPoint()
    {
        return spawnPoint.transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<ObjectMass>() != null && !IsUsed) //(other.CompareTag("Player") || other.CompareTag("Pushable")) && !IsUsed) 
        {
            IsUsed = true;                        
            linkedPortal.IsUsed = true;
            other.transform.position = linkedPortal.GetSpawnPoint();
            //other.GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, 0f);

        }
    }

    private void OnTriggerExit(Collider other)
    {
        IsUsed = false;
    }
}
