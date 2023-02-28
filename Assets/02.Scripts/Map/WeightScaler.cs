using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class WeightScaler : MonoBehaviour
{
    //private FixedJoint joint;

    //float forceToMass;

    private float combinedForce;

    private float calculatedMass;
    public float CalculatedMass { get { return calculatedMass; } set { calculatedMass = value; } }

    //private int registeredRigidbodies;

    public List<Rigidbody> objects = new List<Rigidbody>();

    //[SerializeField] private float speed = 1f;
    //public float Speed { get { return speed; } }

    private float currentDeltaTime;
    private float lastDeltaTime;

    public bool IsMovAble { get; set; }
    public bool IsMovUp { get; set; }
    public bool IsMovDown { get; set; }

    private void Awake()
    {
        //joint = GetComponent<FixedJoint>();
       
        //forceToMass = 1f / Physics.gravity.magnitude;
        IsMovUp = false;
        IsMovDown = false;
        IsMovAble = true;
    }

    //
    private void UdateObjectMovement()
    {
        if (objects != null) 
        {
            foreach (var obj in objects)
            {
                obj.velocity = gameObject.GetComponent<Rigidbody>().velocity;
                //obj.MovePosition(obj.position + Vector3.up * 3f * Time.fixedDeltaTime);

            }
        }

    }

    private void FixedUpdate()
    {
        lastDeltaTime = currentDeltaTime;
        currentDeltaTime = Time.deltaTime;
        //aUdateObjectMovement();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ground")
        {
            Debug.Log("ground");
            IsMovAble = false;

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Ground")
        {
            //Debug.Log("ground");
            IsMovAble = true;

        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.rigidbody != null)
        {
            collision.transform.SetParent(transform);
            calculatedMass += collision.rigidbody.mass;
            //joint.connectedBody = collision.rigidbody;
            objects.Add(collision.rigidbody);

        }
    }
    private void OnCollisionExit(Collision collision)
    {     
        if (collision.rigidbody != null)
        {
            Debug.Log("exit");
            //joint.connectedBody = null;
            collision.transform.SetParent(null);
            calculatedMass -= collision.rigidbody.mass;
            objects.Remove(collision.rigidbody);
        }
    }
}
