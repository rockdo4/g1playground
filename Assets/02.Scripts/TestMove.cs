using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMove : MonoBehaviour
{
    public new Rigidbody rigidbody;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.D))
        {
            rigidbody.AddForce(new Vector3(30, 0));
        }
        if (Input.GetKey(KeyCode.A))
        {
            rigidbody.AddForce(new Vector3(-30, 0));
        }
        if (Input.GetKey(KeyCode.Space))
        {
            rigidbody.AddForce(new Vector3(0, 30));
        }
    }
}
