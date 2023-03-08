using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
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

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector3 GetSpawnPoint()
    {
        return spawnPoint.transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !IsUsed)
        {
            IsUsed = true;                        
            linkedPortal.IsUsed = true;
            other.gameObject.transform.position = linkedPortal.GetSpawnPoint();
            other.GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, 0f);

        }
    }

    private void OnTriggerExit(Collider other)
    {
        IsUsed = false;
    }
}
