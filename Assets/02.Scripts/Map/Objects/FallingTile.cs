using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingTile : MonoBehaviour
{
    [SerializeField] private new Collider collider;
    private Animator animator;
    private Rigidbody rb;
    //private new BoxCollider collider;

    [SerializeField] private float delay = 1f;
    [SerializeField] private float destroyDelay = 3f;
    private float timer = 0f;
    private bool trigger = false;

    private Vector3 originPos;
    private Vector3 boxScale;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();        
       
    }

    // Update is called once per frame
    void Update()
    {
        if (trigger)
        {
            timer += Time.deltaTime;
            if (timer >= delay) 
            {
                timer = 0f;
                //gameObject.SetActive(false);
                collider.isTrigger = trigger;
                rb.isKinematic = false;
                trigger = false;
                Destroy(gameObject, destroyDelay);
            }
        }        
    }

    public void SetTrigger(bool isTrigger)
    {
        trigger = isTrigger;
    }    

    //private void OnDrawGizmos()
    //{
    //    Gizmos.DrawWireCube(originPos, boxScale);
    //}

    //private void OnBecameInvisible()
    //{
    //    Debug.Log("destroy");
    //    Destroy(gameObject);
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pushable") || other.CompareTag("Player") || other.CompareTag("Enemy"))
        {
            if (transform.position.y < other.transform.position.y) 
            {
                //Debug.Log(other.tag);
                trigger = true;
            }
           
        }
    }
}
