using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundCollider : MonoBehaviour
{
    private PlayerController playerController;
    private Rigidbody playerRb;
    private void Start()
    {
        playerController = GetComponentInParent<PlayerController>();
        playerRb = GetComponentInParent<Rigidbody>();
    }

    private void OnTriggerStay(Collider other)
    {
        PushPlayerDown(other);
        if (other.CompareTag("Ground") || other.CompareTag("Pushable") || other.CompareTag("Falling")) 
            playerController.OnGround(true);
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ground") || other.CompareTag("Pushable") || other.CompareTag("Falling")) 
        {
            playerController.OnGround(false);
        }
    }
    public void PushPlayerDown(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Bounds otherBounds = other.bounds;
            Bounds playerBounds = GetComponent<Collider>().bounds;
            if (playerBounds.min.y > otherBounds.center.y)
            {
                
                if (playerBounds.center.x <= otherBounds.center.x)
                {
                    playerRb.velocity = new Vector3(-5f , playerRb.velocity.y, 0f);
                }
                if (playerBounds.center.x > otherBounds.center.x)
                {
                    playerRb.velocity = new Vector3(5f , playerRb.velocity.y, 0f);
                }
            }
        }
    }

}
