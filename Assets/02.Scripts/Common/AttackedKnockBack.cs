using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackedKnockBack : MonoBehaviour, IAttackable
{
    public float up;
    public float force;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void OnAttack(GameObject attacker, Attack attack)
    {
        var dir = transform.position - attacker.transform.position;
        dir.y += up * dir.magnitude;
        dir.Normalize();
        rb.velocity = Vector3.zero;
        rb.AddForce(dir * force, ForceMode.Impulse);
    }
}
