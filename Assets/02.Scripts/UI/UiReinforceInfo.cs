using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

public class UiReinforceInfo : MonoBehaviour
{
    public Image result;
    public Image material1;
    public Image material2;
    public Image material3;
    private Sprite powderSprite;
    private Sprite essenceSprite;

    private void Awake()
    {
        var consumeTable = DataTableMgr.GetTable<ConsumeData>().GetTable();
        var iconTable = DataTableMgr.GetTable<IconData>();
        foreach (var consumeData in consumeTable)
        {
            switch (consumeData.Value.consumeType)
            {
                case ConsumeTypes.Powder:
                    powderSprite = Resources.Load<Sprite>(iconTable.Get(consumeData.Value.iconSpriteId).iconName);
                    break;
                case ConsumeTypes.Essence:
                    essenceSprite = Resources.Load<Sprite>(iconTable.Get(consumeData.Value.iconSpriteId).iconName);
                    break;
            }
        }
        SetEmpty();
    }

    public void SetEmpty()
    {
        result.sprite = null;
        material1.sprite = null;
        material2.sprite = null;
        material3.sprite = null;
    }

    public bool Set(ReinforceSystem.Types type, string id)
    {
        if (!ReinforceSystem.CheckReinforcable(id))
        {
            // message
            SetEmpty();
            return false;
        }

        var data = ReinforceSystem.GetData(id);
        var weaponTable = DataTableMgr.GetTable<WeaponData>();
        var armorTable = DataTableMgr.GetTable<ArmorData>();
        var skillTable = DataTableMgr.GetTable<SkillData>();
        var consumeTable = DataTableMgr.GetTable<ConsumeData>();
        var iconTable = DataTableMgr.GetTable<IconData>();

        if (!ReinforceSystem.CheckMaterials(type, id))
        {
            //message
            SetEmpty();
            switch (type)
            {
                case ReinforceSystem.Types.Weapon:
                    result.sprite = Resources.Load<Sprite>(iconTable.Get(weaponTable.Get(data.result).iconSpriteId).iconName);
                    material1.sprite = Resources.Load<Sprite>(iconTable.Get(weaponTable.Get(data.material1).iconSpriteId).iconName);
                    break;
                case ReinforceSystem.Types.Armor:
                    result.sprite = Resources.Load<Sprite>(iconTable.Get(armorTable.Get(data.result).iconSpriteId).iconName);
                    material1.sprite = Resources.Load<Sprite>(iconTable.Get(armorTable.Get(data.material1).iconSpriteId).iconName);
                    break;
                case ReinforceSystem.Types.Skill:
                    result.sprite = Resources.Load<Sprite>(iconTable.Get(skillTable.Get(data.result).iconSpriteId).iconName);
                    material1.sprite = Resources.Load<Sprite>(iconTable.Get(skillTable.Get(data.material1.ToString()).iconSpriteId).iconName);
                    break;
            }
            return false;
        }

        switch (type)
        {
            case ReinforceSystem.Types.Weapon:
                result.sprite = Resources.Load<Sprite>(iconTable.Get(weaponTable.Get(data.result).iconSpriteId).iconName);
                material1.sprite = Resources.Load<Sprite>(iconTable.Get(weaponTable.Get(data.material1).iconSpriteId).iconName);
                material2.sprite = powderSprite;
                material3.sprite = essenceSprite;
                break;
            case ReinforceSystem.Types.Armor:
                result.sprite = Resources.Load<Sprite>(iconTable.Get(armorTable.Get(data.result).iconSpriteId).iconName);
                material1.sprite = Resources.Load<Sprite>(iconTable.Get(armorTable.Get(data.material1).iconSpriteId).iconName);
                material2.sprite = powderSprite;
                material3.sprite = essenceSprite;
                break;
            case ReinforceSystem.Types.Skill:
                result.sprite = Resources.Load<Sprite>(iconTable.Get(skillTable.Get(data.result).iconSpriteId).iconName);
                material1.sprite = Resources.Load<Sprite>(iconTable.Get(skillTable.Get(data.material1.ToString()).iconSpriteId).iconName);
                material2.sprite = Resources.Load<Sprite>(iconTable.Get(ReinforceSystem.GetSkillMaterial(data.material1).iconSpriteId).iconName);
                material3.sprite = powderSprite;
                break;
        }
        return true;
    }
}
