using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnGround : MonoBehaviour
{
    private Collider onGround;
    public bool isGround = false;
    private void Awake()
    {
        onGround = GetComponent<Collider>();
    }

    //private void OnTriggerEnter(Collider collider)
    //{
    //    if (collider.tag != "Ground")
    //    {
    //        isGround = false;
    //        return;
    //    }
    //    else
    //        isGround = true;
    //}
    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Ground"))
            return;
        isGround = true;
    }
    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Ground"))
            return;
        isGround = false;
    }
}
