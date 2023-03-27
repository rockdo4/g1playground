using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardPanel : MonoBehaviour
{
    public Image getImage;
    public Image[] getImages;

    public void OpenRewardPopUp(string id)
    {
        var weapon = DataTableMgr.GetTable<WeaponData>().GetTable();
        var armor = DataTableMgr.GetTable<ArmorData>().GetTable();
        var icon = DataTableMgr.GetTable<IconData>().GetTable();
        var skill = DataTableMgr.GetTable<SkillData>().GetTable();  

        if (int.Parse(id[0].ToString()) == 2)
        {
            getImage.sprite = Resources.Load<Sprite>(icon[weapon[id].iconSpriteId].iconName);
        }
        else if (int.Parse(id[0].ToString()) == 3)
        {
            getImage.sprite = Resources.Load<Sprite>(icon[armor[id].iconSpriteId].iconName);
        }
        else if (int.Parse(id[0].ToString()) == 1)
        {
            getImage.sprite = Resources.Load<Sprite>(icon[skill[id].iconSpriteId].iconName);
        }
    }

    public void OpenTenRewardPopUp(string[] ids)
    {
        var weapon = DataTableMgr.GetTable<WeaponData>().GetTable();
        var armor = DataTableMgr.GetTable<ArmorData>().GetTable();
        var icon = DataTableMgr.GetTable<IconData>().GetTable();
        var skill = DataTableMgr.GetTable<SkillData>().GetTable();

        for (int i = 0; i < 10;i++)
        {
            if (int.Parse(ids[i][0].ToString()) == 2)
            {
                getImages[i].sprite = Resources.Load<Sprite>(icon[weapon[ids[i]].iconSpriteId].iconName);
            }
            else if (int.Parse(ids[i][0].ToString()) == 3)
            {
                getImages[i].sprite = Resources.Load<Sprite>(icon[armor[ids[i]].iconSpriteId].iconName);
            }
            else if (int.Parse(ids[i][0].ToString()) == 1)
            {
                getImages[i].sprite = Resources.Load<Sprite>(icon[skill[ids[i]].iconSpriteId].iconName);
            }
        }
    }

    void Update()
    {

    }
}
