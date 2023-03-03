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
        {
            Vector3 pos = other.ClosestPoint(transform.position);
            playerAttack.AttackTarget(other.gameObject, pos);
            var effect = GameManager.instance.effectManager.GetEffect("Sword Slash 1");
            effect.transform.position = transform.position;
            effect.transform.forward = transform.forward;
        }
    }

    public void ExecuteAttack() => StartCoroutine(CoExecuteAttack());

    private IEnumerator CoExecuteAttack()
    {
        isAttacking = true;
        yield return new WaitForFixedUpdate();
        isAttacking = false;
    }
}
