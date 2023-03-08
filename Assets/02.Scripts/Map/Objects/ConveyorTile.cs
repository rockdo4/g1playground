using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorTile : MonoBehaviour
{
    public enum Direction
    {
        Left,
        Right,
    }

    [SerializeField] private float speed = 20f;
    [SerializeField] private Direction direction;

    private List<GameObject> movingObjects = new List<GameObject>();    
    private Vector3 dir;

    // Start is called before the first frame update
    void Start()
    {
        SetDirection();
    }

    // Update is called once per frame
    void Update()
    {
        if (movingObjects.Count > 0) 
        {
            foreach (var massObj in movingObjects)
            {
                massObj.GetComponent<Rigidbody>().velocity = speed * dir * Time.deltaTime;
                //Debug.Log(massObj.name);
                //if (massObj.GetComponent<BoxTile>() != null)
                //{
                //    //massObj.GetComponent<BoxTile>().rigidbody.velocity = speed * dir * Time.deltaTime;
                //} 
                
            }
        }
        
    }

    private void SetDirection()
    {
        switch (direction)
        {
            case Direction.Left:
                dir = Vector3.left;
                break;
            case Direction.Right:
                dir = Vector3.right;
                break;
        }
        Debug.Log(dir);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<ObjectMass>() != null) 
        {
            movingObjects.Add(collision.gameObject);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (movingObjects.Contains(collision.gameObject))
        {
            movingObjects.Remove(collision.gameObject);
        }
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.GetComponent<ObjectMass>() != null) 
    //    {
    //        //SetParent the Entered object
    //        movingObjects.Add(other.gameObject);

    //        ////turn off kinematic
    //        //if (other.CompareTag("Pushable") && !other.GetComponent<BoxTile>().IsPushing)
    //        //{
    //        //    other.GetComponent<Rigidbody>().isKinematic = true;
    //        //}

    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (movingObjects.Contains(other.gameObject)) 
    //    {
    //        movingObjects.Remove(other.gameObject);
    //    }

    //}
}
