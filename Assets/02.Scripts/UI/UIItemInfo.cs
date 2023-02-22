using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIItemInfo : MonoBehaviour
{
    public Image characterImage;
    //public Image weaponImage;
    //public Image armorImage;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemInfo;
    //public TextMeshProUGUI incdecHp;
    //public TextMeshProUGUI incdecMp;
    //public TextMeshProUGUI incdecStr;
    //public TextMeshProUGUI incdecDex;
    //public TextMeshProUGUI incdecInt;
    //public TextMeshProUGUI incdecAtk;
    //public TextMeshProUGUI incdecDef;

    private int incdecValue;
    public void SetEmpty()
    {
        characterImage.sprite = null;
        //weaponImage.sprite = null;
        //armorImage.sprite = null;
        itemName.text = string.Empty;
        itemInfo.text = string.Empty;
        //incdecHp.text = string.Empty;
        //incdecMp.text = string.Empty;
        //incdecStr.text = string.Empty;
        //incdecDex.text = string.Empty;
        //incdecInt.text = string.Empty;
        //incdecAtk.text = string.Empty;
        //incdecDef.text = string.Empty;
    }

    public void Set(ItemData data)
    {
        if (data == null)
        {
            SetEmpty();
            return;
        }

        characterImage.sprite = data.iconSprite;
        //weaponImage.sprite = data.iconSprite;
        itemName.text = data.name;
        itemInfo.text = data.desc;
    }
}
