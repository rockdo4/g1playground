using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UISkillInfo : MonoBehaviour
{
    public Image equipImage;
    public Image useImage;
    public Image firstEquipIcon;
    public Image secondEquipIcon;
    public TextMeshProUGUI skillName;
    public TextMeshProUGUI skillDesc;
    private SkillData currSkillData;

    public void SetEmpty()
    {
        currSkillData = null;
        equipImage.sprite = null;
        useImage.sprite = null;
        firstEquipIcon.sprite = null;
        secondEquipIcon.sprite = null;
        skillName.text = string.Empty;
        skillDesc.text = string.Empty;
    }

    public void Set(SkillData data)
    {
        if (data == null)
        {
            SetEmpty();
            return;
        }
        currSkillData = data;
        equipImage.sprite = Resources.Load<Sprite>(DataTableMgr.GetTable<IconData>().Get(data.iconSpriteId).iconName);
        skillName.text = data.name;
        skillDesc.text = data.desc;
    }

    public void SetPlayerSkill(int index)
    {
        if (currSkillData != null)
        {
            GameManager.instance.player.GetComponent<PlayerSkills>().SetSkill(index, currSkillData.id);
            ShowCurrPlayerSkills();
        }
    }

    public void ShowCurrPlayerSkills()
    {
        var playerSkills = GameManager.instance.player.GetComponent<PlayerSkills>();
        var skillTable = DataTableMgr.GetTable<SkillData>();
        firstEquipIcon.sprite = Resources.Load<Sprite>(DataTableMgr.GetTable<IconData>().Get(skillTable.Get(playerSkills.GetCurrSkillID(0)).iconSpriteId).iconName);
        secondEquipIcon.sprite = Resources.Load<Sprite>(DataTableMgr.GetTable<IconData>().Get(skillTable.Get(playerSkills.GetCurrSkillID(1)).iconSpriteId).iconName);
    }
}
