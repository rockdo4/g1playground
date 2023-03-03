using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSkills : MonoBehaviour
{
    private PlayerController playerController;
    public Transform skillPivot;
    private int skillCount = 0;
    public SkillAttack[] skillAttacks;
    private bool[] skillOn;
    public Toggle[] toggles;
    private float[] skillTimers;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        skillCount = skillAttacks.Length;
        skillOn = new bool[skillCount];
        skillTimers = new float[skillCount];
        for (int i = 0; i < skillCount; i++)
        {
            if (toggles[i] == null)
                return;
            int n = i;
            toggles[i].onValueChanged.AddListener(onOff => SkillOnOff(n, onOff));
        }
    }

    private void Update()
    {
        for (int i = 0; i < skillCount; ++i)
        {
            if (skillOn[i])
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

    public void SkillOnOff(int index, bool onOff) => skillOn[index] = onOff;
}
