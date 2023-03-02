using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerConnecter : MonoBehaviour
{
    [SerializeField] private GameObject player;

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(gameObject.name);
        if (other.tag == "Pushable" )
        {
            //Debug.Log("connected");
            //Debug.Log(other.gameObject.name + " " + gameObject.name);
            //Debug.Log("True");
            other.GetComponent<ObjectMass>().AddMass(player.GetComponent<Rigidbody>().mass);
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Pushable" )
        {
            //transform.SetParent(null);
            //other.gameObject.transform.SetParent(null);
            other.GetComponent<ObjectMass>().ResetMass();
            //boxTile.IsConnected = false;
            //other.GetComponent<BoxTile>().connectedObject = null;
            //player.transform.SetParent(null);
        }
    }
}
