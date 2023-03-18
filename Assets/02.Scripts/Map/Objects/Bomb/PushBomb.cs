using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushBomb : ObjectTile
{
    [SerializeField] private float pushTime = 1f;
    private float timer = 0f;

    [SerializeField] private float pushForce = 1f;

    private void Awake()
    {
        originPos = transform.position;
    }

    protected override void OnEnable()
    {
        //reset position and material when Reset the stage
        timer = 0f;
        transform.position = originPos;
        TileColorManager.instance.ToInvisibleMaterial(gameObject);
    }

    public override void ResetObject()
    {
        timer = 0f;
        transform.position = originPos;
        TileColorManager.instance.ToInvisibleMaterial(gameObject);
    }

    public override void SetOriginPos()
    {
        base.SetOriginPos();
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //timer to check player is pushing mover than pushTime
            timer += Time.deltaTime;

            if (timer >= pushTime)
            {
                timer = 0f;
                //Raycast to check if player is pushing the box on side
                if (!Physics.Raycast(transform.position, Vector3.up, 1f, LayerMask.GetMask("Player")))
                {
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
