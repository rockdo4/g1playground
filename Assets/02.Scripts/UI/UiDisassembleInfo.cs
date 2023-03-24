using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiDisassembleInfo : MonoBehaviour
{
    public Image image;

    public void SetEmpty()
    {
        if (image != null)
            image.sprite = null;
    }

    public void Set(ReinforceSystem.Types type, string id)
    {
        var iconTable = DataTableMgr.GetTable<IconData>();
        switch (type)
        {
            case ReinforceSystem.Types.Weapon:
                {
                    var data = DataTableMgr.GetTable<WeaponData>().Get(id);
                    image.sprite = Resources.Load<Sprite>(iconTable.Get(data.iconSpriteId).iconName);
                }
                break;
            case ReinforceSystem.Types.Armor:
                {
                    var data = DataTableMgr.GetTable<ArmorData>().Get(id);
                    image.sprite = Resources.Load<Sprite>(iconTable.Get(data.iconSpriteId).iconName);
                }
                break;
            case ReinforceSystem.Types.Skill:
                {
                    var data = DataTableMgr.GetTable<SkillData>().Get(id);
                    image.sprite = Resources.Load<Sprite>(iconTable.Get(data.iconSpriteId).iconName);
                }
                break;
        }
    }
}
