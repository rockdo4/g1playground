using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour
{
    protected EnemyState state;
    protected Status status;

    protected EnemySoundPlayer enemySound;
    //protected EnemyBody enemyBody;
    protected GameObject player;
    protected Vector3 mySpawnPos;
    protected Quaternion mySpawnDir;
    protected bool isLive = true;

    public enum EnemyState
    {
        None,
        Hide,
        Spawn,
        Motion,
        Idle,
        Patrol,
        Return,
        Chase,
        Attack,
        Skill,
        TakeDamage,
        KnockBack,
        Stun,
        Groggy,
        Die,
    }

    protected Animator animator;
    protected Rigidbody rb;
    protected NavMeshAgent agent;
    protected GameObject enemyBody;
    //protected CapsuleCollider collider;

    public virtual EnemyState State
    {
        get;
        protected set;
    }

    public void SetStartPos(Vector3 pos)
    {
        mySpawnPos = pos;
    }
    protected virtual void Awake()
    {
        enemySound = GetComponent<EnemySoundPlayer>();
        status = GetComponent<Status>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        enemyBody = GameObject.Find(gameObject.name + "/EnemyBody");
        //collider = GetComponent<CapsuleCollider>();
        mySpawnPos = transform.position;
        mySpawnDir = transform.rotation;
    }
   
    protected virtual void Start()
    {
        player = GameManager.instance.player;
        GetComponent<DestructedEvent>().OnDestroyEvent = () =>
        {
            //Play Die Sound
            enemySound.PlayDeathSound();
            State = EnemyState.Die;
            animator.ResetTrigger("TakeDamage");
            animator.SetTrigger("Die");
            isLive = false;

            enemyBody.SetActive(false);

            player.GetComponent<PlayerLevelManager>().CurrExp += DataTableMgr.GetTable<MonsterData>().Get(status.id).exp;
        };
    }
    protected virtual void OnEnable()
    {
        transform.position = mySpawnPos;
        transform.rotation = mySpawnDir;
        enemyBody.SetActive(true);
        isLive = true;
    }

    public virtual void KnockBack()
    {
    }
    public virtual void Stun(float stunCool)
    {
    }

    protected Vector3 GetSpawnPos() { return mySpawnPos; }


    protected bool LookAtTarget()
    {
        Vector3 dir = player.transform.position - transform.position;
        dir.y = 0;

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 10f);

        if (dir.x == 0f)
            return true;

        if (Quaternion.Angle(transform.rotation, Quaternion.LookRotation(dir)) < 1f)
            return true;

        return true;
        //// 방향 벡터의 크기가 0이 아닌지 확인
        //if (dir.sqrMagnitude > 0.0f)
        //{
        //    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 10f);

        //    if (Quaternion.Angle(transform.rotation, Quaternion.LookRotation(dir)) < 1f)
        //        return true;
        //}
        //else if (dir.sqrMagnitude < 0.0f)
        //{
        //    // 방향 벡터의 크기가 0인 경우, 예외 처리를 추가합니다.
        //    // 여기서는 false를 반환하거나 필요한 처리를 수행할 수 있습니다.
        //    return false;
        //}

        //return false;

        //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 10f);

        //if (dir.x == 0f)
        //    return true;

        //if (Quaternion.Angle(transform.rotation, Quaternion.LookRotation(dir)) < 1f)
        //    return true;

        //return false;
    }
    protected bool LookAtPos(Vector3 targetPos)
    {
        Vector3 dir = targetPos - transform.position;
        dir.y = 0;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 10f);

        if (dir.x == 0f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.left), Time.deltaTime * 10f);
            return true;
        }

        if (Quaternion.Angle(transform.rotation, Quaternion.LookRotation(dir)) < 1f)
            return true;

        return false;
    }

    public bool GetIsLive()
    {
        return isLive;
    }

    protected void LookAtFront()
    {
        var front = Quaternion.Euler(0, 180, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, front, Time.deltaTime * 10f);
    }
    protected bool RayShooter(float range, bool isGoingRight)
    {
        Vector3 rayOrigin;
        Ray ray;
        rayOrigin = transform.position + new Vector3(0, 0.5f, 0);

        if (isGoingRight)
        {
            ray = new Ray(rayOrigin, Vector3.right);
        }
        else
        {
            ray = new Ray(rayOrigin, Vector3.left);
        }
#if UNITY_EDITOR
        Debug.DrawRay(ray.origin, ray.direction * range, Color.red);
#endif
        if (Physics.Raycast(ray, out RaycastHit hit, range, LayerMask.GetMask("Player")))
        {
            //if (hit.collider.tag == "Player")
            if (State != EnemyState.Chase)
            {
                State = EnemyState.Chase;
                return true;
            }

            if (State == EnemyState.Chase)
            {
                return true;
            }
        }
        return false;
    }
    protected void SaveFloorLength(ref Vector3 startPos, ref Vector3 endPos)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -Vector3.up, out hit))
        {
            Collider collider = hit.collider;
            //floorLength = collider.bounds.size.x;
            startPos = collider.bounds.center + new Vector3(0.5f - collider.bounds.extents.x, collider.bounds.extents.y, 0);
            endPos = collider.bounds.center + new Vector3(-0.5f + collider.bounds.extents.x, collider.bounds.extents.y, 0);
        }
        else
        {
#if UNITY_EDITOR
            Debug.Log("Arrangement Fail");
#endif
        }
    }


    protected bool AngleIgnoringHeight(float angle)
    {
        Vector3 playerDirection = new Vector3(player.transform.position.x, 0, player.transform.position.z) -
                              new Vector3(transform.position.x, 0, transform.position.z);

        if (playerDirection.sqrMagnitude > 0.0f)
        {
            return Quaternion.Angle(
                transform.rotation,
                Quaternion.LookRotation(playerDirection.normalized))
                <= angle;
        }
        else
        {

            return true;
        }
    }

    protected float SetStunTime(float stunCool, int count)
    {
        if (count == 0)
        {
            return stunCool * 1f;
        }
        else if (count == 1)
        {
            return stunCool * 0.7f;
        }
        else if (count == 2)
        {
            return stunCool * 0.5f;
        }
        else
        {
            return 0f;
        }
    }

    public Status GetStatus() { return status; }
}