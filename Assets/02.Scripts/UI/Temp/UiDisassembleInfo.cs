using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiDisassembleInfo : MonoBehaviour
{
    public Image image;
    public TextMeshProUGUI itemname;
    public TextMeshProUGUI itemdesc;

    public void SetEmpty()
    {
        if (image != null)
        {
            image.sprite = null;
            image.color = Color.clear;
        }
    }

    public void Set(ReinforceSystem.Types type, string id)
    {
        image.color = Color.white;
        var iconTable = DataTableMgr.GetTable<IconData>();
        switch (type)
        {
            case ReinforceSystem.Types.Weapon:
                {
                    var data = DataTableMgr.GetTable<WeaponData>().Get(id);
                    image.sprite = Resources.Load<Sprite>(iconTable.Get(data.iconSpriteId).iconName);
                    itemname.text = DataTableMgr.GetTable<NameData>().Get(data.name).name;
                    itemdesc.text = DataTableMgr.GetTable<DescData>().Get(data.desc).text;
                }
                break;
            case ReinforceSystem.Types.Armor:
                {
                    var data = DataTableMgr.GetTable<ArmorData>().Get(id);
                    image.sprite = Resources.Load<Sprite>(iconTable.Get(data.iconSpriteId).iconName);
                    itemname.text = DataTableMgr.GetTable<NameData>().Get(data.name).name;
                    itemdesc.text = DataTableMgr.GetTable<DescData>().Get(data.desc).text;
                }
                break;
            case ReinforceSystem.Types.Skill:
                {
                    var data = DataTableMgr.GetTable<SkillData>().Get(id);
                    image.sprite = Resources.Load<Sprite>(iconTable.Get(data.iconSpriteId).iconName);
                    itemname.text = DataTableMgr.GetTable<NameData>().Get(data.name).name;
                    itemdesc.text = DataTableMgr.GetTable<DescData>().Get(data.desc).text;
                }
                break;
        }
    }
}
