using Newtonsoft.Json.Schema;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSkills : MonoBehaviour
{
    public struct SkillState
    {
        public int index;
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
    public string[] defaultPossessedSkills;
    public List<string> PossessedSkills { get; private set; }

    public Toggle[] toggles;
    private SkillState[] skillStates;
    private int skillCount = 0;

    public void Reset()
    {
        for (int i = 0; i < skillStates.Length; ++i)
        {
            if (skillStates[i].skill == null)
                continue;

            skillStates[i].skillTimer = 0f;
            skillStates[i].skillUsable = true;

            if (skillStates[i].skillOn)
                UseSkill(i);
        }
    }

    public void Load(List<string> possessedSkills, int currSkill1, int currSkill2)
    {
        PossessedSkills = possessedSkills;
        SetSkill(0, currSkill1);
        SetSkill(1, currSkill2);
    }
    
    public void SetDefault()
    {
        PossessedSkills = defaultPossessedSkills.ToList();
        SetEmpty();
    }
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
            toggles[i].interactable = false;
        }
    }

    private void Update()
    {
        for (int i = 0; i < skillCount; ++i)
        {
            if (skillStates[i].skill == null)
                continue;

            if (!skillStates[i].skill.isOnOffSkill)
            {
                if (!skillStates[i].skillUsable)
                {
                    if (skillStates[i].skillTimer < skillStates[i].skill.coolDown)
                        skillStates[i].skillTimer += Time.deltaTime;
                    else
                    {
                        skillStates[i].skillTimer = 0f;
                        skillStates[i].skillUsable = true;
                    }
                }
            }

            if (skillStates[i].skillOn)
            {
                skillStates[i].skill.Update();
                UseSkill(i);
            }
        }
    }

    public void SetEmpty()
    {
        for (int i = 0; i < skillStates.Length; ++i)
        {
            SetEmpty(i);
        }
    }

    public void SetEmpty(int index)
    {
        if (index < 0 || index >= skillStates.Length)
            return;
        toggles[index].isOn = false;
        toggles[index].interactable = false;
        toggles[index].image.sprite = Resources.Load<Sprite>("Select/UI/Rectangle 153 2");
        skillStates[index].index = -1;
        skillStates[index].skill = null;

        PlayerDataManager.instance.SaveSkills();
        PlayerDataManager.instance.SaveFile();
    }

    public void SetSkill(int index, int possessedIndex)
    {
        if (possessedIndex < 0)
            SetEmpty(index);
        else
        {
            if (PossessedSkills == null)
                PossessedSkills = new List<string>();

            if (PossessedSkills.Count < 1)
                return;

            var len = skillStates.Length;
            var skillData = DataTableMgr.GetTable<SkillData>().Get(PossessedSkills[possessedIndex]);
            for (int i = 0; i < len; ++i)
            {
                if (i == index)
                    continue;
                if (skillStates[i].skill != null && string.Equals(skillStates[i].skill.Group, skillData.group))
                    return;
            }

            toggles[index].isOn = false;
            var skill = allSkillGroups[skillData.group];
            skill.SetData(PossessedSkills[possessedIndex]);
            skillStates[index].index = possessedIndex;
            skillStates[index].Set(skill);
            toggles[index].interactable = true;
            toggles[index].image.sprite = DataTableMgr.LoadIcon(skillData.iconSpriteId);
        }
        
        //toggles[index].image.sprite = Resources.Load<Sprite>(DataTableMgr.GetTable<IconData>().Get(skillData.iconSpriteId).iconName);

        PlayerDataManager.instance.SaveSkills();
        PlayerDataManager.instance.SaveFile();
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

    public int GetCurrSkillIndex(int index) => skillStates[index].index;

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

    public void AddSkill(string id)
    {
        if (PossessedSkills == null)
            PossessedSkills = new List<string>();
        PossessedSkills.Add(id);

        PlayerDataManager.instance.SaveSkills();
        PlayerDataManager.instance.SaveFile();
    }

    public void RemoveSkill(int index)
    {
        for (int i = 0; i < skillStates.Length; ++i)
        {
            if (skillStates[i].index == index)
                SetEmpty(i);
        }
        PossessedSkills.RemoveAt(index);
        for (int i = 0; i < skillStates.Length; ++i)
        {
            if (skillStates[i].index > index)
                SetSkill(i, skillStates[i].index - 1);
        }

        PlayerDataManager.instance.SaveSkills();
        PlayerDataManager.instance.SaveFile();
    }

    public void Reinforce(int index, int materialIndex, string newId)
    {
        PossessedSkills[index] = newId;
        for (int i = 0; i < skillStates.Length; ++i)
        {
            if (skillStates[i].index == index)
                SetSkill(i, index);
        }
        PlayerDataManager.instance.SaveSkills();
        PlayerDataManager.instance.SaveFile();
    }

    public void Disassemble(int index)
    {
        int powderCount = 0;
        var table = DataTableMgr.GetTable<SkilldisassembleData>().GetTable();
        string id = PossessedSkills[index];
        RemoveSkill(index);
        var skillData = DataTableMgr.GetTable<SkillData>().Get(id);
        foreach (var data in table)
        {
            if (skillData.reinforce == data.Value.skillReinforce)
            {
                powderCount = data.Value.powder;
                break;
            }
        }
        var consumeTable = DataTableMgr.GetTable<ConsumeData>().GetTable();
        string powderId = null;
        foreach (var data in consumeTable)
        {
            if (data.Value.consumeType == ConsumeTypes.Powder)
            {
                powderId = data.Value.id;
                break;
            }
        }
        GetComponent<PlayerInventory>()?.AddConsumable(powderId, powderCount);
        PlayerDataManager.instance.SaveSkills();
        PlayerDataManager.instance.SaveFile();
    }
}