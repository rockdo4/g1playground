using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Animator playerAnimator;
    public BasicAttack basicAttack;
    public PlayerAttackBox attackBox;

    private void Awake()
    {
        playerAnimator = GetComponent<Animator>();
    }

    public void StartAttack() => playerAnimator.SetBool("IsAttacking", true);
    public void ExecuteAttack() => attackBox.ExecuteAttack();
    public void AttackTarget(GameObject target, Vector3 attackPos) => basicAttack.ExecuteAttack(gameObject, target, attackPos); 
}
