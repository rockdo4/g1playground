using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxTile : MonoBehaviour
{
    private new Rigidbody rigidbody;
    private float originMass;

    [SerializeField] private float pushTime = 1f;
    private float timer = 0f;
    
    [SerializeField] private float pushForce = 1f;

   
    public bool IsPushing { get; set; }

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        originMass = rigidbody.mass;
       
        IsPushing = false;
    }

    public void AddMass(float mass)
    {
        rigidbody.mass += mass;
    }

    public void RemoveMass()
    {
        rigidbody.mass = originMass;
    }

    public void MoveBlock(Vector3 dir, float speed)
    {

        gameObject.GetComponent<Rigidbody>().MovePosition(gameObject.GetComponent<Rigidbody>().position + dir * speed * Time.fixedDeltaTime);
    }

    public void SetKinematic(bool isKinematic)
    {
        rigidbody.isKinematic = isKinematic;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //timer to check player is pushing mover than pushTime
            timer += Time.deltaTime;
            
            if (timer >= pushTime)
            {
                //Raycast to check if player is pushing the box on side
                if (!Physics.Raycast(transform.position, Vector3.up, 1f, LayerMask.GetMask("Player")))
                {
                    
                    IsPushing = true;
                    rigidbody.isKinematic = false;

                    //find direction and push
                    Vector3 pushDirection = transform.position - collision.gameObject.transform.position;
                    pushDirection.y = 0;
                    pushDirection.z = 0;
                    pushDirection.Normalize();
                    gameObject.GetComponent<Collider>().attachedRigidbody.velocity = pushDirection * pushForce;
                }               
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {

            //Reset timer to make player has to push again to move the block
            timer = 0f;
        }
    }
}
