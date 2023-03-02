using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundCollider : MonoBehaviour
{
    private PlayerController playerController;

    private void Start()
    {
        playerController = GetComponentInParent<PlayerController>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Ground") || other.CompareTag("Pushable")) 
            playerController.OnGround(true);
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ground") || other.CompareTag("Pushable"))
        {
            playerController.OnGround(false);
        }
    }
}
