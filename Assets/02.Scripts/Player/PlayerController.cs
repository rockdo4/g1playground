using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.GlobalIllumination;

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
    private PlayerInput input;
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
    public float dashDuration = 0.25f;
    private float dashTimer;
    public bool IsBlocked { get; private set; }

    public float jumpForce;
    public int jumpCount;
    public int maxJumpCount;
    public bool isGrounded;

    public float hitDuration = 0.5f;

    public StageController stageController;
    private List<EnemyController> enemies;
    private Transform target;
    private float[] distance;

    public bool IsAuto { get; set; }

    private NavMeshAgent agent;
    private NavMeshPath path;

    private Coroutine cor;

    private void SetState<T>() where T : State
    {
        if (currState != null)
            currState.Exit();
        currState = states[typeof(T)];
        currState.Enter();
    }

    private void Awake()
    {
        input = GetComponent<PlayerInput>();
        playerRb = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        path = new NavMeshPath();
    }

    private void Start()
    {
        states.Add(typeof(IdleState), new IdleState(this));
        states.Add(typeof(MoveState), new MoveState(this));
        states.Add(typeof(DashState), new DashState(this));
        states.Add(typeof(JumpState), new JumpState(this));
        states.Add(typeof(HitState), new HitState(this));
        states.Add(typeof(AutoMoveState), new AutoMoveState(this));
        SetState<IdleState>();
        GetComponent<DestructedEvent>().OnDestroyEvent = GameManager.instance.Respawn;
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

        if (input.LeftMove)
            SetMoveX(-1f);
        else if (input.RightMove)
            SetMoveX(1f);
        else
            SetMoveX(0f);

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            cor = StartCoroutine(SearchTarget());
            IsAuto = true;
            SetState<AutoMoveState>();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            StopCoroutine(cor);
            cor = null;
            IsAuto = false;
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            agent.CalculatePath(transform.position + Vector3.right * 3f, path);
            agent.SetDestination(transform.position + Vector3.right * 3f);
        }
    }

    private void FixedUpdate()
    {
        CheckFrontObject();
        playerAnimator.SetBool("IsGrounded", isGrounded);
    }

    private void SetMoveX(float moveX)
    {
        this.moveX = moveX;
        if (Mathf.Approximately(moveX, 0f))
        {
            //playerRb.velocity = new Vector3(0, playerRb.velocity.y, 0);
            return;
        }
        LastMoveX = moveX;
        transform.forward = new Vector3(moveX, 0, 0);
    }

    public void Move(float speed)
    {
        if (!IsBlocked)
        {
            if (currState.GetType() == typeof(DashState))
                playerRb.velocity = new Vector3(LastMoveX * speed, playerRb.velocity.y, 0);
            else
                playerRb.velocity = new Vector3(moveX * speed, playerRb.velocity.y, 0);
        }
        else
            playerRb.velocity = new Vector3(0f, playerRb.velocity.y, 0f);
    }

    public void Dash()
    {
        if (!input.Dash || DashOnCool)
            return;
        SetState<DashState>();
        dashDuration = 0.1f;
    }

    public void CheckFrontObject()
    {
        var playerPosition = transform.position;
        playerPosition.y += 0.015f;
        var k = 0.935f;
        IsBlocked = false;
        for (int i = 0; i < 3; i++)
        {
            int layerMask = ~LayerMask.GetMask("Projectile");
            var hits = Physics.RaycastAll(playerPosition, new Vector3(moveX, 0, 0), 0.5f, layerMask);
            Debug.DrawRay(playerPosition, new Vector3(moveX * 0.5f, 0, 0), Color.green);
            foreach (var hit in hits)
            {

                //if (hit.collider != null)
                //{
                //    if (!(hit.transform.CompareTag("CheckPoint") ||
                //        hit.transform.CompareTag("Falling") ||
                //        hit.transform.CompareTag("Portal") ||
                //    hit.transform.CompareTag("Stage") ||
                //        hit.transform.CompareTag("Player") ||
                //        hit.collider.CompareTag("AttackBox") ||
                //        (hit.transform.CompareTag("Pushable") && isGrounded) ||
                //        hit.transform.CompareTag("Door")))
                //    {
                //        IsBlocked = true;
                //        return;
                //    }
                //}
                if ((hit.transform.CompareTag("Pushable") && !isGrounded)||
                    hit.transform.CompareTag("Ground"))
                {
                    IsBlocked = true;
                    return;
                }

            }
            playerPosition.y += k;
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

    public void Jump() => Jump(jumpForce);

    public void Jump(float force, bool forbidJumping = false)
    {
        if (!input.Jump || jumpCount >= maxJumpCount)
            return;
        playerRb.velocity = new Vector3(moveX, 0, 0);
        playerRb.AddForce(Vector3.up * force, ForceMode.Impulse);
        playerAnimator.SetTrigger("Jump");
        SetState<JumpState>();
        if (jumpCount == 1 || forbidJumping)
            jumpCount = 2;
    }

    public void OnAttack(GameObject attacker, Attack attack, Vector3 attackPos) => SetState<HitState>();

    public float GetLength(NavMeshPath path)
    {
        float pathLength = 0;
        if (path.status == NavMeshPathStatus.PathComplete)
        {
            for (int i = 1; i < path.corners.Length; i++)
            {
                pathLength += Vector3.Distance(path.corners[i - 1], path.corners[i]);
            }
        }
        return pathLength;
    }
    IEnumerator SearchTarget()
    {
        //Debug.Log("1");
        while (true)
        {
            yield return new WaitForSeconds(0.2f);

            float temp = 999999f;
            var count = 0;
            foreach (var enemy in enemies)
            {
                if (enemy.gameObject.activeSelf && agent.CalculatePath(enemy.transform.position, path))
                {
                    count++;
                    var len = GetLength(path);
                    if (temp >= len)
                    {
                        temp = len;
                        target = enemy.transform;
                    }
                }
            }
            agent.SetDestination(target.transform.position);
            //Debug.Log("!");
            if (count == 0)
                yield break;
        }
    }

    public class IdleState : State
    {
        public IdleState(PlayerController controller) : base(controller) { }

        public override void Enter() { }

        public override void Update()
        {
            if (playerController.IsAuto)
                playerController.SetState<AutoMoveState>();
            if (!Mathf.Approximately(playerController.moveX, 0f))
            {
                playerController.SetState<MoveState>();
                return;
            }
            playerController.playerAnimator.SetFloat("MoveX", playerController.moveX);
            playerController.Dash();
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
            if (playerController.IsAuto)
                playerController.SetState<AutoMoveState>();
            if (Mathf.Approximately(playerController.moveX, 0f))
            {
                playerController.SetState<IdleState>();
                return;
            }
            playerController.playerAnimator.SetFloat("MoveX", playerController.moveX);
            playerController.Move(playerController.moveSpeed);
            playerController.Dash();
            playerController.Jump();
        }

        public override void Exit() 
        {
        }
    }

    public class DashState : State
    {
        public DashState(PlayerController controller) : base(controller) { }

        public override void Enter()
        {
            playerController.playerAnimator.SetBool("IsDashing", true);
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
            playerController.playerAnimator.SetBool("IsDashing", false);
            playerController.playerRb.constraints = ~RigidbodyConstraints.FreezePositionX & ~RigidbodyConstraints.FreezePositionY;
            playerController.playerRb.velocity = Vector3.zero;
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
            playerController.Dash();
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
            playerController.playerAnimator.SetTrigger("Hit");
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

        public override void Exit() => playerController.playerAnimator.SetTrigger("EndHit");
    }
    public class AutoMoveState : State
    {
        public AutoMoveState(PlayerController controller) : base(controller) { }

        public override void Enter() 
        {
            playerController.playerRb.isKinematic = true;
        }

        public override void Update()
        {
            playerController.playerAnimator.SetFloat("MoveX", playerController.agent.velocity.x);
            //Debug.Log( playerController.playerAnimator.GetFloat("MoveX"));
        }

        public override void Exit() 
        {
            playerController.playerRb.isKinematic = false;
        }
    }
}
