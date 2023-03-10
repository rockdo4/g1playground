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
        StopCoroutine(CoEndAttackExecution());
        isAttacking = true;
        var effect = GameManager.instance.effectManager.GetEffect("Sword Slash 1");
        var effectPos = transform.position;
        effect.transform.position = new Vector3(effectPos.x, effectPos.y + 1f, effectPos.z);
        effect.transform.forward = transform.forward;
    }

    public void EndAttackExecution() => StartCoroutine(CoEndAttackExecution());

    private IEnumerator CoEndAttackExecution()
    {
        yield return new WaitForFixedUpdate();
        isAttacking = false;
    }
}
