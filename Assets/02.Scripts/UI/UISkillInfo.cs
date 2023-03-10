using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UISkillInfo : MonoBehaviour
{
    public Image equipImage;
    public Image useImage;
    public TextMeshProUGUI skillName;
    public TextMeshProUGUI skillDesc;
    private SkillData currSkillData;

    public void SetEmpty()
    {
        currSkillData = null;
        equipImage.sprite = null;
        useImage.sprite = null;
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
        equipImage.sprite = data.iconSprite;
        skillName.text = data.name;
        skillDesc.text = data.desc;
    }

    public void SetPlayerSkill(int index)
    {
        if (currSkillData != null)
            GameManager.instance.player.GetComponent<PlayerSkills>().SetSkill(index, currSkillData.id);
    }
}
