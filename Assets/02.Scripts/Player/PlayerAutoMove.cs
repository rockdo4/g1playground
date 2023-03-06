using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerAutoMove : MonoBehaviour
{
    private List<EnemyController> enemies;
    private Transform target;
    private int targetIndex;
    private float[] distance;

    private NavMeshAgent agent;

    private void Start()
    {
        targetIndex = 0;
        StartCoroutine(SearchTarget()); 
    }
    private void Update()
    {
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
                    target = enemies[i].transform;
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
