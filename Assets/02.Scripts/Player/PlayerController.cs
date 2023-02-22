using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public abstract class State
    {
        protected PlayerController playerController;
        public State(PlayerController controller)
        {
            playerController = controller;
            Enter();
        }
        protected abstract void Enter();
        public abstract void Update();
        public abstract void Exit();
    }

    public State currState;
    private Rigidbody playerRb;
    public float moveX;
    public float moveSpeed = 10f;
    public float dashSpeed;

    public bool DashOnCool { get; private set; }
    public float dashCoolDown;
    private float dashCoolTimer;
    public float dashDuration = 0.1f;
    private float dashTimer;
    public bool IsBlocked { get; private set; }

    public float jumpForce;
    public int jumpCount;
    public int maxJumpCount;
    public bool isGrounded;

    public BasicAttack basicAttackTemp;

    private void SetState(State state)
    {
        if (currState != null)
            currState.Exit();
        currState = state;
    }

    private void Awake()
    {
        playerRb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        SetState(new IdleState(this));
    }

    private void Update()
    {
        if (DashOnCool)
        {
            dashCoolTimer += Time.deltaTime;
            if (dashCoolTimer > dashCoolDown)
            {
                dashCoolTimer = 0f;
                DashOnCool = false;
            }
        }
        Jump();
        currState.Update();

        //Temporary KeyBoard
        if (Input.GetKey(KeyCode.A))
        {
            SetMoveX(-1f);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            SetMoveX(1f);
        }
        if(!Input.GetKey(KeyCode.A)&& !Input.GetKey(KeyCode.D))
        {
            SetMoveX(0);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (jumpCount >= maxJumpCount)
                return;
            playerRb.velocity = new Vector3(0, 0, 0);
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            SetState(new JumpState(this));
            isGrounded = false;
            jumpCount++;
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Dash();
        }
    }

    private void FixedUpdate()
    {
        CheckFrontObject();
    }

    public void SetMoveX(float moveX)
    {
        this.moveX = moveX;
        if (moveX < 0)
            transform.eulerAngles = new Vector3(0, 180, 0);
        else if (moveX > 0)
            transform.eulerAngles = new Vector3(0, 0, 0);
        Debug.Log(moveX);
    }

    public void Move(float speed)
    {
        //if (!IsBlocked)
        playerRb.velocity = new Vector3(moveX * speed, playerRb.velocity.y, 0);
    }

    public void Dash()
    {
        SetState(new DashState(this));
        dashDuration = 0.1f;
    }

    public void CheckFrontObject()
    {
        IsBlocked = Physics.Raycast(transform.position,
            new Vector3(moveX, 0, 0),
            1,
            LayerMask.GetMask("Enemy"));
    }

    public void OnGround(bool isGrounded)
    {
        Debug.Log(isGrounded);
        this.isGrounded = isGrounded;
        if (isGrounded)
            jumpCount = 0;
    }

    public void Jump()
    {
        if (jumpCount >= maxJumpCount)
            return;
        foreach (var t in Input.touches)
        {
            var viewportPoint = Camera.main.ScreenToViewportPoint(t.position);

            if (viewportPoint.x > 0.5f && viewportPoint.y < 0.5f)
            {
                if (t.phase == TouchPhase.Began)
                {
                    playerRb.velocity = new Vector3(0, 0, 0);
                    playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                    SetState(new JumpState(this));
                    isGrounded = false;
                    jumpCount++;
                    Debug.Log("jump");
                }
            }
        }
    }

    public class IdleState : State
    {
        public IdleState(PlayerController controller) : base(controller) { }

        protected override void Enter()
        {
            // idle animation trigger
        }

        public override void Update()
        {
            if (!Mathf.Approximately(playerController.moveX, 0f))
            {
                playerController.SetState(new MoveState(playerController));
                return;
            }
            //else if (Attack~~~)
            //        attack~~~
        }

        public override void Exit()
        {
        }
    }

    public class MoveState : State
    {
        public MoveState(PlayerController controller) : base(controller) { }

        protected override void Enter()
        {
            // move animation trigger
        }

        public override void Update()
        {
            if (Mathf.Approximately(playerController.moveX, 0f))
            {
                playerController.SetState(new IdleState(playerController));
                return;
            }
            else
                playerController.Move(playerController.moveSpeed);
        }

        public override void Exit() { }
    }

    public class DashState : State
    {
        public DashState(PlayerController controller) : base(controller) { }

        protected override void Enter()
        {
            // dash animation trigger
            playerController.dashTimer = 0f;
        }

        public override void Update()
        {
            playerController.dashTimer += Time.deltaTime;
            if (playerController.dashTimer > playerController.dashDuration)
            {
                playerController.SetState(new IdleState(playerController));
                return;
            }
            playerController.Move(playerController.dashSpeed);
        }

        public override void Exit() { }
    }

    public class JumpState : State
    {
        public JumpState(PlayerController controller) : base(controller) { }

        protected override void Enter()
        {
            // jump/fall animation trigger
        }

        public override void Update()
        {
            if (playerController.isGrounded)
            {
                playerController.SetState(new IdleState(playerController));
                return;
            }
            playerController.Move(playerController.moveSpeed);
        }

        public override void Exit() { }
    }

    public class AttackState : State
    {
        public AttackState(PlayerController controller) : base(controller) { }

        protected override void Enter()
        {
            // attack animation trigger
        }

        public override void Update()
        {
            if (Mathf.Approximately(playerController.moveX, 0f))
            {
                playerController.SetState(new MoveState(playerController));
                return;
            }
        }

        public override void Exit() { }
    }
}
