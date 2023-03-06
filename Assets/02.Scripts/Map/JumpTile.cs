using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpTile : MonoBehaviour
{
    [SerializeField] private float force = 10f;
    [SerializeField] private float delay = 0.5f;
    private float timer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //timer to check player is pushing mover than pushTime
            timer += Time.deltaTime;

            if (timer >= delay && Physics.Raycast(transform.position, Vector3.up, 1f, LayerMask.GetMask("Player"))) 
            {
                Debug.Log("jump");
                collision.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * force, ForceMode.Impulse);
                timer = 0f;
            }
        }
    }

    //private void OnCollisionExit(Collision collision)
    //{
    //    if (collision.gameObject.tag == "Player")
    //    {
    //        //Reset timer to make player has to push again to move the block
    //        timer = 0f;
    //    }
    //}
}
