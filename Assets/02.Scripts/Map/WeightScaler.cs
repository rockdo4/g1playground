using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightScaler : MonoBehaviour
{
    float forceToMass;

    private float combinedForce;

    [SerializeField] private float calculatedMass;
    public float CalculatedMass { get { return calculatedMass; } set { calculatedMass = value; } }

    //private int registeredRigidbodies;

    private Dictionary<Rigidbody, float> impulsePerRigidBody = new Dictionary<Rigidbody, float>();

    private float currentDeltaTime;
    private float lastDeltaTime;

    public bool IsMovAble { get; set; }
    public bool IsMovUp { get; set; }
    public bool IsMovDown { get; set; }

    private void Awake()
    {
        forceToMass = 1f / Physics.gravity.magnitude;
        IsMovUp = false;
        IsMovDown = false;
        IsMovAble = true;
    }

    void UpdateWeight()
    {
        //registeredRigidbodies = impulsePerRigidBody.Count;
        combinedForce = 0;

        foreach (var force in impulsePerRigidBody.Values)
        {
            combinedForce += force;
        }

        calculatedMass = (float)(combinedForce * forceToMass);
    }

    //
    private void UdateObjectMovement()
    {
        if (IsMovUp)
        {
            foreach (var obj in impulsePerRigidBody.Keys)
            {
                obj.gameObject.transform.parent = transform;
                
            }
        }
        else if (IsMovDown)
        {
            
            foreach (var obj in impulsePerRigidBody.Keys)
            {
                obj.gameObject.transform.parent = transform;
                
            }  
        }
    }

    private void FixedUpdate()
    {
        lastDeltaTime = currentDeltaTime;
        currentDeltaTime = Time.deltaTime;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.rigidbody != null)
        {
            if (impulsePerRigidBody.ContainsKey(collision.rigidbody))
                impulsePerRigidBody[collision.rigidbody] = collision.impulse.y / lastDeltaTime;
            else
                impulsePerRigidBody.Add(collision.rigidbody, collision.impulse.y / lastDeltaTime);

            UpdateWeight();
            UdateObjectMovement();
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.rigidbody != null)
        {
            if (collision.gameObject.tag == "Ground")
            {
                Debug.Log("enter");
                IsMovAble = false;
                return;
            }
            
            if (impulsePerRigidBody.ContainsKey(collision.rigidbody))
                impulsePerRigidBody[collision.rigidbody] = collision.impulse.y / lastDeltaTime;
            else
                impulsePerRigidBody.Add(collision.rigidbody, collision.impulse.y / lastDeltaTime);

            UpdateWeight();
            UdateObjectMovement();
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.rigidbody != null)
        {
            if (collision.gameObject.tag == "Ground")
            {
                Debug.Log("exit");
                IsMovAble = true;
                return;
            }
            impulsePerRigidBody.Remove(collision.rigidbody);
            UpdateWeight();
            //UdateObjectMovement();
        }
    }
}
