using UnityEngine;
using UnityEngine.UI;

public class UiComposeInfo : MonoBehaviour
{
    public Image result;
    public Image[] materials = new Image[2];

    public void SetEmptyAll()
    {
        for (int i = 0; i < materials.Length; ++i)
        {
            SetEmpty(i);
        }
    }

    public void SetEmpty(int index)
    {
        if (index >= materials.Length || materials[index] == null)
            return;

        materials[index].sprite = null;
        if (index == 0)
            result.sprite = null;
    }

    public void SetMaterial(int index, string id)
    {
        if (index >= materials.Length || materials[index] == null)
            return;

        var iconTable = DataTableMgr.GetTable<IconData>();
        var weapon = DataTableMgr.GetTable<WeaponData>().Get(id);
        if (weapon != null && !string.IsNullOrEmpty(weapon.iconSpriteId))
        {
            if (index == 0)
            {
                var resultItem = DataTableMgr.GetTable<WeaponData>().Get(ComposeSystem.GetData(id).resultItem);
                if (resultItem != null && !string.IsNullOrEmpty(resultItem.iconSpriteId))
                {
                    var resultIcon = iconTable.Get(resultItem.iconSpriteId);
                    if (resultIcon != null && !string.IsNullOrEmpty(resultIcon.iconName))
                        result.sprite = Resources.Load<Sprite>(resultIcon.iconName);
                }
            }
            var iconData = iconTable.Get(weapon.iconSpriteId);
            if (iconData != null && !string.IsNullOrEmpty(iconData.iconName))
            {
                materials[index].sprite = Resources.Load<Sprite>(iconData.iconName);
                return;
            }
        }

        var armor = DataTableMgr.GetTable<ArmorData>().Get(id);
        if (armor != null && !string.IsNullOrEmpty(armor.iconSpriteId))
        {
            var iconData = iconTable.Get(armor.iconSpriteId);
            if (iconData != null && !string.IsNullOrEmpty(iconData.iconName))
            {
                if (index == 0)
                {
                    var resultItem = DataTableMgr.GetTable<ArmorData>().Get(ComposeSystem.GetData(id).resultItem);
                    if (resultItem != null && !string.IsNullOrEmpty(resultItem.iconSpriteId))
                    {
                        var resultIcon = iconTable.Get(resultItem.iconSpriteId);
                        if (resultIcon != null && !string.IsNullOrEmpty(resultIcon.iconName))
                            result.sprite = Resources.Load<Sprite>(resultIcon.iconName);
                    }
                }
                materials[index].sprite = Resources.Load<Sprite>(iconData.iconName);
                return;
            }
        }
    }
}
