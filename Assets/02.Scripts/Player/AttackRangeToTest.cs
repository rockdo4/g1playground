using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRangeToTest : MonoBehaviour
{
    public PlayerAttack playerAttack;
    public PlayerAttackBox attackBox;

    private void Awake()
    {
        playerAttack.OnSetWeapon = SetRange;
    }

    public void SetRange()
    {
        var box = attackBox.GetComponent<BoxCollider>();
        transform.localPosition = box.center;
        transform.localScale = box.size;
    }
}
