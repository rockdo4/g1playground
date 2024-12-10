using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnemyControllerTemp;

public class EnemyAttackBox : MonoBehaviour
{
    private EnemyControllerTemp enemyController;
    private BoxCollider boxCollider;
    private void Awake()
    {
        enemyController = GetComponentInParent<EnemyControllerTemp>();
        boxCollider = GetComponent<BoxCollider>();
    }

    private void OnTriggerStay(Collider collider)
    {
        enemyController.GetAttackBoxCollStay(collider, boxCollider);
    }

    //private void OnTriggerExit(Collider collider)
    //{
    //    if (collider.tag == "Player")
    //    {
    //        Debug.LogError("!");
    //        enemyController.GetAttackBoxCollExit(collider, boxCollider);
    //    }
    //}

}
