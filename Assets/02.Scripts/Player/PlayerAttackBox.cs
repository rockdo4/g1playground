using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackBox : MonoBehaviour
{
    public PlayerAttack playerAttack;
    private bool isAttacking = false;

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Enemy"))
            return;
        playerAttack.StartAttack();
        if (isAttacking)
            playerAttack.AttackTarget(other.gameObject);
    }

    public void ExecuteAttack() => StartCoroutine(CoExecuteAttack());

    private IEnumerator CoExecuteAttack()
    {
        isAttacking = true;
        yield return null;
        isAttacking = false;
    }
}
