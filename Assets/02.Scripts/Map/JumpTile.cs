using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpTile : MonoBehaviour
{
    [SerializeField] private float force = 10f;
    [SerializeField] private float delay = 0.5f;
    private float timer = 0f;

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Player" && collision.gameObject.GetComponent<ObjectMass>() != null) 
        {
            //timer to check player is pushing mover than pushTime
            timer += Time.deltaTime;

            if (timer >= delay && Physics.Raycast(transform.position, Vector3.up, 1f, LayerMask.GetMask("Player"))) 
            {
                //Debug.Log("jump");
                //Add player jump here//
                collision.gameObject.GetComponent<PlayerController>().Jump(force, true);
                                                                     //.AddForce(Vector3.up * force, ForceMode.Impulse); // Replace this line
                ///////////////////////

                timer = 0f;
            }
        }
    }
}
