using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxTile : MonoBehaviour, IResetObject
{
    private Vector3 originPos;

    private new Rigidbody rigidbody;
    //private float originMass;
    private Vector3 boxSize;
    [SerializeField] private float pushTime = 1f;
    private float timer = 0f;

    [SerializeField] private float pushForce = 1f;


    public bool IsPushing { get; set; }

    private void Awake()
    {
        originPos = transform.position;
        rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        //originMass = rigidbody.mass;
        rigidbody = GetComponent<Rigidbody>();
        IsPushing = false;
        boxSize = gameObject.GetComponent<BoxCollider>().size;
    }

    private void OnEnable()
    {
        transform.position = originPos;
        rigidbody.isKinematic = false;
        IsPushing = false;

    }

    //public void AddMass(float mass)
    //{
    //    rigidbody.mass += mass;
    //}

    //public void RemoveMass()
    //{
    //    rigidbody.mass = originMass;
    //}

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
        if (collision.gameObject.CompareTag("Player"))
        {
            //timer to check player is pushing mover than pushTime
            timer += Time.deltaTime;

            if (timer >= pushTime)
            {
                //Raycast to check if player is pushing the box on side
                if (!Physics.BoxCast(transform.position, boxSize / 2, Vector3.up, Quaternion.identity, 1f, LayerMask.GetMask("Player"))
                    && collision.gameObject.GetComponent<PlayerController>().moveX != 0f)
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

    public void ResetObject()
    {
        transform.position = originPos;
        if (gameObject.activeSelf)
            rigidbody.isKinematic = false;
        IsPushing = false;
    }

    public void ActiveSelfCheck()
    {

    }
}
