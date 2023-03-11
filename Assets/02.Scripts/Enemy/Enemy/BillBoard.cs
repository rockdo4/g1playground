using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillBoard : MonoBehaviour
{
    private void Start()
    {
        Vector3 pos = GetComponentInParent<CapsuleCollider>().bounds.center;
        pos.y = GetComponentInParent<CapsuleCollider>().bounds.max.y + 0.5f;
        transform.position = pos;

        //transform.position = GetComponentInParent<CapsuleCollider>().bounds.max;
        //transform.position.x = GetComponentInParent<CapsuleCollider>().center.x;
    }
    private void LateUpdate()
    {
        transform.LookAt(transform.position + Camera.main.transform.forward);
    }
}
