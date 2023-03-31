using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class UIItemInfo : MonoBehaviour
{
    public Image weaponImage;
    public Image armorImage;
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

    private int incdecValue;

    private void Awake()
    {
        hpBar = GetComponentInChildren<MaxHpBar>(true);
        mpBar = GetComponentInChildren<MaxMp>(true);
        skillPowBar = GetComponentInChildren<SkillPowerBar>(true);
        skillDefBar = GetComponentInChildren<SkillDefBar>(true);
        attPowBar = GetComponentInChildren<MeleePowBar>(true);
        attDefBar = GetComponentInChildren<MeleeDefBar>(true);
    }
    public void SetEmpty()
    {
        weaponImage.sprite = null;
        armorImage.sprite = null;
        itemName.text = string.Empty;
        itemInfo.text = string.Empty;
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

    public void ShowSlider(Status.Value value)
    {
        hpBar.SetImageFillAmount(value.maxHp);
        mpBar.SetImageFillAmount(value.maxMp);
        skillPowBar.SetImageFillAmount(value.skillPower);
        skillDefBar.SetImageFillAmount(value.skillDef);
        attPowBar.SetImageFillAmount(value.meleePower);
        attDefBar.SetImageFillAmount(value.meleeDef);
    }
}
