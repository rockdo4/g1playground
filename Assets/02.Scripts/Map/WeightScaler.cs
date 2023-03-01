using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class WeightScaler : MonoBehaviour
{
    [SerializeField] private Collider colliderTrigger;
    
    private float calculatedMass;
    public float CalculatedMass { get { return calculatedMass; } set { calculatedMass = value; } }

    
    public List<Rigidbody> objects = new List<Rigidbody>();
   
    public bool IsMovAble { get; set; }

   

    private void Awake()
    {
        IsMovAble = true;
        CalculatedMass = 0f;
    }

    //
    //private void UdateObjectMovement()
    //{
    //    if (objects != null) 
    //    {
    //        foreach (var obj in objects)
    //        {
    //            obj.velocity = gameObject.GetComponent<Rigidbody>().velocity;                                
    //        }
    //    }
    //}

    private void FixedUpdate()
    {
        float temp = 0f;

        foreach (var mass in objects)
        {
            temp += mass.mass;
            //Debug.Log(temp);
            //Debug.Log(mass.mass);
        }
        CalculatedMass = temp;
    }

    public void EnableTrigger()
    { 
        colliderTrigger.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ground")
        {
            IsMovAble = false;
            colliderTrigger.enabled = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.rigidbody != null)
        {
            if (collision.gameObject.tag == "Pushable")
            {
                Debug.Log("Tag");
                collision.gameObject.GetComponent<BoxTile>().IsConnected = true;
            }
            collision.transform.SetParent(transform);

            
            if (!objects.Contains(collision.rigidbody))
            {
                //CalculatedMass += collision.rigidbody.mass;
                objects.Add(collision.rigidbody);
            }
            
        }
    }

    private void OnCollisionExit(Collision collision)
    {     
        if (collision.rigidbody != null)
        {
            if (collision.gameObject.tag == "Pushable")
            {
                collision.gameObject.GetComponent<BoxTile>().IsConnected = false;
            }
            collision.transform.SetParent(null);


            if (objects.Contains(collision.rigidbody))
            {
                //CalculatedMass -= collision.rigidbody.mass;
                objects.Remove(collision.rigidbody);
            }      
        }
    }
}
