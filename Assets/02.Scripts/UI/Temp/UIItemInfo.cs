using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class UIItemInfo : MonoBehaviour
{
    public Image equipmentImage;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemInfo;

    public UIInventory inventoty;

    public Image currWeaponImage;
    public Image currArmorImage;

    private PlayerInventory inven;

    public Button currWeaponView;
    public Button currArmorView;

    private int incdecValue;

    private void Awake()
    {
        inven = GameManager.instance.player.GetComponent<PlayerInventory>();
        CurrEquipView();
    }

    private void Update()
    {
        CurrEquipView();
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

    public void CurrWeaponSet()
    {
        WeaponData weaponData = null;        
        if (inven.CurrWeapon != null)
            weaponData = DataTableMgr.GetTable<WeaponData>().Get(inven.CurrWeapon);
        if (weaponData != null)
        {
            itemName.text = DataTableMgr.GetTable<NameData>().Get(weaponData.name).name;
            itemInfo.text = DataTableMgr.GetTable<DescData>().Get(weaponData.desc).text;
        }
           
    }

    public void CurrArmorSet()
    {
        ArmorData armorData = null;
        if (inven.CurrArmor != null)
            armorData = DataTableMgr.GetTable<ArmorData>().Get(inven.CurrArmor);
        if (armorData != null)
        {
            itemName.text = DataTableMgr.GetTable<NameData>().Get(armorData.name).name;
            itemInfo.text = DataTableMgr.GetTable<DescData>().Get(armorData.desc).text;
        }
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
