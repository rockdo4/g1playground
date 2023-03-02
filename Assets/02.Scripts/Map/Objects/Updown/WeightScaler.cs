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

        var objs = gameObject.GetComponentsInChildren<ObjectMass>();
        
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
