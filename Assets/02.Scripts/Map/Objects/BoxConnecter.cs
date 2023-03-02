using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxConnecter : MonoBehaviour
{
    [SerializeField] private BoxTile boxTile;

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(gameObject.name);
        if (other.tag == "Pushable") 
        {
            //Debug.Log(other.gameObject.name + " " + gameObject.name);
            //Debug.Log("True");
            other.gameObject.GetComponent<ObjectMass>().AddMass(boxTile.GetComponent<ObjectMass>().Mass);
            boxTile.IsConnected = other.GetComponent<BoxTile>().IsConnected;
            //other.GetComponent<BoxTile>().SetConnectedBox(boxTile.gameObject);
            boxTile.transform.SetParent(other.transform);
            //Debug.Log("True");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Pushable") 
        {
            //transform.SetParent(null);
            //other.gameObject.transform.SetParent(null);
            other.GetComponent<ObjectMass>().ResetMass();
            boxTile.IsConnected = false;
            //other.GetComponent<BoxTile>().connectedObject = null;
            boxTile.transform.SetParent(null);
        }

        if (other.GetComponent<WeightScaler>() != null )
        {
            Debug.Log("exit");
            //boxTile.SetKinematic(false);
            //boxTile.transform.SetParent(null);
        }
    }
}
