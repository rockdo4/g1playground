using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScorpionKing : Enemy
{
    [SerializeField] private ScorpionSound scorpionSound;

    private CapsuleCollider mainColl;
    public GameObject attackBox;
    public GameObject skillPivot;


    public BasicAttack meleeAttack;
    public SkillAttack projectileSkill;
    public SkillAttack FallingAreaSkill;

    public bool isGoingRight;

    public float patrolSpeed;
    public float chaseSpeed;
    public float searchRange;
    public float attackRange;
    public float projectileTime;
    private float projectileCoolTime;
    public float areaTime;
    private float areaCoolTime;
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
        skillPivot = GameObject.Find(gameObject.name + "/SkillPivot");
        State = EnemyState.None;
    }
    protected override void Start()
    {
        player = GameManager.instance.player;
        GetComponent<DestructedEvent>().OnDestroyEvent = () =>
        {
            var clip = scorpionSound.dieClip;
            SoundManager.instance.PlayEnemyEffect(clip);
            State = EnemyState.Die;
            animator.SetTrigger("Die");
            isLive = false;

            enemyBody.SetActive(false);

        };
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        State = EnemyState.None;
        mainColl.enabled = true;
        projectileCoolTime = 0f;
        areaCoolTime = 0f;
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
            var clip = scorpionSound.groggyClip;
            SoundManager.instance.PlayEnemyEffect(clip);
            isGroggy1 = true;
            animator.SetTrigger("Groggy");
            State = EnemyState.Groggy;
            return;
        }
        else if (!isGroggy2 && status.CurrHp <= status.FinalValue.maxHp * groggy2)
        {
            var clip = scorpionSound.groggyClip;
            SoundManager.instance.PlayEnemyEffect(clip);
            isGroggy2 = true;
            animator.SetTrigger("Groggy");
            State = EnemyState.Groggy;
            return;
        }
        else if (!isGroggy3 && status.CurrHp <= status.FinalValue.maxHp * groggy3)
        {
            var clip = scorpionSound.groggyClip;
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
            areaCoolTime += Time.deltaTime;
        }
        CheackGroggy();

        switch (State)
        {
            case EnemyState.None:
                None();
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
            case EnemyState.Die:
                break;
        }

        animator.SetFloat("Move", agent.velocity.magnitude / chaseSpeed);

        if (Input.GetKeyDown(KeyCode.F))
        {
            status.CurrHp -= 1000;
        }
    }

    private void None()
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

        if (Vector3.Distance(transform.position, mySpawnPos) <= 0.5)
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

    }

    protected Vector3 startPos;
    protected Vector3 endPos;

    void BattleProcess()
    {
        if (areaCoolTime >= areaTime)
        {
            areaCoolTime = 0f;
            animator.SetTrigger("Area");
            return;
        }


        if (projectileCoolTime >= projectileTime && Vector3.Distance(transform.position, player.transform.position) >= attackRange)
        {
            var clip = scorpionSound.projectileAttackClip;
            SoundManager.instance.PlayEnemyEffect(clip);
            projectileCoolTime = 0f;
            State = EnemyState.Attack;
            animator.SetTrigger("Projectile");
            return;
        }

        if (Vector3.Distance(transform.position, player.transform.position) <= attackRange)
        {
            animator.SetTrigger("Attack");
            State = EnemyState.Attack;
            isHit = true;
            return;
        }
    }


    private void Attack()
    {
        var clip = scorpionSound.normalAttackClip;
        SoundManager.instance.PlayEnemyEffect(clip);
        attackBox.SetActive(true);
        isHit = false;
    }

    private void AttackDone()
    {
        if (!isHit)
        {
            State = EnemyState.Chase;
            attackBox.SetActive(false);
            animator.SetTrigger("AttackEnd");
        }

    }

    private void LastAttackDone()
    {
        State = EnemyState.Chase;
        attackBox.SetActive(false);
        isHit = true;
    }

    private bool isHit = false;
    private void OnTriggerEnter(Collider collider)
    {
        if (!attackBox.activeSelf)
            return;

        if (collider.gameObject.CompareTag("Player") && collider.gameObject.GetComponent<ObjectMass>() != null)
        {
#if UNITY_EDITOR
            Debug.Log(State);
#endif
            isHit = true;
            Vector3 pos = collider.ClosestPoint(transform.position + new Vector3(0f, 0.5f, 0f));
            meleeAttack.ExecuteAttack(gameObject, player.gameObject, pos);
            attackBox.SetActive(false);
        }
    }

    private void Projectile()
    {
        var playerDir = (new Vector3(player.transform.position.x, player.transform.position.y + 1f, player.transform.position.z) - skillPivot.transform.position).normalized;

        ((EnemyStraightSpell)projectileSkill).Fire(gameObject, skillPivot.transform.position, playerDir);
    }
    private void ProjectileDone()
    {
        State = EnemyState.Chase;
    }

    private void Area()
    {
        var clip = scorpionSound.areaAttackClip;
        SoundManager.instance.PlayEnemyEffect(clip);

        ((EnemyStraightSpell)FallingAreaSkill).Fire(gameObject, new Vector3(player.transform.position.x, player.transform.position.y + 8f, player.transform.position.z), Vector3.down);
    }
    private void AreaDone()
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
