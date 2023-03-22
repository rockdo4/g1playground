using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class ReinforceSystem
{
    public enum Types
    {
        Skill,
        Weapon,
        Armor,
    }
    private static PlayerInventory inventory;
    private static PlayerSkills skills;
    private static string powder;
    private static string essence;
    
    public static bool Reinforce(Types type, int indexOfInventory)
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
                    id = inventory.weapons[indexOfInventory];
                break;
            case Types.Armor:
                if (indexOfInventory < 0)
                    id = inventory.CurrArmor;
                else
                    id = inventory.armors[indexOfInventory];
                break;
        }

        if (string.IsNullOrEmpty(id))
            return false;
        
        foreach (var data in table)
        {
            if (string.Equals(data.Value.material1, id))
            {
                if (!CheckMaterials(type, data.Value))
                    return false;

                switch (type)
                {
                    case Types.Skill:
                        skills.Reinforce(indexOfInventory, data.Value.result);
                        break;
                    case Types.Weapon:
                        inventory.Reinforce(ItemTypes.Weapon, indexOfInventory, data.Value.result);
                        break;
                    case Types.Armor:
                        inventory.Reinforce(ItemTypes.Armor, indexOfInventory, data.Value.result);
                        break;
                    default:
                        return false;
                }
                return true;
            }
        }
        return false;
    }

    public static bool CheckReinforcable(string materialId)
    {
        var table = DataTableMgr.GetTable<ReinforceData>().GetTable();
        foreach (var data in table)
        {
            if (string.Equals(data.Value.material1, materialId))
                return true;
        }
        return false;
    }

    public static bool CheckMaterials(Types type, ReinforceData data)
    {
        var powderCount = inventory.GetConsumableCount(powder);
        var essenceCount = inventory.GetConsumableCount(essence);
        if (powderCount < data.powder || essenceCount < data.essence)
            return false;

        switch (type)
        {
            case Types.Skill:
                {
                    var skillTable = DataTableMgr.GetTable<SkillData>().GetTable();
                    foreach (var skillData in skillTable)
                    {
                        var value = skillData.Value;
                        if (value.group == skillTable[data.material1].group && value.reinforce == 0)
                        {
                            if (skills.PossessedSkills.Contains(value.id))
                                return true;
                            break;
                        }
                    }
                    return false;
                }
            default:
                return true;
        }
    }
}
