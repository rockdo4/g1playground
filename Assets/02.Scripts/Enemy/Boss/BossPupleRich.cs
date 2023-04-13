using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.Android;

public class BossPupleRich : Enemy
{
    [SerializeField] private PurpleRichSound purpleRichSound;

    private CapsuleCollider mainColl;
    public GameObject attackBox;
    public GameObject skillPivot;
    public GameObject indicatorBox;

    public BasicAttack meleeAttack;
    public SkillAttack projectileSkill;

    public bool isGoingRight;

    public float patrolSpeed;
    public float chaseSpeed;
    public float searchRange;
    public float attackRange;
    public float projectileTime;
    private float projectileCoolTime;
    public float spawnTime;
    private float spawnCoolTime;
    public float returnTime;
    private float returnCoolTime;

    public override EnemyState State
    {
        get { return state; }
        protected set
        {
            var prevState = state;
            state = value;
            if (EnemyState.Die == prevState && EnemyState.None != value)
                return;

            if (prevState == state)
                return;

            switch (State)
            {
                case EnemyState.None:
                    rb.isKinematic = true;
                    agent.enabled = true;
                    break;
                case EnemyState.Patrol:
                    agent.enabled = true;
                    agent.isStopped = false;
                    agent.speed = patrolSpeed;
                    rb.isKinematic = true;
                    break;
                case EnemyState.Return:
                    agent.velocity = Vector3.zero;
                    agent.speed = chaseSpeed;
                    rb.isKinematic = true;
                    break;
                case EnemyState.Chase:
                    agent.enabled = true;
                    agent.isStopped = false;
                    agent.speed = chaseSpeed;
                    rb.isKinematic = true;
                    break;
                case EnemyState.Groggy:
                    agent.enabled = true;
                    agent.velocity = Vector3.zero;
                    agent.isStopped = true;
                    rb.isKinematic = true;
                    break;
                case EnemyState.Attack:
                    agent.enabled = true;
                    agent.velocity = Vector3.zero;
                    agent.isStopped = true;
                    rb.isKinematic = false;
                    indicatorBox.SetActive(false);
                    break;
                case EnemyState.Skill:
                    agent.isStopped = true;
                    agent.velocity = Vector3.zero;
                    rb.isKinematic = false;
                    agent.enabled = false;
                    break;
                case EnemyState.Die:
                    agent.velocity = Vector3.zero;
                    agent.enabled = true;
                    rb.isKinematic = true;
                    mainColl.enabled = false;
                    indicatorBox.SetActive(false);
                    break;
            }
        }
    }

    public bool preGoingRight;
    protected override void Awake()
    {
        base.Awake();
        mainColl = GetComponent<CapsuleCollider>();
        attackBox = GameObject.Find(gameObject.name + "/AttackBox");
        attackBox.SetActive(false);
        skillPivot = GameObject.Find(gameObject.name + "/AttackPivot");
        indicatorBox = GameObject.Find(gameObject.name + "/IndicatorBox");
        indicatorBox.SetActive(false);
        State = EnemyState.None;
    }
    protected override void Start()
    {
        player = GameManager.instance.player;
        GetComponent<DestructedEvent>().OnDestroyEvent = () =>
        {
            var clip = purpleRichSound.dieClip;
            SoundManager.instance.PlayEnemyEffect(clip);
            State = EnemyState.Die;
            animator.SetTrigger("Die");
            isLive = false;

            enemyBody.SetActive(false);
            player.GetComponent<PlayerLevelManager>().CurrExp += DataTableMgr.GetTable<MonsterData>().Get(status.id).exp;
        };
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        State = EnemyState.None;
        mainColl.enabled = true;
        projectileCoolTime = 0f;
        spawnCoolTime = 0f;
        returnCoolTime = 0f;
        isGroggy1 = false;
        isGroggy2 = false;
        isGroggy3 = false;
    }

