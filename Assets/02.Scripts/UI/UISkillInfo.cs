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

    public void SetEmpty()
    {
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

        equipImage.sprite = data.iconSprite;
        skillName.text = data.name;
        skillDesc.text = data.desc;
    }
}
