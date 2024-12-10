using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FallingTile : ObjectTile
{
    [SerializeField] private new Collider collider;
    private Rigidbody rb;
    

    [SerializeField] private float delay = 1f;
    [SerializeField] private float destroyDelay = 3f;
    private float timer = 0f;
    private bool trigger = false;

    private Vector3 boxScale;

    private void Awake()
    {
        originPos = transform.position;
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
                collider.isTrigger = trigger;
                rb.isKinematic = false;
                trigger = false;

                StartCoroutine(CoActiveFalse());
            }
        }        
    }

    private IEnumerator CoActiveFalse()
    {
        //wait for delay after falling then set active to false
        yield return new WaitForSeconds(destroyDelay);
        rb.isKinematic = true;
        gameObject.SetActive(false);
        StopAllCoroutines();
    }

    protected override void OnEnable()
    {
        timer = 0f;
        StopAllCoroutines();
        transform.position = originPos;
        trigger = false;
        collider.isTrigger = trigger;
        rb.isKinematic = true;
    }

    public override void ResetObject()
    {
        timer = 0f;
        StopAllCoroutines();
        transform.position = originPos;
        trigger = false;
        collider.isTrigger = trigger;
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }

    public override void SetOriginPos()
    {
        base.SetOriginPos();
    }

    public void SetTrigger(bool isTrigger)
    {
        trigger = isTrigger;
    }    

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
