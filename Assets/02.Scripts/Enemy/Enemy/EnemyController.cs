using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : Enemy
{

    public override EnemyState State
    {
        get { return state; }
        protected set
        {
            var prevState = state;
            state = value;

            if (prevState == state)
                return;

            switch (State)
            {
                case EnemyState.None:

                    break;
                case EnemyState.Spawn:
                    
                    break;
                case EnemyState.Motion:
                   
                    break;
                case EnemyState.Idle:
                    
                    break;
                case EnemyState.Chase:
                    
                    break;
                case EnemyState.TakeDamage:
                    
                    break;
                case EnemyState.Attack:
                    
                    break;
                case EnemyState.Skill:
                    
                    break;
                case EnemyState.Die:
                    break;
            }
        }
    }


}

//public class EnemyPatrol : MonoBehaviour
//{
//    NavMeshAgent agent;
//    public float patrolSpeed = 1.0f;

//    Vector3[] patrolPoints;
//    int currentPoint = 0;

//    void Start()
//    {
//        agent = GetComponent<NavMeshAgent>();
//        agent.speed = patrolSpeed;

//        // Get the points along the NavMesh surface
//        NavMesh navMesh = GetComponent<NavMesh>();
//        NavMeshHit hit;
//        NavMesh.SamplePosition(transform.position, out hit, 20.0f, NavMesh.AllAreas);
//        Vector3 start = hit.position;

//        NavMesh.SamplePosition(transform.position + new Vector3(10.0f, 0.0f, 0.0f), out hit, 20.0f, NavMesh.AllAreas);
//        Vector3 end = hit.position;

//        // Create an array of patrol points
//        patrolPoints = new Vector3[] { start, end };

//        // Set the destination to the first patrol point
//        agent.destination = patrolPoints[currentPoint];
//    }

//    void Update()
//    {
//        // If the agent is close enough to the patrol point, switch to the next one
//        if (agent.remainingDistance <= agent.stoppingDistance)
//        {
//            currentPoint = (currentPoint + 1) % patrolPoints.Length;
//            agent.destination = patrolPoints[currentPoint];
//        }
//    }
//}