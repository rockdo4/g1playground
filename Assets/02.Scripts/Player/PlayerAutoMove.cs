using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static EnemyController;

public class PlayerAutoMove : MonoBehaviour
{
    public StageController stageController;
    private List<EnemyController> enemies;
    private Transform target;
    private int targetIndex;
    private float[] distance;

    public bool IsAuto { get; set; }

    private NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        targetIndex = 0;
        StartCoroutine(SearchTarget()); 
    }
    private void FixedUpdate()
    {
        if (target != null)
        {
            agent.SetDestination(target.position);
            transform.forward = new Vector3(agent.velocity.x, 0, 0);
        }
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.V))
        {
            enemies = stageController.GetStageEnemies();
            target = enemies[0].transform;
        }
    }

    IEnumerator SearchTarget()
    {
        yield return new WaitForSeconds(0.2f);

        while(target != null)
        {
            for(int i =0; i<enemies.Count; i++)
            {
                distance[i] = (transform.position - enemies[i].transform.position).magnitude;

                if (distance[targetIndex]<= distance[i+1])
                {
                    target = enemies[targetIndex].transform;
                }
                else
                {
                    targetIndex = i + i;
                    target = enemies[targetIndex].transform;
                }
            }
        }
    }
}
