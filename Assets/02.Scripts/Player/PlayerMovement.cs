using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    private Vector3 inputVec;
    private Rigidbody playerRb;

    public float jumpPower;
    public float speed;
    public float moveX;

    private void Awake()
    {  
        playerRb = GetComponent<Rigidbody>();
    }
    //private void FixedUpdate()
    //{
    //    transform.position += inputVec.normalized * speed * Time.fixedDeltaTime;
    //}
    //public void OnMove(InputValue value)
    //{
    //    inputVec = value.Get<Vector2>();
    //}
    private void Update()
    {
        Jump();
    }
    public void FixedUpdate()
    {
        transform.position += new Vector3(moveX, 0, 0) * speed * Time.fixedDeltaTime;
    }
    public void Move(float moveX)
    {
        if (moveX < 0)
            transform.eulerAngles = new Vector3(0, 180, 0);
        else if(moveX>0)
            transform.eulerAngles = new Vector3(0, 0, 0);
        this.moveX = moveX; 
    }
    public void Jump()
    {
        foreach (var t in Input.touches)
        {
            var viewportPoint = Camera.main.ScreenToViewportPoint(t.position);
           
            if (viewportPoint.x > 0.5f) 
            {
                if(t.phase == TouchPhase.Began)
                playerRb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
                
            }
        }
    }
}
