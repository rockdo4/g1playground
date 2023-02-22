using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIItemInfo : MonoBehaviour
{
    public Image equipImage;
    public Image useImage;
    public TextMeshProUGUI itemInfo;

    public void SetEmpty()
    {
        equipImage.sprite = null;
        useImage.sprite = null;
        itemInfo.text = string.Empty;
    }
}
