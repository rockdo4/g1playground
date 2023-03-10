using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSkills : MonoBehaviour
{
    private PlayerController playerController;
    public Transform skillPivot;
    public Transform playerCenter;
    private int skillCount = 0;
    public SkillAttack[] skillAttacks;
    private bool[] skillOn;
    private bool[] skillUsable;
    public Toggle[] toggles;
    private float[] skillTimers;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        skillCount = skillAttacks.Length;
        skillOn = new bool[skillCount];
        skillUsable = new bool[skillCount];
        skillTimers = new float[skillCount];
        for (int i = 0; i < skillCount; ++i)
        {
            skillTimers[i] = 0f;
            skillUsable[i] = true;
        }

        for (int i = 0; i < skillCount; i++)
        {
            if (toggles[i] == null)
                return;
            int n = i;
            toggles[i].onValueChanged.AddListener(onOff => SkillOnOff(n, onOff));
            toggles[i].onValueChanged.AddListener(onOff => ToggleSkill(n, onOff));
        }
    }

    private void Update()
    {
        for (int i = 0; i < skillCount; ++i)
        {
            if (skillOn[i])
            {
                if (skillAttacks[i].isOnOffSkill)
                    skillAttacks[i].Update();
                else
                {
                    if (skillTimers[i] < skillAttacks[i].CoolDown)
                        skillTimers[i] += Time.deltaTime;
                    else
                    {
                        skillTimers[i] = 0f;
                        skillUsable[i] = true;
                    }
                }
                UseSkill(i);
            }
        }
    }

    public void UseSkill(int index)
    {
        if (!skillUsable[index])
            return;
        skillUsable[index] = false;
        var skill = skillAttacks[index];
        switch (skill)
        {
            case StraightSpell:
                ((StraightSpell)skill).Fire(gameObject, skillPivot.position, new Vector3(playerController.LastMoveX, 0f, 0f));
                break;
            case BoomerangSpell:
                ((BoomerangSpell)skill).Fire(gameObject, skillPivot.position, new Vector3(playerController.LastMoveX, 0f, 0f));
                break;
            case CloseRange:
                ((CloseRange)skill).Fire(gameObject, skillPivot);
                break;
            case RotateAttacker:
                ((RotateAttacker)skill).Rotate(gameObject, playerCenter);
                break;
        }
    }

    public void EndSkill(int index)
    {
        switch (skillAttacks[index])
        {
            case RotateAttacker:
                skillUsable[index] = true;
                ((RotateAttacker)skillAttacks[index]).Stop();
                break;
            default:
                break;
        }
    }

    public void SkillOnOff(int index, bool onOff) => skillOn[index] = onOff;
    public void ToggleSkill(int index, bool skillOn)
    {
        if (skillOn)
            UseSkill(index);
        else
            EndSkill(index);

    }
}
