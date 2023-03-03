using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMass : MonoBehaviour
{
    public float mass = 0f;
    public float Mass { get; set; }

    private void Awake()
    {
        if (gameObject.GetComponent<Rigidbody>() != null)
        {
            Mass = gameObject.GetComponent<Rigidbody>().mass;
            mass = Mass;
        }
    }

    public void AddMass(float mass)
    {
        Mass += mass;
    }

    public void ResetMass()
    {
        Mass = mass;
    }

}
