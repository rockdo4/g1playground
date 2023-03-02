using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxTile : MonoBehaviour
{
    private new Rigidbody rigidbody;
    //[Header("Delay Push Time")]
    public float pushTime = 1f;
    private float timer = 0f;
    //[Header("Force")]
    [SerializeField] private float pushForce = 1f;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.tag == "Player")
    //    {
    //        timer += Time.deltaTime;
    //        if (timer >= pushTime)
    //        {
    //            rigidbody.mass = 1;
    //            //Push(other.transform.position);
    //        }
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.tag == "Player")
    //    {
    //        timer = 0f;
    //        rigidbody.mass = 100;
    //    }
    //}

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //timer to check player is pushing mover than pushTime
            timer += Time.deltaTime;
            if (timer >= pushTime)
            {
                Debug.Log("push");
                Vector3 pushDirection = transform.position - collision.gameObject.transform.position;
                pushDirection.y = 0;
                pushDirection.z = 0;
                pushDirection.Normalize();
                gameObject.GetComponent<Collider>().attachedRigidbody.velocity = pushDirection * pushForce;
                
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("exit");
            //Reset timer to make player has to push again to move the block
            timer = 0f;
        }
    }
}
