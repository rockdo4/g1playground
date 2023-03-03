using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxConnecter : MonoBehaviour
{
    [SerializeField] private BoxTile boxTile;
    private GameObject connectedObject;

    private void OnTriggerEnter(Collider other)
    {
        //check if there is Pushable object under
        if (other.tag == "Pushable" && other.GetComponent<BoxTile>() != null) 
        {  
            boxTile.transform.SetParent(other.transform);
            connectedObject = other.gameObject;
            
        }
        else if (other.tag == "Ground")     //check if the box is on Ground
        {
            //SetParent to Ground in Case of Box on top of UpDown Tile
            boxTile.transform.SetParent(other.transform);
            connectedObject = other.gameObject;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        //Reset when exit
        if (other.tag == "Pushable" || other.tag == "Ground") 
        {
            if (other.gameObject == connectedObject)
            {
                boxTile.transform.SetParent(null);
                connectedObject = null;
            }
        }
    }
}
