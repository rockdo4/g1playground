using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnGround : MonoBehaviour
{
    private CapsuleCollider onGround;
    public bool isGround = false;
    private void Awake()
    {
        onGround = GetComponent<CapsuleCollider>();
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag != "Ground")
        {
            isGround = false;
            return;
        }
        else
            isGround = true;



    }
}
