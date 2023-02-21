using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private Rigidbody playerRb;

    [Header("Move")]
    public float speed;
    private float defaultSpeed = 10f;
    public float moveX;

    [Header("Dash")]
    public float dashSpeed;
    public float dashTime;
    private float leftButtonTime;
    private float rightButtonTime;

    [Header("Jump")]
    public float jumpPower;
    public int jumpCount;

    public bool IsMoving { get; private set; } //move
    public bool IsJumping { get; private set; } //jump
    public bool IsDash { get; set; } //dash
    public bool IsBorder { get; private set; }
    public bool IsGrounded { get; private set; }
    private void Awake()
    {  
        playerRb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
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
        if (IsJumping && playerRb.velocity.y < 0)
            GroundRay();

        if (jumpCount > 0) 
            Jump();
        if(IsGrounded)
        {
            jumpCount = 2;
            IsJumping = false;
        }

        Dash();

        if (moveX ==0)
        {
            IsMoving = false;
        }
        else
        {
            IsMoving = true;
        }
    }
    private void FixedUpdate()
    {
        StopRay();
        if(!IsBorder)
            transform.position += new Vector3(moveX, 0, 0) * speed * Time.fixedDeltaTime;
    }
    public void Move(float moveX)
    {
        this.moveX = moveX;
        if (moveX < 0)
            transform.eulerAngles = new Vector3(0, 180, 0);
        else if (moveX > 0)
            transform.eulerAngles = new Vector3(0, 0, 0);
    }
    public void Dash()
    {
        if(IsDash)
        {
            speed = dashSpeed;
            dashTime = 0.1f;
            IsDash = false;
        }
        
        if(dashTime <0)
        {
            speed = defaultSpeed;
        }

        dashTime -= Time.deltaTime;
    }
    public void Jump()
    {
        foreach (var t in Input.touches)
        {
            var viewportPoint = Camera.main.ScreenToViewportPoint(t.position);
           
            if (viewportPoint.x > 0.5f&&viewportPoint.y <0.5f) 
            {
                if (t.phase == TouchPhase.Began)
                {
                    playerRb.velocity = new Vector3 (moveX,0,0);
                    playerRb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
                    IsJumping = true;
                    IsGrounded = false;
                    jumpCount--;
                }
            }
        }
        
    }
    
    public void StopRay()
    {
        IsBorder = Physics.Raycast(transform.position,
            new Vector3(moveX, 0, 0), 1, LayerMask.GetMask("Enemy"));
    }
    public void GroundRay()
    {
        Debug.DrawRay(transform.position,
            new Vector3(0, -1f, 0), Color.green);
        IsGrounded = Physics.Raycast(transform.position,
            new Vector3(0, -1f, 0), 1, LayerMask.GetMask("Ground"));
        Debug.Log(IsGrounded);
    }
}
