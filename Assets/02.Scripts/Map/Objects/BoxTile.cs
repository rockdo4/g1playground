using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxTile : MonoBehaviour
{
    private new Rigidbody rigidbody;
    public Collider colliderTrigger;
    private float originMass;
    public GameObject connectedObject;

    [SerializeField] private float pushTime = 1f;
    private float timer = 0f;
    
    [SerializeField] private float pushForce = 1f;

    public bool IsConnected { get; set; }

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        originMass = rigidbody.mass;
        IsConnected = false;
    }

    public void AddMass(float mass)
    {
        rigidbody.mass += mass;
    }

    public void RemoveMass()
    {
        rigidbody.mass = originMass;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Pushable")
        {
            //Debug.Log("True");
            if (other.gameObject.GetComponent<BoxTile>().IsConnected)
            {
                Debug.Log(other.gameObject .name+ " "+gameObject.name);
                //Debug.Log("True");
                other.gameObject.GetComponent<BoxTile>().AddMass(rigidbody.mass);
                IsConnected = other.GetComponent<BoxTile>().IsConnected;
                connectedObject = other.gameObject;
                //if (connectedObject == null)
                //{
                    
                //}
                
            }
           
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Pushable")
        {
            //transform.SetParent(null);
            //other.gameObject.transform.SetParent(null);
            other.GetComponent<BoxTile>().RemoveMass();
            IsConnected = false;
            connectedObject = null;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //timer to check player is pushing mover than pushTime
            timer += Time.deltaTime;

            if (timer >= pushTime)
            {
                Vector3 pushDirection = transform.position - collision.gameObject.transform.position;
                pushDirection.z = 0;
                pushDirection.Normalize();
                gameObject.GetComponent<Collider>().attachedRigidbody.velocity = pushDirection * pushForce;
                
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //Reset timer to make player has to push again to move the block

            timer = 0f;
        }
    }
}
