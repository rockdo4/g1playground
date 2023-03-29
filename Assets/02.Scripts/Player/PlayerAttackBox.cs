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
        if (!other.CompareTag("Enemy") || !other.GetComponent<Enemy>().GetIsLive())
            return;
        playerAttack.StartAttack();
        if (isAttacking)
        {
            if (attackedList.Contains(other.gameObject))
                return;
            playerAttack.StartSlowMode();
            Vector3 pos = other.ClosestPoint(transform.position + new Vector3(0f, 0.5f, 0f));
            playerAttack.HitSound();
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
