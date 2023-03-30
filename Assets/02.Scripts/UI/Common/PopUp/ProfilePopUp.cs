using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfilePopUp : PopupUI
{
    public MaxHpBar maxHpBar;
    public MaxMp maxMpBar;
    public MeleeDefBar meleeDefBar;
    public MeleePowBar meleePowBar;
    public SkillDefBar skillDefBar;
    public SkillPowerBar skillPowerBar;

    private int hp;
    private int mp;
    private int sp;
    private int sd;
    private int ma;
    private int md;

    protected void Awake()
    {
        maxHpBar = GetComponentInChildren<MaxHpBar>();
        maxMpBar = GetComponentInChildren<MaxMp>();
        meleeDefBar = GetComponentInChildren<MeleeDefBar>();
        meleePowBar = GetComponentInChildren<MeleePowBar>();
        skillDefBar = GetComponentInChildren<SkillDefBar>();
        skillPowerBar = GetComponentInChildren<SkillPowerBar>();
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        UI.Instance.popupPanel.menuSlider.ActiveFalse();
    }

    private void Start()
    {
        GetPlayerStat();
    }

    public void SetStat()
    {
        maxHpBar.SetImageFillAmount(hp);
        maxMpBar.SetImageFillAmount(mp);
        meleeDefBar.SetImageFillAmount(sp);
        meleePowBar.SetImageFillAmount(sd);
        skillDefBar.SetImageFillAmount(ma);
        skillDefBar.SetImageFillAmount(md);
    }

    public void GetPlayerStat()
    {
        hp = GameManager.instance.player.GetComponent<Status>().FinalValue.maxHp;
        mp = GameManager.instance.player.GetComponent<Status>().FinalValue.maxMp;
        sp = GameManager.instance.player.GetComponent<Status>().FinalValue.skillPower;
        sd = GameManager.instance.player.GetComponent<Status>().FinalValue.skillDef;
        ma = GameManager.instance.player.GetComponent<Status>().FinalValue.meleePower;
        md = GameManager.instance.player.GetComponent<Status>().FinalValue.meleeDef;

        SetStat();
    }

    public string weaponId;
    public string armorId;
    public void GetItemStat(string newWeaponId, string newArmorId)
    {
        weaponId = GameManager.instance.player.GetComponent<PlayerInventory>().CurrWeapon;
        armorId = GameManager.instance.player.GetComponent<PlayerInventory>().CurrArmor;
        //var weaponData = DataTableMgr.GetTable<WeaponData>().Get(weaponId);
        //var armorData = DataTableMgr.GetTable<ArmorData>().Get(armorId);

        //Status.Value weaponValue = new Status.Value();
        //Status.Value armorValue = new Status.Value();

        //if (weaponData != null)
        //{
        //    weaponValue.str += weaponData.addStr;
        //    weaponValue.dex += weaponData.addDex;
        //    weaponValue.intel += weaponData.addInt;
        //    weaponValue.meleePower += weaponData.addMeleePower;
        //    weaponValue.skillPower += weaponData.addSkillPower;
        //}
        //if (armorData != null)
        //{
        //    armorValue.str += armorData.addStr;
        //    armorValue.dex += armorData.addDex;
        //    armorValue.intel += armorData.addInt;
        //    armorValue.meleeDef += armorData.addMeleeDef;
        //    armorValue.skillDef += armorData.addSkillDef;
        //}

        //List<Status.Value> values = new List<Status.Value>();
        //values.Add(weaponValue);
        //values.Add(armorValue);

        GameManager.instance.player.GetComponent<PlayerInventory>().SetDefault();

        GameManager.instance.player.GetComponent<PlayerInventory>().AddWeapon(newWeaponId);
        GameManager.instance.player.GetComponent<PlayerInventory>().AddArmor(newArmorId);

        hp = GameManager.instance.player.GetComponent<Status>().FinalValue.maxHp;
        mp = GameManager.instance.player.GetComponent<Status>().FinalValue.maxMp;
        sp = GameManager.instance.player.GetComponent<Status>().FinalValue.skillPower;
        sd = GameManager.instance.player.GetComponent<Status>().FinalValue.skillDef;
        ma = GameManager.instance.player.GetComponent<Status>().FinalValue.meleePower;
        md = GameManager.instance.player.GetComponent<Status>().FinalValue.meleeDef;

        GameManager.instance.player.GetComponent<PlayerInventory>().SetDefault();
        GameManager.instance.player.GetComponent<PlayerInventory>().AddWeapon(weaponId);
        GameManager.instance.player.GetComponent<PlayerInventory>().AddArmor(armorId);
    }
}