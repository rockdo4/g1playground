using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Ground"))
            return;
        GameManager.instance.player.IsGrounded = true;
    }
    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Ground"))
            return;
        Debug.Log("false");
        GameManager.instance.player.IsGrounded = false;
    }
}
