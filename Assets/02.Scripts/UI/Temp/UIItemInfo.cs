using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIItemInfo : MonoBehaviour
{
    public Image equipmentImage;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemInfo;

    public UIInventory inventoty;

    public Image currWeaponImage;
    public Image currArmorImage;

    private PlayerInventory inven;

    private int incdecValue;

    private void Start()
    {
        inven = GameManager.instance.player.GetComponent<PlayerInventory>();
    }

    public void SetEmpty()
    {
        equipmentImage.sprite = null;
        itemName.text = string.Empty;
        itemInfo.text = string.Empty;
        currWeaponImage.sprite = null;
        currArmorImage.sprite = null;
    }

    public void Set(ItemData data)
    {
        if (data == null)
        {
            SetEmpty();
            return;
        }

        equipmentImage.sprite = Resources.Load<Sprite>(DataTableMgr.GetTable<IconData>().Get(data.iconSpriteId).iconName);
        itemName.text = DataTableMgr.GetTable<NameData>().Get(data.name).name;
        itemInfo.text = DataTableMgr.GetTable<DescData>().Get(data.desc).text;
    }

    public void CurrEquipView()
    {
        WeaponData weaponData = null;
        ArmorData armorData = null;
        if (inven.CurrWeapon != null)
            weaponData = DataTableMgr.GetTable<WeaponData>().Get(inven.CurrWeapon);
        if (inven.CurrArmor != null)
            armorData = DataTableMgr.GetTable<ArmorData>().Get(inven.CurrArmor);

        if(weaponData != null)
        {
            currWeaponImage.sprite = Resources.Load<Sprite>(DataTableMgr.GetTable<IconData>().Get(weaponData.iconSpriteId).iconName);
        }
        if(armorData != null)
        {
            currArmorImage.sprite = Resources.Load<Sprite>(DataTableMgr.GetTable<IconData>().Get(armorData.iconSpriteId).iconName);
        }
    }
}
