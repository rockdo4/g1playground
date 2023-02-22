using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UISkillInfo : MonoBehaviour
{
    public Image equipImage;
    public Image useImage;
    public TextMeshProUGUI skillInfo;

    public void SetEmpty()
    {
        equipImage.sprite = null;
        useImage.sprite = null;
        skillInfo.text = string.Empty;
    }
}
