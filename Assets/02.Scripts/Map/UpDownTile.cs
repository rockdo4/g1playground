using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpDownTile : MonoBehaviour
{
    [SerializeField] private GameObject blockA;
    [SerializeField] private GameObject blockB;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("enter");
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("trigger");
    }
}
