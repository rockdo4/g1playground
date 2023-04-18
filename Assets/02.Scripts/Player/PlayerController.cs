using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
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

    private Status status;
    private PlayerInventory inventory;
    private PlayerSkills playerSkills;

    private PlayerInput input;
    private Dictionary<Type, State> states = new Dictionary<Type, State>();
    public State currState;
    private Rigidbody playerRb;
    private Animator playerAnimator;
    [NonSerialized] public float moveX;
    public float LastMoveX { get; private set; } = 1f;
    public float moveSpeed = 10f;

    public bool IsBlocked { get; private set; }

    public float jumpForce;
    public int jumpCount;
    public int maxJumpCount;
    public bool isGrounded;

    public float hitDuration = 0.5f;

    private List<GameObject> enemies;

    private GameObject target;
    public bool IsAuto { get; set; }

    private NavMeshAgent agent;
    private NavMeshPath path;
    private AgentLinkMover linkMover;
    private Vector3 prevPosition;
    private bool inDistance;
    public float defaultSpeed;

    private Coroutine cor;
    private float enemyPathLength;

    public Toggle autoToggle;

    public void SetState<T>() where T : State
    {
        if (currState != null)
            currState.Exit();
        currState = states[typeof(T)];
        currState.Enter();
    }

    private void Awake()
    {
        input = GetComponent<PlayerInput>();
        status = GetComponent<Status>();
        inventory = GetComponent<PlayerInventory>();
        playerSkills = GetComponent<PlayerSkills>();
        playerRb = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.enabled = false;
        agent.updateRotation = false;
        defaultSpeed = moveSpeed;
        agent.speed = moveSpeed;
        linkMover = GetComponent<AgentLinkMover>();
        prevPosition = agent.transform.position;
        transform.forward = new Vector3(1f, 0f, 0f);
        path = new NavMeshPath();
        autoToggle.onValueChanged.AddListener((isAuto) =>
        {
            IsAuto = isAuto;

            for (int i = 0; i < playerSkills.toggles.Length; i++)
            {
                if (playerSkills.toggles[i].interactable)
                    playerSkills.toggles[i].isOn = IsAuto;
            }
            AgentOnOff();
        });
    }

    private void Start()
    {
        states.Add(typeof(IdleState), new IdleState(this));
        states.Add(typeof(MoveState), new MoveState(this));
        states.Add(typeof(JumpState), new JumpState(this));
        states.Add(typeof(KnockBackState), new KnockBackState(this));
        states.Add(typeof(AutoMoveState), new AutoMoveState(this));
        SetState<IdleState>();
        // GetComponent<DestructedEvent>().OnDestroyEvent = GameManager.instance.Respawn;
    }

    private void Update()
    {
        currState.Update();

        if (input.LeftMove)
            SetMoveX(-1f);
        else if (input.RightMove)
            SetMoveX(1f);
        else
            SetMoveX(0f);
    }
    private void OnEnable()
    {
        RemoveAgentLinkMover();
        AddAgentLinkMover();

        testList.Clear();
    }
    private void FixedUpdate()
    {
        CheckFrontObject();
        playerAnimator.SetBool("IsGrounded", isGrounded);
    }
    public void SetSpeedReduction(float speed)
    {
        agent.speed = speed;
        moveSpeed = speed;
    }
    public void SetSpeedRateReduction(float speedRate)
    {
        agent.speed *= speedRate;
        moveSpeed *= speedRate;
    }
    public void ResetAgent()
    {
        agent.ResetPath();
        agent.velocity = Vector3.zero;
        agent.isStopped = true;
        agent.enabled = false;
        agent.enabled = true;
    }
    public void RemoveAgentLinkMover()
    {
        AgentLinkMover agentLinkMover = GetComponent<AgentLinkMover>();
        if (agentLinkMover != null)
        {
            Destroy(agentLinkMover);
        }
    }
    public void AddAgentLinkMover()
    {
        gameObject.AddComponent<AgentLinkMover>();
    }
    public bool SkillOnOff(bool isOn, int index)
    {
        return playerSkills.toggles[index].isOn = isOn;
    }
    public void AgentOn()
    {
        playerRb.isKinematic = true;
        agent.enabled = true;
        if (enemies != null)
        {
            if (cor != null)
            {
                StopCoroutine(cor);
                cor = null;
                Debug.Log("끝");
            }
            cor = StartCoroutine(SearchTarget());
        }
    }
    public void AgentOff()
    {
        playerRb.isKinematic = false;
        agent.enabled = false;
        if (cor != null)
        {
            StopCoroutine(cor);
            cor = null;
            Debug.Log("끝");
        }
    }
    public void AgentOnOff()
    {
        if (IsAuto)
        {
            if (SceneManager.GetActiveScene().name == "Scene02")
            {
                var stageController = GameObject.Find(MapManager.instance.GetCurrentMapName()).GetComponent<StageController>();
                if (stageController.unlockRequirement == StageController.UnLockRequirement.Fight)
                    enemies = stageController.GetStageEnemies();
                else if (stageController.unlockRequirement == StageController.UnLockRequirement.Puzzle ||
                    stageController.unlockRequirement == StageController.UnLockRequirement.Heal)
                {
                    autoToggle.isOn = false;
                    SetState<IdleState>();
                    return;
                }
            }
            else if (SceneManager.GetActiveScene().name != "Scene02")
                enemies = DungeonManager.instance.Enemies;

            //SetState<AutoMoveState>();
        }
        else
        {
            if (cor != null)
            {
                StopCoroutine(cor);
                cor = null;
                Debug.Log("끝");
            }
            SetState<IdleState>();
        }
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
            playerRb.velocity = new Vector3(moveX * speed, playerRb.velocity.y, 0);
        else
            playerRb.velocity = new Vector3(0f, playerRb.velocity.y, 0f);
    }

    private List<GameObject> testList = new List<GameObject>();

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Pushable"))
        {
            if (!testList.Contains(collision.gameObject))
                testList.Add(collision.gameObject);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Pushable"))
        {
            testList.Remove(collision.gameObject);
        }
    }

    public void CheckFrontObject()
    {
        var playerPosition = transform.position;
        playerPosition.y += 0.015f;
        var k = 0.935f;
        IsBlocked = false;
        for (int i = 0; i < 3; i++)
        {
            int layerMask = ~(LayerMask.GetMask("Falling") | LayerMask.GetMask("Projectile"));
            var hits = Physics.RaycastAll(playerPosition, new Vector3(moveX, 0, 0), 0.5f, layerMask);
            foreach (var hit in hits)
            {
                if ((hit.transform.CompareTag("Pushable") && !isGrounded) ||
                    hit.transform.CompareTag("Ground") || hit.transform.CompareTag("Falling"))
                {
                    IsBlocked = true;
                    return;
                }
            }

            playerPosition.y += k;
        }

        if (testList.Count > 0 && currState.GetType() == typeof(JumpState))
        {
            IsBlocked = true;
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

    private void Jump()
    {
        if (!input.Jump)
            return;
        Jump(jumpForce);
    }

    public void Jump(float force, bool forbidJumping = false)
    {
        if (jumpCount >= maxJumpCount)
            return;
        playerRb.velocity = new Vector3(moveX, 0f, 0f);
        playerRb.AddForce(Vector3.up * force, ForceMode.Impulse);
        playerAnimator.SetTrigger("Jump");
        SetState<JumpState>();
        if (jumpCount == 1 || forbidJumping)
            jumpCount = 2;
    }

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
        Debug.Log("시작");
        if (enemies == null && !isGrounded)
            yield break;

        //SetMoveX(LastMoveX);
        yield return new WaitForEndOfFrame();
        //Debug.Log("1");
        while (true)
        {
            yield return new WaitForSeconds(0.2f);
            float temp = 999999f;
            var count = 0;

            if (currState.GetType() != typeof(JumpState) && agent.isOnNavMesh)
            {
                foreach (var enemy in enemies)
                {
                    if (enemy.GetComponent<Enemy>().GetIsLive())
                    {
                        count++;
                        agent.CalculatePath(enemy.transform.position, path);
                        enemyPathLength = GetLength(path);
                        if (temp >= enemyPathLength)
                        {
                            temp = enemyPathLength;
                            target = enemy;
                        }
                    }
                }
                if (count != 0 && enemies.Count != 0&& target != null && path != null && isGrounded && target.GetComponentInChildren<OnGround>().isGround)
                {
                    agent.isStopped = false;
                    inDistance = false;
                    agent.SetDestination(target.transform.position);
                }
                var dis = Vector3.Distance(target.transform.position, transform.position);
                if (dis <= 1.5f && count != 0 && isGrounded && target.transform.position != transform.position)
                {
                    agent.isStopped = true;
                    inDistance = true;
                    SetMoveX(target.transform.position.x - transform.position.x);
                }
                //Debug.Log(enemies.Count);
                if (count == 0 && enemies.Count != 0)
                {
                    SearchPortal();
                    yield break;
                }
            }
        }
    }
    public void SearchPortal()
    {
        //문 따라가게
        var portals = MapManager.instance.GetCurrentStageObject().GetComponent<StageController>().Portals;
        foreach (var portal in portals)
        {
            if (portal.GetNextStageName().CompareTo(MapManager.instance.GetCurrentStageObject().GetComponent<StageController>().PrevStageName) != 0)
            {
                inDistance = false;
                agent.isStopped = false;
                target = portal.gameObject;
                agent.SetDestination(target.transform.position);
            }
        }
    }
    public class IdleState : State
    {
        public IdleState(PlayerController controller) : base(controller) { }

        public override void Enter()
        {
            playerController.playerRb.velocity = new Vector3(0f, playerController.playerRb.velocity.y, 0f);
        }

        public override void Update()
        {
            if (!Mathf.Approximately(playerController.moveX, 0f))
            {
                playerController.SetState<MoveState>();
                return;
            }
            playerController.playerAnimator.SetFloat("MoveX", playerController.moveX);
            playerController.Jump();
            if (playerController.IsAuto &&
                playerController.currState.GetType() != typeof(JumpState) &&
                playerController.isGrounded)
                playerController.SetState<AutoMoveState>();
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
            playerController.playerAnimator.SetFloat("MoveX", playerController.moveX);
            playerController.Move(playerController.moveSpeed);
            playerController.Jump();
        }

        public override void Exit()
        {
        }
    }

    public class JumpState : State
    {

        public JumpState(PlayerController controller) : base(controller) { }
        public float jumpTime;

        public override void Enter()
        {
            jumpTime = 0f;
        }

        public override void Update()
        {
            jumpTime += Time.deltaTime;
            if (jumpTime >= 0.3f && playerController.isGrounded)
            {
                if (!Mathf.Approximately(playerController.playerRb.velocity.x, 0f))
                    playerController.SetState<MoveState>();
                else
                    playerController.SetState<IdleState>();
                return;
            }
            playerController.Jump();
            playerController.Move(playerController.moveSpeed);
        }

        public override void Exit() { }
    }

    public class KnockBackState : State
    {
        private float hitTimer;

        public KnockBackState(PlayerController controller) : base(controller) { }

        public override void Enter()
        {
            playerController.playerAnimator.SetTrigger("Hit");
            hitTimer = 0f;
        }

        public override void Update()
        {
            hitTimer += Time.deltaTime;
            if (hitTimer > playerController.hitDuration && playerController.isGrounded)
            {
                if (playerController.IsAuto)
                    playerController.SetState<AutoMoveState>();
                else
                    playerController.SetState<IdleState>();
            }
        }

        public override void Exit() => playerController.playerAnimator.SetTrigger("EndHit");
    }
    public class AutoMoveState : State
    {
        public AutoMoveState(PlayerController controller) : base(controller) { }

        public override void Enter()
        {
            playerController.AgentOn();
        }

        public override void Update()
        {
            playerController.playerAnimator.SetFloat("MoveX", playerController.agent.velocity.x);

            while (playerController.status.CurrHp < playerController.status.FinalValue.maxHp / 2 + 1 && playerController.inventory.PotionCount[0] > 0)
            {
                playerController.inventory.UseHpPotion();
            }
            while (playerController.status.CurrMp < playerController.status.FinalValue.maxMp / 2 + 1 && playerController.inventory.PotionCount[1] > 0)
            {
                playerController.inventory.UseMpPotion();
            }

            var agentPos = (playerController.agent.transform.position - playerController.prevPosition).normalized;
            if (!playerController.inDistance && !Mathf.Approximately(agentPos.x, 0f) && playerController.currState.GetType() != typeof(KnockBackState))
            {
                playerController.SetMoveX(agentPos.x);
            }
            playerController.prevPosition = playerController.agent.transform.position;
            if (playerController.input.Jump)
            {
                playerController.AgentOff();
                playerController.Jump();
                return;
            }

            if (playerController.input.LeftMove || playerController.input.RightMove)
            {
                playerController.SetState<MoveState>();
                return;
            }
            if (Input.GetKeyDown(KeyCode.M))
            {
                var cc = playerController.GetComponent<AttackedCC>();
                cc.Reset();
            }
        }

        public override void Exit()
        {
            playerController.AgentOff();
        }
    }
}
