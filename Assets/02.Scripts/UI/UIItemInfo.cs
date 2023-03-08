using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIItemInfo : MonoBehaviour
{
    //public Image[] itemImages;
    public Image weaponImage;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemInfo;

    //public bool isConsumable;
    private int incdecValue;
    public void SetEmpty()
    {
        //foreach (var image in itemImages)
        //{
        //    image.sprite = null;
        //}
        weaponImage.sprite = null;
        itemName.text = string.Empty;
        itemInfo.text = string.Empty;
    }

    public void Set(ItemData data)
    {
        if (data == null)
        {
            SetEmpty();
            return;
        }

        //foreach(var image in itemImages)
        //{
        //    image.sprite = data.iconSprite;
        //}
        weaponImage.sprite = data.iconSprite;
        itemName.text = data.name;
        itemInfo.text = data.desc;
    }
}
