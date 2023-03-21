using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIItemInfo : MonoBehaviour
{
    public Image weaponImage;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemInfo;

    private int incdecValue;
    public void SetEmpty()
    {
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

        weaponImage.sprite = Resources.Load<Sprite>(DataTableMgr.GetTable<IconData>().Get(data.iconSpriteId).iconName);
        itemName.text = data.name;
        itemInfo.text = data.desc;
    }
}
