using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerController;

public class AttackedKnockBack : MonoBehaviour, IAttackable
{
    private float up = 1f;
    private float force = 7f;
    private Rigidbody rb;
    private bool hitOnThisFrame;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        hitOnThisFrame = false;
    }

    public void OnAttack(GameObject attacker, Attack attack, Vector3 attackPos)
    {
        if (!attack.IsKnockBackable || hitOnThisFrame)
            return;
        var dir = new Vector3(Mathf.Sign(transform.position.x - attackPos.x), 0f, 0f);
        dir.y = up;
        dir.Normalize();
        rb.velocity = Vector3.zero;
        rb.AddForce(dir * force, ForceMode.Impulse);
        hitOnThisFrame = true;
        if (CompareTag("Player"))
            GetComponent<PlayerController>().SetState<HitState>();
    }
}
