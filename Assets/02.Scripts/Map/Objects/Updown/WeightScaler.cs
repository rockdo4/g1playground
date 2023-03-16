using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class WeightScaler : MonoBehaviour
{
    [SerializeField] private UpDownDetecter detecter;
    
    private float calculatedMass;
    public float CalculatedMass { get { return calculatedMass; } set { calculatedMass = value; } }

    public bool IsMovAble { get; set; }

    private void Awake()
    {
        IsMovAble = true;
        CalculatedMass = 0f;
    }

    private void OnEnable()
    {
        IsMovAble = true;
        CalculatedMass = 0f;
    }

    private void OnDisable()
    {
        //SetParent the Entered object
        GameManager.instance.player.transform.SetParent(null);
    }

    private void FixedUpdate()
    {
        //Find objects with Mass in Children
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
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.rigidbody != null)
        {
            //SetParent the Entered object
            collision.transform.SetParent(transform);

            //turn off kinematic
            if (collision.gameObject.CompareTag("Pushable") && collision.gameObject.GetComponent<BoxTile>() != null) 
            {
                if (!collision.gameObject.GetComponent<BoxTile>().IsPushing)
                {
                    collision.rigidbody.isKinematic = true;
                }
                
            }

        }
    }

    private void OnCollisionExit(Collision collision)
    {     
        if (collision.rigidbody != null)
        { 
            if (collision.gameObject.CompareTag("Player"))
            {
                collision.transform.SetParent(null);
            }
        }
    }

    public void ResetWeight()
    {
        IsMovAble = true;
        CalculatedMass = 0f;
    }
}
