using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using Unity.VisualScripting;

public class UIItemInfo : MonoBehaviour
{
    public ItemData Data { get; private set; }
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

    //public Image currWeaponImage;
    //public Image currArmorImage;

    private PlayerInventory inven;

    //public Button currWeaponView;
    //public Button currArmorView;

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
        //CurrEquipView();
    }

    private void Update()
    {
        //CurrEquipView();
    }

    public void SetEmpty()
    {
        Data = null;
        equipmentImage.sprite = null;
        equipmentImage.color = Color.clear;
        itemName.text = string.Empty;
        itemInfo.text = string.Empty;
        //currWeaponImage.sprite = null;
        //currArmorImage.sprite = null;
        var playerStatus = GameManager.instance.player.GetComponent<PlayerStatus>();
        ShowSlider(playerStatus.DefaultValue + playerStatus.EquipValue);
    }

    public void Set(ItemData data)
    {
        Data = data;
        if (data == null)
        {
            SetEmpty();
            return;
        }

        var playerStatus = GameManager.instance.player.GetComponent<PlayerStatus>();
        var playerInventory = GameManager.instance.player.GetComponent<PlayerInventory>();
        var resultValue = Status.Value.Zero;
        List<Status.Value> list = new List<Status.Value>();
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
        resultValue = playerStatus.DefaultValue + playerStatus.AddValue(list);
        ShowSlider(resultValue);

        equipmentImage.sprite = Resources.Load<Sprite>(DataTableMgr.GetTable<IconData>().Get(data.iconSpriteId).iconName);
        equipmentImage.color = Color.white;
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

    //public void CurrWeaponSet()
    //{
    //    WeaponData weaponData = null;        
    //    if (inven.CurrWeapon != null)
    //        weaponData = DataTableMgr.GetTable<WeaponData>().Get(inven.CurrWeapon);
    //    if (weaponData != null)
    //    {
    //        equipmentImage.sprite = Resources.Load<Sprite>(DataTableMgr.GetTable<IconData>().Get(weaponData.iconSpriteId).iconName);
    //        itemName.text = DataTableMgr.GetTable<NameData>().Get(weaponData.name).name;
    //        itemInfo.text = DataTableMgr.GetTable<DescData>().Get(weaponData.desc).text;
    //    }
    //}

    //public void CurrArmorSet()
    //{
    //    ArmorData armorData = null;
    //    if (inven.CurrArmor != null)
    //        armorData = DataTableMgr.GetTable<ArmorData>().Get(inven.CurrArmor);
    //    if (armorData != null)
    //    {
    //        equipmentImage.sprite = Resources.Load<Sprite>(DataTableMgr.GetTable<IconData>().Get(armorData.iconSpriteId).iconName);
    //        itemName.text = DataTableMgr.GetTable<NameData>().Get(armorData.name).name;
    //        itemInfo.text = DataTableMgr.GetTable<DescData>().Get(armorData.desc).text;
    //    }
    //}

    //public void CurrEquipView()
    //{
    //    WeaponData weaponData = null;
    //    ArmorData armorData = null;
    //    if (inven.CurrWeapon != null)
    //        weaponData = DataTableMgr.GetTable<WeaponData>().Get(inven.CurrWeapon);
    //    if (inven.CurrArmor != null)
    //        armorData = DataTableMgr.GetTable<ArmorData>().Get(inven.CurrArmor);

    //    if(weaponData != null)
    //    {
    //        currWeaponImage.sprite = Resources.Load<Sprite>(DataTableMgr.GetTable<IconData>().Get(weaponData.iconSpriteId).iconName);
    //    }
    //    if(armorData != null)
    //    {
    //        currArmorImage.sprite = Resources.Load<Sprite>(DataTableMgr.GetTable<IconData>().Get(armorData.iconSpriteId).iconName);
    //    }
    //}
}
