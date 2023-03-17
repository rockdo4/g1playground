using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerController;

public class AttackedKnockBack : MonoBehaviour, IAttackable
{
    public float up;
    public float force;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void OnAttack(GameObject attacker, Attack attack, Vector3 attackPos)
    {
        if (!attack.IsKnockBackable)
            return;
        var dir = new Vector3(Mathf.Sign(transform.position.x - attackPos.x), 0f, 0f);
        dir.y = up;
        dir.Normalize();
        rb.velocity = Vector3.zero;
        rb.AddForce(dir * force, ForceMode.Impulse);
        if (CompareTag("Player"))
            GetComponent<PlayerController>().SetState<HitState>();
    }
}
