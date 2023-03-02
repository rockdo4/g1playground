using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private Status status;
    public string[] weapons;    // for test
    public string[] armors;     // for test
    //private List<string> weapons = new LinkedList<string>();
    //private List<string> slots = new LinkedList<string>();
    public string CurrWeapon { get; private set; }
    public string CurrArmor { get; private set; }

    private void Awake()
    {
        status = GetComponent<Status>();
    }

    public void SetWeapon(int index)
    {
        if (weapons[index] == null)
            return;
        CurrWeapon = weapons[index];
        weapons[index] = null;
        ApplyStatus();
    }

    public void SetArmor(int index)
    {
        if (armors[index] == null)
            return;
        CurrArmor = armors[index];
        armors[index] = null;
        ApplyStatus();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            SetWeapon(0);
        if (Input.GetKeyDown(KeyCode.R))
            SetArmor(0);
        Debug.Log(status.FinalValue.str);
    }

    public void ApplyStatus()
    {
        WeaponData weaponData = null;
        ArmorData armorData = null;
        if (CurrWeapon != null)
            weaponData = DataTableMgr.GetTable<WeaponData>().Get(CurrWeapon);
        if (CurrArmor != null)
            armorData = DataTableMgr.GetTable<ArmorData>().Get(CurrArmor);
        Status.Value add = new Status.Value();
        if (weaponData != null)
        {
            add.str += weaponData.addStr;
            add.dex += weaponData.addDex;
            add.intel += weaponData.addInt;
            add.atkPower += weaponData.addMeleePower;
            add.skillPower += weaponData.addSkillPower;
            add.criticalChance += weaponData.addMeleeCriChance;
            add.criticalDamage += weaponData.addMeleeCriDamage;
        }
        if (armorData != null)
        {
            add.str += armorData.addStr;
            add.dex += armorData.addDex;
            add.intel += armorData.addInt;
            add.defense += armorData.addDef;
        }
        status.AddValue(add);
    }
}
