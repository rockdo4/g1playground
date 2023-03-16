using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerAutoMove : MonoBehaviour
{
    public StageController stageController;
    private List<GameObject> enemies;
    private Rigidbody rb;
    private Transform target;

    public bool IsAuto { get; set; }

    private NavMeshAgent agent;
    private NavMeshPath path;

    private Coroutine cor;
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = 2f;
        rb = GetComponent<Rigidbody>();
        path = new NavMeshPath();
        
        enemies = stageController.GetStageEnemies();
    }
    private void FixedUpdate()
    {
        if (target != null)
        {
            //transform.Rotate(agent.velocity.x, 0, 0);
            rb.isKinematic = true;
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            cor = StartCoroutine(SearchTarget());
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            StopCoroutine(cor);
            cor = null;
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            agent.CalculatePath(transform.position + Vector3.right * 3f, path);
            agent.SetDestination(transform.position + Vector3.right * 3f);
        }
    }
    IEnumerator SearchTarget()
    {
        Debug.Log("1");
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
            Debug.Log("!");
            if (count == 0)
                yield break;
        }
        

        //for (int i = 0; i < enemies.Count; i++)
        //{
        //    if (enemies[i].gameObject.activeSelf && agent.CalculatePath(enemies[i].transform.position, agent.path))
        //    {
        //        if (agent.path.status == NavMeshPathStatus.PathComplete)
        //        {
        //            distance[i] = agent.remainingDistance;

        //        }
        //    }
        //}
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
}
