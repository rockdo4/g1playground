using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnemyController;

public class EnemyAttackBox : MonoBehaviour
{
    private EnemyController enemyController;
    private BoxCollider boxCollider;
    private void Awake()
    {
        enemyController = GetComponentInParent<EnemyController>();
        boxCollider = GetComponent<BoxCollider>();
    }

    private void OnTriggerStay(Collider collider)
    {
        enemyController.GetAttackBoxCollStay(collider, boxCollider);
    }

    private void OnTriggerExit(Collider collider)
    {
        enemyController.GetAttackBoxCollExit(collider, boxCollider);
    }

}