    public float groggy1;
    public float groggy2;
    public float groggy3;
    private bool isGroggy1;
    private bool isGroggy2;
    private bool isGroggy3;
    private void CheackGroggy()
    {
        if (isGroggy1 && isGroggy2 && isGroggy3)
            return;

        if (!isGroggy1 && status.CurrHp <= status.FinalValue.maxHp * groggy1)
        {
            var clip = purpleRichSound.groggyClip;
            SoundManager.instance.PlayEnemyEffect(clip);
            isGroggy1 = true;
            animator.SetTrigger("Groggy");
            State = EnemyState.Groggy;
            return;
        }
        else if (!isGroggy2 && status.CurrHp <= status.FinalValue.maxHp * groggy2)
        {
            var clip = purpleRichSound.groggyClip;
            SoundManager.instance.PlayEnemyEffect(clip);
            isGroggy2 = true;
            animator.SetTrigger("Groggy");
            State = EnemyState.Groggy;
            return;
        }
        else if (!isGroggy3 && status.CurrHp <= status.FinalValue.maxHp * groggy3)
        {
            var clip = purpleRichSound.groggyClip;
            SoundManager.instance.PlayEnemyEffect(clip);
            isGroggy3 = true;
            animator.SetTrigger("Groggy");
            State = EnemyState.Groggy;
            return;
        }
    }
    protected void Update()
    {
        //attackTime += Time.deltaTime;

        if (state != EnemyState.None)
        {
            projectileCoolTime += Time.deltaTime;
            spawnCoolTime += Time.deltaTime;
        }
        CheackGroggy();

        switch (State)
        {
            case EnemyState.None:
                NoneUpdate();
                break;
            case EnemyState.Patrol:
                PatrolUpdate();
                break;
            case EnemyState.Return:
                ReturnUpdate();
                break;
            case EnemyState.Chase:
                ChaseUpdate();
                break;
            case EnemyState.Attack:
                AttackUpdate();
                break;
            case EnemyState.Skill:
                SkillUpdate();
                break;
            case EnemyState.Die:
                break;
        }

        animator.SetFloat("Move", agent.velocity.magnitude / chaseSpeed);
        //Debug.Log(State);

        if (Input.GetKeyDown(KeyCode.F))
        {
            status.CurrHp -= 1000;
        }
    }

    private void NoneUpdate()
    {
        State = EnemyState.Patrol;
        SaveFloorLength(ref startPos, ref endPos);
    }

