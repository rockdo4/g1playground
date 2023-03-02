using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class WeightScaler : MonoBehaviour
{
    [SerializeField] private UpDownDetecter detecter;
    
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

        //foreach (var mass in objects)
        //{
        //    if (mass.gameObject.GetComponent<ObjectMass>() != null) 
        //    {

        //        temp += mass.gameObject.GetComponent<ObjectMass>().Mass;
        //    }
            
        //    //Debug.Log(temp);
        //    //Debug.Log(mass.mass);
        //}
        var objs = gameObject.GetComponentsInChildren<ObjectMass>();
        //Debug.Log(objs.Length);
        
        foreach (var mass in objs)
        {
            temp += mass.Mass;
        }
        CalculatedMass = temp;
    }

    public void EnableTrigger()
    {
        detecter.EnableTrigger();
        //colliderTrigger.enabled = true;
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.tag == "Ground" || other.tag == "Pushable") 
    //    {
    //        IsMovAble = false;
    //        colliderTrigger.enabled = false;
    //    }
    //    if (other.tag == "Player" || other.tag == "Pushable") 
    //    {
    //        Debug.Log(other.tag);
    //        IsMovAble = false;

    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.tag == "Player" || other.tag == "Pushable") 
    //    {
    //        IsMovAble = true;

    //    }
    //}

    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.rigidbody != null)
        {
            
            collision.transform.SetParent(transform);

            
            if (!objects.Contains(collision.rigidbody))
            {
                //CalculatedMass += collision.rigidbody.mass;
                objects.Add(collision.rigidbody);
            }

            if (collision.gameObject.tag == "Pushable" && !collision.gameObject.GetComponent<BoxTile>().IsPushing)
            {
                
                collision.rigidbody.isKinematic = true;
                collision.gameObject.GetComponent<BoxTile>().IsConnected = true;
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
            //collision.transform.SetParent(null);
            if (collision.gameObject.tag == "Player")
            {
                collision.transform.SetParent(null);
            }

            if (objects.Contains(collision.rigidbody))
            {
                //CalculatedMass -= collision.rigidbody.mass;
                objects.Remove(collision.rigidbody);
            }      
        }
    }
}
