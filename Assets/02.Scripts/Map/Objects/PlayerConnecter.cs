using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerConnecter : MonoBehaviour
{
    [SerializeField] private GameObject player;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Pushable" )
        {
            other.GetComponent<ObjectMass>().AddMass(player.GetComponent<Rigidbody>().mass);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Pushable" )
        {   
            other.GetComponent<ObjectMass>().ResetMass(); 
        }
    }
}
