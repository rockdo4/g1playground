using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkills : MonoBehaviour
{
    private PlayerController playerController;
    public Transform skillPivot;
    private int skillCount = 0;
    public SkillAttack[] skillAttacks;
    private float[] skillTimers;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        skillCount = skillAttacks.Length;
        skillTimers = new float[skillCount];
    }

    private void Update()
    {
        for (int i = 0; i < skillCount; ++i)
        {
            if (skillTimers[i] < skillAttacks[i].CoolDown)
            {
                skillTimers[i] += Time.deltaTime;
                if (skillTimers[i] > skillAttacks[i].CoolDown)
                {
                    skillTimers[i] = 0f;
                    UseSkill(skillAttacks[i]);
                }
            }
        }
    }

    public void UseSkill(SkillAttack skill)
    {
        switch (skill)
        {
            case StraightSpell:
                ((StraightSpell)skill).Fire(gameObject, skillPivot.position, new Vector3(playerController.LastMoveX, 0f, 0f));
                break;
            case BoomerangSpell:
                ((BoomerangSpell)skill).Fire(gameObject, skillPivot.position, new Vector3(playerController.LastMoveX, 0f, 0f));
                break;
        }
    }
}
