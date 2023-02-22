using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Ground"))
            return;
        GameManager.instance.playerController.OnGround(true);
    }
    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Ground"))
            return;
        Debug.Log("false");
        GameManager.instance.playerController.OnGround(false);
    }
}
