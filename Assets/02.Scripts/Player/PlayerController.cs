using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;
using static PlayerController;

public class PlayerController : MonoBehaviour, IAttackable
{
    public abstract class State
    {
        protected PlayerController playerController;
        public State(PlayerController controller)
        {
            playerController = controller;
        }
        public abstract void Enter();
        public abstract void Update();
        public abstract void Exit();
    }
    private Dictionary<Type, State> states = new Dictionary<Type, State>();
    public State currState;
    private Rigidbody playerRb;
    private Animator playerAnimator;
    public float moveX;
    public float LastMoveX { get; private set; } = 1f;
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

    public float hitDuration = 0.5f;

    private void SetState<T>() where T : State
    {
        if (currState != null)
            currState.Exit();
        currState = states[typeof(T)];
        currState.Enter();
    }

    private void Awake()
    {
        playerRb = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
    }

    private void Start()
    {
        states.Add(typeof(IdleState), new IdleState(this));
        states.Add(typeof(MoveState), new MoveState(this));
        states.Add(typeof(DashState), new DashState(this));
        states.Add(typeof(JumpState), new JumpState(this));
        states.Add(typeof(HitState), new HitState(this));
        SetState<IdleState>();
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
        currState.Update();
        playerAnimator.SetFloat("MoveX", moveX);

        //Temporary KeyBoard
        if (Input.GetKey(KeyCode.A))
        {
            SetMoveX(-1f);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            SetMoveX(1f);
        }
        else if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {
            SetMoveX(0);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (jumpCount >= maxJumpCount)
                return;
            playerRb.velocity = new Vector3(moveX, 0, 0);
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            SetState<JumpState>();
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
        if (Mathf.Approximately(moveX, 0f))
        {
            playerRb.velocity = new Vector3(0, playerRb.velocity.y, 0);
            return;
        }
        else
        {
            LastMoveX = moveX;
            transform.forward = new Vector3(moveX, 0, 0);
        }
    }

    public void Move(float speed)
    {
        if (!IsBlocked)
            playerRb.velocity = new Vector3(moveX * speed, playerRb.velocity.y, 0);
        else
            playerRb.velocity = new Vector3(0f, playerRb.velocity.y, 0f);
    }

    public void Dash()
    {
        SetState<DashState>();
        dashDuration = 0.1f;
    }

    public void CheckFrontObject()
    {
        var playerPosition = transform.position;
        playerPosition.y += 0.06f;
        var k = 0.296f;

        for (int i = 0; i < 3; i++)
        {
            RaycastHit hit;
            IsBlocked = Physics.Raycast(playerPosition, new Vector3(moveX, 0, 0), out hit, 0.9f);
            Debug.DrawRay(playerPosition, new Vector3(moveX * 0.5f, 0, 0), Color.green);
            if (hit.collider != null)
            {
                if (hit.transform.CompareTag("Player") ||
                    hit.collider.CompareTag("AttackBox") ||
                    (hit.transform.CompareTag("Pushable") && isGrounded) ||
                    hit.transform.CompareTag("Door"))
                {
                    IsBlocked = false;
                    break;
                }
            }
            playerPosition.y += k;
            if (IsBlocked)
                break;
        }
    }

    public void OnGround(bool isGrounded)
    {
        this.isGrounded = isGrounded;
        if (isGrounded)
            jumpCount = 0;
        else
        {
            jumpCount = 1;
        }
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
                if (t.phase == TouchPhase.Began&& !EventSystem.current.IsPointerOverGameObject(t.fingerId))
                {
                    playerRb.velocity = new Vector3(moveX, 0, 0);
                    playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                    SetState<JumpState>();
                    if (jumpCount == 1)
                        jumpCount = 2;
                }
            }
        }
    }

    public void OnAttack(GameObject attacker, Attack attack) => SetState<HitState>();

    public class IdleState : State
    {
        public IdleState(PlayerController controller) : base(controller) { }

        public override void Enter() { }

        public override void Update()
        {
            if (!Mathf.Approximately(playerController.moveX, 0f))
            {
                playerController.SetState<MoveState>();
                return;
            }
            playerController.Jump();
        }

        public override void Exit() { }
    }

    public class MoveState : State
    {
        public MoveState(PlayerController controller) : base(controller) { }

        public override void Enter() { }

        public override void Update()
        {
            if (Mathf.Approximately(playerController.moveX, 0f))
            {
                playerController.SetState<IdleState>();
                return;
            }
            playerController.Move(playerController.moveSpeed);
            playerController.Jump();
        }

        public override void Exit() { }
    }

    public class DashState : State
    {
        public DashState(PlayerController controller) : base(controller) { }

        public override void Enter()
        {
            playerController.dashTimer = 0f;
            playerController.playerRb.constraints = ~RigidbodyConstraints.FreezePositionX;
        }

        public override void Update()
        {
            playerController.dashTimer += Time.deltaTime;
            if (playerController.dashTimer > playerController.dashDuration)
            {
                playerController.SetState<IdleState>();
                return;
            }
            playerController.Move(playerController.dashSpeed);
            playerController.Jump();
        }

        public override void Exit()
        {
            playerController.playerRb.constraints = ~RigidbodyConstraints.FreezePositionX & ~RigidbodyConstraints.FreezePositionY;
        }
    }

    public class JumpState : State
    {
        public JumpState(PlayerController controller) : base(controller) { }

        public override void Enter() { }

        public override void Update()
        {
            if (playerController.isGrounded)
            {
                playerController.SetState<IdleState>();
                return;
            }
            playerController.Move(playerController.moveSpeed);
            playerController.Jump();
        }

        public override void Exit() { }
    }

    public class HitState : State
    {
        private float hitTimer;

        public HitState(PlayerController controller) : base(controller) { }

        public override void Enter()
        {
            playerController.playerAnimator.SetBool("Hit", true);
            hitTimer = 0f;
        }

        public override void Update()
        {
            hitTimer += Time.deltaTime;
            if (hitTimer > playerController.hitDuration)
            {
                playerController.SetState<IdleState>();
                return;
            }
        }

        public override void Exit() => playerController.playerAnimator.SetBool("Hit", false);
    }
}
