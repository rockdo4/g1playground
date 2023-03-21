using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSkills : MonoBehaviour
{
    public struct SkillState
    {
        public SkillAttack skill;
        public bool skillOn;
        public bool skillUsable;
        public float skillTimer;

        public void Set(SkillAttack skill)
        {
            this.skill = skill;
            skillUsable = true;
            skillTimer = 0f;
        }
    }

    private PlayerController playerController;
    public Transform skillPivot;
    public Transform playerCenter;

    public SkillAttack[] allSkillsInInspector;
    private Dictionary<string, SkillAttack> allSkillGroups = new Dictionary<string, SkillAttack>();
    public string[] PossessedSkills;
    //public List<string> PossessedSkills { get; private set; } = new List<string>();

    public Toggle[] toggles;
    public string[] defaultSkills;
    private SkillState[] skillStates;
    private int skillCount = 0;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        foreach (var skill in allSkillsInInspector)
        {
            allSkillGroups.Add(skill.Group, skill);
        }
        skillCount = toggles.Length;
        skillStates = new SkillState[skillCount];
        for (int i = 0; i < skillCount; ++i)
        {
            skillStates[i].skillTimer = 0f;
            skillStates[i].skillUsable = true;
        }

        for (int i = 0; i < skillCount; i++)
        {
            if (toggles[i] == null)
                return;
            int n = i;
            toggles[i].onValueChanged.AddListener(onOff => SkillOnOff(n, onOff));
            toggles[i].onValueChanged.AddListener(onOff => ToggleSkill(n, onOff));
            SetSkill(i, defaultSkills[i]);
        }
    }

    private void Update()
    {
        for (int i = 0; i < skillCount; ++i)
        {
            if (skillStates[i].skillOn)
            {
                if (skillStates[i].skill.isOnOffSkill)
                    skillStates[i].skill.Update();
                else
                {
                    if (skillStates[i].skillTimer < skillStates[i].skill.coolDown)
                        skillStates[i].skillTimer += Time.deltaTime;
                    else
                    {
                        skillStates[i].skillTimer = 0f;
                        skillStates[i].skillUsable = true;
                    }
                }
                UseSkill(i);
            }
        }
    }

    public void SetEmpty()
    {
        for (int i = 0; i < skillStates.Length; ++i)
        {
            skillStates[i].skill = null;
            toggles[i].image.sprite = null;
        }
    }

    public void SetSkill(int index, string id)
    {
        var len = skillStates.Length;
        var skillData = DataTableMgr.GetTable<SkillData>().Get(id);
        for (int i = 0; i < len; ++i)
        {
            if (i == index)
                continue;
            if (skillStates[i].skill != null && string.Equals(skillStates[i].skill.Group, skillData.group))
                return;
        }

        foreach (var possessedSkill in PossessedSkills)
        {
            if (string.Equals(possessedSkill, id))
            {
                toggles[index].isOn = false;
                var skill = allSkillGroups[skillData.group];
                skill.SetData(id);
                skillStates[index].Set(skill);
                toggles[index].image.sprite = Resources.Load<Sprite>(DataTableMgr.GetTable<IconData>().Get(skillData.iconSpriteId).iconName);
            }
        }
    }

    public void UseSkill(int index)
    {
        if (!skillStates[index].skillUsable)
            return;
        skillStates[index].skillUsable = false;
        var skill = skillStates[index].skill;
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
        switch (skillStates[index].skill)
        {
            case RotateAttacker:
                skillStates[index].skillUsable = true;
                ((RotateAttacker)skillStates[index].skill).Stop();
                break;
            default:
                break;
        }
    }

    public void SkillOnOff(int index, bool onOff) => skillStates[index].skillOn = onOff;
    public void ToggleSkill(int index, bool skillOn)
    {
        if (skillOn)
            UseSkill(index);
        else
            EndSkill(index);
    }

    public string GetCurrSkillID(int index)
    {
        var skill = skillStates[index].skill;
        if (skill != null)
            return skill.id;
        return null;
    }
    public List<string> GetAllSkillIds()
    {
        var list = new List<string>();
        for (int i = 0; i < skillStates.Length; ++i)
        {
            list.Add(GetCurrSkillID(i));
        }
        return list;
    }
}
