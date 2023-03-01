using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkills : MonoBehaviour
{
    private PlayerController playerController;
    public SkillAttack skillAttack;
    public Transform skillPivot;
    private float skillTimer = 0f;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (skillTimer < skillAttack.CoolDown)
        {
            skillTimer += Time.deltaTime;
            if (skillTimer > skillAttack.CoolDown)
            {
                skillTimer = 0f;
                UseSkill();
            }
        }
    }

    public void UseSkill()
    {
        switch (skillAttack)
        {
            case StraightSpell:
                ((StraightSpell)skillAttack).Fire(gameObject, skillPivot.position, new Vector3(playerController.LastMoveX, 0f, 0f));
                break;
        }
    }
}
