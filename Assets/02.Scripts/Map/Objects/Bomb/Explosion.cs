using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    private List<GameObject> objects = new List<GameObject>();
    [SerializeField] private float force = 10f;
    [SerializeField] private float damagePercentage = 10f;


    private void OnDisable()
    {
        objects.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<ObjectMass>() != null)
        {
            //Check if object been Pushed
            if (objects.Contains(other.gameObject))
            {
                return;
            }
            //calculate direction
            var dir = other.transform.position - transform.position;
            dir.z = 0;
            dir.Normalize();

            //push object
            other.GetComponent<Rigidbody>().AddForce(dir * force, ForceMode.VelocityChange);

            objects.Add(other.gameObject);

            if (other.CompareTag("Player")) 
            {
                //Give player Damage here
            }
        }
    }

}
