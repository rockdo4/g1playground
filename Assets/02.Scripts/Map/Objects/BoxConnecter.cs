using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxConnecter : MonoBehaviour
{
    [SerializeField] private BoxTile boxTile;
    public GameObject connectedObject;

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(gameObject.name);
        if (other.tag == "Pushable" && other.GetComponent<BoxTile>() != null) 
        {
            //Debug.Log(other.gameObject.name + " " + gameObject.name);
            //Debug.Log("True");
            //other.gameObject.GetComponent<ObjectMass>().AddMass(boxTile.GetComponent<ObjectMass>().Mass);
            boxTile.IsConnected = other.GetComponent<BoxTile>().IsConnected;
            //other.GetComponent<BoxTile>().SetConnectedBox(boxTile.gameObject);
            boxTile.transform.SetParent(other.transform);
            connectedObject = other.gameObject;
            //Debug.Log("True");
            //boxTile.GetComponent<Rigidbody>().isKinematic = true;
        }
        else if (other.tag == "Ground")
        {
            boxTile.transform.SetParent(other.transform);
            connectedObject = other.gameObject;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Pushable" || other.tag == "Ground") 
        {
            if (other.gameObject == connectedObject)
            {
                Debug.Log("exit");
                boxTile.IsConnected = false;
                boxTile.transform.SetParent(null);
                connectedObject = null;
            }
            
            //transform.SetParent(null);
            //other.gameObject.transform.SetParent(null);
            //other.GetComponent<ObjectMass>().ResetMass();
            
            //other.GetComponent<BoxTile>().connectedObject = null;
            //boxTile.transform.SetParent(null);
            //boxTile.GetComponent<Rigidbody>().isKinematic = false;
        }


        if (other.GetComponent<WeightScaler>() != null )
        {
            //Debug.Log("exit");
            //boxTile.SetKinematic(false);
            //boxTile.transform.SetParent(null);
        }
    }
}
