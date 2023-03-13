using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxConnecter : MonoBehaviour
{
    [SerializeField] private BoxTile boxTile;
    private GameObject connectedObject;

    private void OnTriggerEnter(Collider other)
    {
        if (connectedObject != null)
        {
            return;
        }
        //check if there is Pushable object under
        if (other.CompareTag("Pushable") && other.GetComponent<BoxTile>() != null)
        {
            boxTile.transform.SetParent(other.transform);
            connectedObject = other.gameObject;

        }
        else if (other.CompareTag("Ground") && !(other.GetComponent<FallingTile>() != null))     //check if the box is on Ground
        {
            //Debug.Log("weight");
            //SetParent to Ground in Case of Box on top of UpDown Tile
            boxTile.transform.SetParent(other.transform);
            connectedObject = other.gameObject;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        //Reset when exit
        if (other.CompareTag("Pushable") || (other.CompareTag("Ground") && !(other.GetComponent<FallingTile>() != null)))
        {
            if (other.gameObject == connectedObject)
            {
                //Debug.Log("exit " + other.gameObject.name);
                boxTile.transform.SetParent(null);
                connectedObject = null;
            }

        }
    }
}