    protected void PatrolUpdate()
    {
        returnCoolTime += Time.deltaTime;
        if (returnTime < returnCoolTime)
        {
            State = EnemyState.Return;
            returnCoolTime = 0f;
        }

        RayShooter(searchRange, isGoingRight);

        if (isGoingRight)
        {
            if (Vector3.Distance(transform.position, endPos) < 0.1f)
            {
                agent.velocity = Vector3.zero;

                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.left), Time.deltaTime * 10f);
                if (Quaternion.Angle(transform.rotation, Quaternion.LookRotation(startPos - transform.position).normalized) <= 5f)
                    isGoingRight = false;

                return;
            }
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(endPos - transform.position).normalized, Time.deltaTime * 10f);
            agent.SetDestination(endPos);
        }
        else
        {
            if (Vector3.Distance(transform.position, startPos) < 0.1f)
            {
                agent.velocity = Vector3.zero;

                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.right).normalized, Time.deltaTime * 10f);
                if (Quaternion.Angle(transform.rotation, Quaternion.LookRotation(endPos - transform.position).normalized) <= 5f)
                    isGoingRight = true;

                return;
            }
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(startPos - transform.position).normalized, Time.deltaTime * 10f);
            agent.SetDestination(startPos);
        }

    }
    private void ReturnUpdate()
    {
        if (LookAtPos(mySpawnPos))
            agent.SetDestination(mySpawnPos);

        if (Vector3.Distance(transform.position, mySpawnPos) <= 1.5f)
        {
            State = EnemyState.None;
            returnCoolTime = 0f;
        }
    }

    protected void ChaseUpdate()
    {
        if (LookAtTarget())
            agent.SetDestination(player.transform.position);

        BattleProcess();
    }

    protected void AttackUpdate()
    {
        LookAtTarget();
    }
    protected void SkillUpdate()
    {
        LookAtTarget();
    }

    protected Vector3 startPos;
    protected Vector3 endPos;

    private int attackCount;
    void BattleProcess()
    {
        if (spawnCoolTime >= spawnTime)
        {
            spawnCoolTime = 0;
            State = EnemyState.Skill;
            animator.SetTrigger("Spawn");
        }

        if (projectileCount == 2)
        {
            projectileCount = 0;
            State = EnemyState.Skill;
            animator.SetTrigger("Area");
            indicatorBox.SetActive(true);
            indicatorBox.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 1f, player.transform.position.z);
            return;
        }


        if (projectileCoolTime >= projectileTime && Vector3.Distance(transform.position, player.transform.position) >= attackRange)
        {
            var clip = purpleRichSound.projectileAttackClip;
            SoundManager.instance.PlayEnemyEffect(clip);
            projectileCoolTime = 0f;
            State = EnemyState.Skill;
            animator.SetTrigger("Projectile");
            return;
        }

        if (Vector3.Distance(transform.position, player.transform.position) <= attackRange)
        {
            animator.SetTrigger("Attack");
            State = EnemyState.Attack;
            return;
        }
    }

    private void Attack()
    {
        var clip = purpleRichSound.normalAttackClip;
        SoundManager.instance.PlayEnemyEffect(clip);
        attackBox.SetActive(true);
        ++attackCount;
    }

    private void AttackDone()
    {
        attackBox.SetActive(false);

        if (attackCount >= 3)
            State = EnemyState.Chase;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (!attackBox.activeSelf)
            return;

        if (collider.gameObject.CompareTag("Player") && collider.gameObject.GetComponent<ObjectMass>() != null)
        {
#if UNITY_EDITOR
            Debug.Log(State);
#endif
            Vector3 pos = collider.ClosestPoint(transform.position + new Vector3(0f, 0.5f, 0f));
            meleeAttack.ExecuteAttack(gameObject, player.gameObject, pos);
            attackBox.SetActive(false);
        }
    }

    public float summonDistance = 2f;
    private void SpawnEnemy()
    {
        var clip = purpleRichSound.spawnEnemyClip;
        SoundManager.instance.PlayEnemyEffect(clip);

        Vector3 leftSummonPosition = new Vector3(transform.position.x - summonDistance, transform.position.y, transform.position.z);
        Vector3 rightSummonPosition = new Vector3(transform.position.x + summonDistance, transform.position.y, transform.position.z);

        GameObject leftEnemy = GameManager.instance.enemyManager.GetPooledEnemy(0);
        if (leftEnemy != null)
        {
            var agent = leftEnemy.GetOrAddComponent<NavMeshAgent>();
            agent.enabled = false;
            leftEnemy.GetComponent<Enemy>().SetStartPos(leftSummonPosition);

            leftEnemy.SetActive(true);
            agent.enabled = true;
        }

        GameObject rightEnemy = GameManager.instance.enemyManager.GetPooledEnemy(0);
        if (rightEnemy != null)
        {
            agent = rightEnemy.GetOrAddComponent<NavMeshAgent>();
            agent.enabled = false;
            rightEnemy.GetComponent<Enemy>().SetStartPos(rightSummonPosition);

            rightEnemy.SetActive(true);
            agent.enabled = true;
        }
    }

    private void Projectile()
    {
        ((EnemyStraightSpell)projectileSkill).Fire(gameObject, skillPivot.transform.position, GetShotDir());
    }
    private int projectileCount = 0;


    private void ProjectileDone()
    {
        State = EnemyState.Chase;
        ++projectileCount;
    }

    private void Area()
    {
        var clip = purpleRichSound.areaAttackClip;
        SoundManager.instance.PlayEnemyEffect(clip);
        indicatorBox.SetActive(false);
        ((EnemyStraightSpell)projectileSkill).Fire(gameObject, indicatorBox.transform.position, Vector3.down);
    }
    private void AreaDone()
    {
        State = EnemyState.Chase;
    }
    private void Spawn()
    {
        SpawnEnemy();
    }
    private void SpawnDone()
    {
        State = EnemyState.Chase;
    }
    private void GroggyDone()
    {
        State = EnemyState.Chase;
    }
    private void DieDone()
    {
        gameObject.SetActive(false);
    }
}
