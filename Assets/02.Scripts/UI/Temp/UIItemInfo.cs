using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIItemInfo : MonoBehaviour
{
    public Image weaponImage;
    public Image armorImage;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemInfo;

    public UIInventory inventoty;

    private int incdecValue;

    public void SetEmpty()
    {
        weaponImage.sprite = null;
        armorImage.sprite = null;
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

        var itemImage = Resources.Load<Sprite>(DataTableMgr.GetTable<IconData>().Get(data.iconSpriteId).iconName);
        if (inventoty.itemType == ItemTypes.Weapon)
        {
            weaponImage.sprite = itemImage;
        }
        else if (inventoty.itemType == ItemTypes.Armor)
        {
            armorImage.sprite = itemImage;
        }
        itemName.text = DataTableMgr.GetTable<NameData>().Get(data.name).name;
        itemInfo.text = DataTableMgr.GetTable<DescData>().Get(data.desc).text;
    }
}
