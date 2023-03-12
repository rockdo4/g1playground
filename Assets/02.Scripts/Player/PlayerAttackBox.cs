using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackBox : MonoBehaviour
{
    public PlayerAttack playerAttack;
    private bool isAttacking = false;
    private List<GameObject> attackedList = new List<GameObject>();

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Enemy"))
            return;
        playerAttack.StartAttack();
        if (isAttacking)
        {
            if (attackedList.Contains(other.gameObject))
                return;
            Vector3 pos = other.ClosestPoint(transform.position);
            playerAttack.AttackTarget(other.gameObject, pos);
            attackedList.Add(other.gameObject);
        }
    }

    public void ExecuteAttack()
    {
        attackedList.Clear();
        StartCoroutine(CoAttackExecution());
    }

    private IEnumerator CoAttackExecution()
    {
        isAttacking = true;
        yield return new WaitForFixedUpdate();
        isAttacking = false;
    }
}
