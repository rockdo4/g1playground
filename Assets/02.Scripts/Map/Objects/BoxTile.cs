using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.Video;

public class BoxTile : ObjectTile
{
    private new Rigidbody rigidbody;
    private Vector3 boxSize;

    [SerializeField] private float pushTime = 1f;
    private float timer = 0f;
    private float detachtimer = 0f;
    [SerializeField] private float pushForce = 1f;

    private bool detachStarted;


    public bool IsPushing { get; set; }

    private void Awake()
    {
        originPos = transform.position;
        rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        //rigidbody = GetComponent<Rigidbody>();
        IsPushing = false;
        boxSize = gameObject.GetComponent<BoxCollider>().size;
    }

    protected override void OnEnable()
    {
        transform.position = originPos;
        rigidbody.isKinematic = false;
        IsPushing = false;
        timer = 0f;
    }

    private void FixedUpdate()
    {
        if (GameManager.instance.player.GetComponent<PlayerController>().moveX == 0)
            timer = 0;
        if (detachStarted)
        {
            detachtimer+= Time.deltaTime;
            if (detachtimer > 0.4f)
            {
                timer = 0;
            }
        }

    
    }

    public override void ResetObject()
    {
        transform.position = originPos;
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.isKinematic = false;
        IsPushing = false;
        timer = 0f;
    }

    public override void SetOriginPos()
    {
        base.SetOriginPos();
    }

    public void SetKinematic(bool isKinematic)
    {
        rigidbody.isKinematic = isKinematic;
    }


    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.transform.position.y >transform.position.y)
                return;
            detachStarted = false;
            detachtimer = 0f;
            //timer to check player is pushing mover than pushTime
            timer += Time.deltaTime;

            if (timer >= pushTime)
            {

                //Raycast to check if player is pushing the box on side
                if (collision.gameObject.GetComponent<PlayerController>().moveX != 0f)
                {

                    IsPushing = true;
                    rigidbody.isKinematic = false;
                    ChangeParent();
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

    void ChangeParent()
    {
        if (transform.parent == null)
        {
            return;
        }
        if (transform.parent.GetComponent<BoxTile>() != null)
        {
            transform.parent = transform.parent.parent;
            if(transform.parent.GetComponent<BoxTile>() != null){
                ChangeParent();
            }
        }
        
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //Reset timer to make player has to push again to move the block
            detachStarted = true;
        }
    }
}
