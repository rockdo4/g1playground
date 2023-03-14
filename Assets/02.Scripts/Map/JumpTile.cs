using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpTile : MonoBehaviour
{
    [SerializeField] private float force = 10f;
    [SerializeField] private float delay = 0.5f;
    private float timer = 0f;

    private Vector3 boxSize;

    private void Start()
    {
        boxSize = gameObject.GetComponent<BoxCollider>().size;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (Physics.BoxCast(transform.position, boxSize / 2, Vector3.up, Quaternion.identity, 1f, LayerMask.GetMask("Player")) &&
            collision.gameObject.GetComponent<ObjectMass>() != null) 
        {
            //timer to check player is pushing mover than pushTime
            timer += Time.deltaTime;

            if (timer >= delay) 
            {
                Debug.Log("Jump");
                //Add player jump here//
                collision.gameObject.GetComponent<PlayerController>().Jump(force);
               
                timer = 0f;
            }
        }
    }
}
