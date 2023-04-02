using System;
using UnityEngine;

public class ReinforceSystem
{
    public enum Types
    {
        Weapon,
        Armor,
        Skill,
    }
    private static bool isInitialized = false;
    private static PlayerInventory inventory;
    private static PlayerSkills skills;
    private static string powder;
    private static string essence;

    private static void Initialize()
    {
        if (inventory == null)
            inventory = GameManager.instance.player.GetComponent<PlayerInventory>();
        if (skills == null)
            skills = GameManager.instance.player.GetComponent<PlayerSkills>();

        if (string.IsNullOrEmpty(powder) || string.IsNullOrEmpty(essence))
        {
            var consumeTable = DataTableMgr.GetTable<ConsumeData>().GetTable();
            foreach (var consumeData in consumeTable)
            {
                switch (consumeData.Value.consumeType)
                {
                    case ConsumeTypes.Powder:
                        powder = consumeData.Value.id;
                        break;
                    case ConsumeTypes.Essence:
                        essence = consumeData.Value.id;
                        break;
                }
            }
        }
    }
    
    public static bool Reinforce(Types type, int indexOfInventory)
    {
        if (!isInitialized)
            Initialize();
        var table = DataTableMgr.GetTable<ReinforceData>().GetTable();
        string id = null;
        switch (type)
        {
            case Types.Skill:
                id = skills.PossessedSkills[indexOfInventory];
                break;
            case Types.Weapon:
                if (indexOfInventory < 0)
                    id = inventory.CurrWeapon;
                else
                    id = inventory.Weapons[indexOfInventory];
                break;
            case Types.Armor:
                if (indexOfInventory < 0)
                    id = inventory.CurrArmor;
                else
                    id = inventory.Armors[indexOfInventory];
                break;
        }

        if (string.IsNullOrEmpty(id))
            return false;

        var data = GetData(id);
        if (data == null)
            return false;

        if (!CheckMaterials(type, data))
            return false;

        var random = UnityEngine.Random.Range(0f, 1f);
        var successed = Mathf.Approximately(random, data.rate) || random < data.rate;
        
        switch (type)
        {
            case Types.Skill:
                {
                    var material = GetSkillMaterial(id);
                    int materialIndex = 0;
                    int len = skills.PossessedSkills.Count;
                    for (int i = 0; i < len; ++i)
                    {
                        if (string.Equals(skills.PossessedSkills[i], material.id))
                        {
                            materialIndex = i;
                            break;
                        }
                    }
                    skills.RemoveSkill(materialIndex);
                    if (materialIndex < indexOfInventory)
                        --indexOfInventory;
                    inventory.UseConsumable(powder, data.powder);
                    inventory.UseConsumable(essence, data.essence);
                    if (successed)
                    {
                        skills.Reinforce(indexOfInventory, materialIndex, data.result);
                        return true;
                    }
                }
                break;
            case Types.Weapon:
                inventory.UseConsumable(powder, data.powder);
                inventory.UseConsumable(essence, data.essence);
                if (successed)
                {
                    inventory.Reinforce(ItemTypes.Weapon, indexOfInventory, data.result);
                    return true;
                }
                break;
            case Types.Armor:
                inventory.UseConsumable(powder, data.powder);
                inventory.UseConsumable(essence, data.essence);
                if (successed)
                {
                    inventory.Reinforce(ItemTypes.Armor, indexOfInventory, data.result);
                    return true;
                }
                break;
            default:
                break;
        }
        return false;
    }

    public static bool CheckReinforcable(string materialId)
    {
        if (!isInitialized)
            Initialize();
        if (GetData(materialId) == null)
            return false;
        return true;
    }

    public static bool CheckMaterials(Types type, string id)
    {
        if (!isInitialized)
            Initialize();
        var data = GetData(id);
        if (data != null)
            return CheckMaterials(type, data);
        return false;
    }

    private static bool CheckMaterials(Types type, ReinforceData data)
    {
        if (!isInitialized)
            Initialize();
        var powderCount = inventory.GetConsumableCount(powder);
        var essenceCount = inventory.GetConsumableCount(essence);
        if (powderCount < data.powder || essenceCount < data.essence)
            return false;

        switch (type)
        {
            case Types.Skill:
                {
                    var skillMaterial = GetSkillMaterial(data.material1);
                    if (skillMaterial == null)
                        return false;

                    if (!string.Equals(data.material1, skillMaterial.id))
                    {
                        if (skills.PossessedSkills.Contains(skillMaterial.id))
                            return true;
                    }
                    else
                    {
                        var count = 0;
                        foreach (var skill in skills.PossessedSkills)
                        {
                            if (string.Equals(skill, skillMaterial.id))
                                ++count;

                            if (count >= 2)
                                return true;
                        }
                    }
                    return false;
                }
            default:
                return true;
        }
    }

    public static SkillData GetSkillMaterial(string id)
    {
        if (!isInitialized)
            Initialize();
        var skillTable = DataTableMgr.GetTable<SkillData>().GetTable();
        foreach (var skillData in skillTable)
        {
            var value = skillData.Value;
            if (value.group == skillTable[id].group && value.reinforce == 0)
                return value;
        }
        return null;
    }

    public static ReinforceData GetData(string toReinforceId)
    {
        if (!isInitialized)
            Initialize();
        var table = DataTableMgr.GetTable<ReinforceData>().GetTable();
        foreach (var data in table)
        {
            if (string.Equals(data.Value.material1, toReinforceId))
                return data.Value;
        }
        return null;
    }

    public static void Disassemble(Types type, int indexOfInventory)
    {
        if (!isInitialized)
            Initialize();
        switch (type)
        {
            case Types.Weapon:
                inventory.Disassemble(ItemTypes.Weapon, indexOfInventory);
                break;
            case Types.Armor:
                inventory.Disassemble(ItemTypes.Armor, indexOfInventory);
                break;
            case Types.Skill:
                skills.Disassemble(indexOfInventory);
                break;
        }
    }
}
