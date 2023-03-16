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
        if (other.CompareTag("Enemy"))
            PushPlayerSide(other);
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
    public void PushPlayerSide(Collider other)
    {
        Bounds otherBounds = other.bounds;
        Bounds playerBounds = GetComponent<Collider>().bounds;
        if (playerBounds.min.y > otherBounds.center.y)
        {

            if (playerBounds.min.x <= otherBounds.center.x)
            {
                //playerRb.velocity = new Vector3(-5f ,5f, 0f);
                playerRb.AddForce(new Vector3(-5f, 2f, 0f), ForceMode.Impulse);
            }
            else if (playerBounds.min.x > otherBounds.center.x)
            {
                //playerRb.velocity = new Vector3(5f , 5f, 0f);
                playerRb.AddForce(new Vector3(5f, 2f, 0f), ForceMode.Impulse);
            }
        }
    }


}
