using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using Unity.VisualScripting;

public class UIItemInfo : MonoBehaviour
{
    public Image equipmentImage;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemInfo;

    public UIInventory inventoty;
    public BarUI skillPowBar;
    public BarUI skillDefBar;
    public BarUI attPowBar;
    public BarUI attDefBar;
    public BarUI hpBar;
    public BarUI mpBar;
    List<BarUI> barUis = new List<BarUI>();

    public Image currWeaponImage;
    public Image currArmorImage;

    private PlayerInventory inven;

    public Button currWeaponView;
    public Button currArmorView;

    private int incdecValue;

    private void Awake()
    {
        //hpBar = GetComponentInChildren<HpBar>(true);
        //mpBar = GetComponentInChildren<MpBar>(true);
        //skillPowBar = GetComponentInChildren<SkillPowerBar>(true);
        //skillDefBar = GetComponentInChildren<SkillDefBar>(true);
        //attPowBar = GetComponentInChildren<MeleePowBar>(true);
        //attDefBar = GetComponentInChildren<MeleeDefBar>(true);
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
        ShowSlider(GameManager.instance.player.GetComponent<PlayerStatus>().FinalValue);
    }

    public void Set(ItemData data)
    {
        if (data == null)
        {
            SetEmpty();
            return;
        }

        var playerStatus = GameManager.instance.player.GetComponent<PlayerStatus>();
        var playerInventory = GameManager.instance.player.GetComponent<PlayerInventory>();
        var resultValue = Status.Value.Zero;
        List<Status.Value> list = new List<Status.Value>();
        list.Add(playerStatus.DefaultValue);
        switch (data)
        {
            case WeaponData:
                list.Add(playerInventory.GetWeaponStat(data.id));
                list.Add(playerInventory.GetArmorStat(playerInventory.CurrArmor));
                break;
            case ArmorData:
                list.Add(playerInventory.GetWeaponStat(playerInventory.CurrWeapon));
                list.Add(playerInventory.GetArmorStat(data.id));
                break;
        }
        resultValue = playerStatus.AddValue(list);
        ShowSlider(resultValue);

        equipmentImage.sprite = Resources.Load<Sprite>(DataTableMgr.GetTable<IconData>().Get(data.iconSpriteId).iconName);
        itemName.text = DataTableMgr.GetTable<NameData>().Get(data.name).name;
        itemInfo.text = DataTableMgr.GetTable<DescData>().Get(data.desc).text;
    }

    private void SetBar()
    {
        if (hpBar == null)
        {
            hpBar = GetComponentInChildren<HpBar>(true);
            mpBar = GetComponentInChildren<MpBar>(true);
            skillPowBar = GetComponentInChildren<SkillPowerBar>(true);
            skillDefBar = GetComponentInChildren<SkillDefBar>(true);
            attPowBar = GetComponentInChildren<MeleePowBar>(true);
            attDefBar = GetComponentInChildren<MeleeDefBar>(true);
        }
    }

    public void ShowSlider(Status.Value value)
    {
        SetBar();
        hpBar.SetImageFillAmount(value.maxHp);
        mpBar.SetImageFillAmount(value.maxMp);
        skillPowBar.SetImageFillAmount(value.skillPower);
        skillDefBar.SetImageFillAmount(value.skillDef);
        attPowBar.SetImageFillAmount(value.meleePower);
        attDefBar.SetImageFillAmount(value.meleeDef);
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
