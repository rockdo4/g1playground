using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static PlayerInventory;

public class PlayerInventory : MonoBehaviour
{
    [Serializable]
    public struct Consumable
    {
        public string id;
        public int count;
    }

    private Status status;
    public string[] weaponsTemp;    // for test
    public string[] armorsTemp;     // for test
    [SerializeField] public Consumable[] consumablesTemp;   // for test

    public List<string> weapons { get; private set; } = new List<string>();
    public List<string> armors { get; private set; } = new List<string>();
    public List<Consumable> consumables { get; private set; } = new List<Consumable>();
    public string CurrWeapon { get; private set; }
    public string CurrArmor { get; private set; }

    // if in-game consumable item except potion exists, need to change code
    public enum Potions
    {
        Hp,
        Mp,
    }
    public string[] potionIds;
    public int[] PotionCount { get; private set; }

    private void Awake()
    {
        status = GetComponent<Status>();
        weapons = weaponsTemp.ToList();
        armors = armorsTemp.ToList();
        consumables = consumablesTemp.ToList();
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
        if (status.CurrHp < status.FinalValue.maxHp / 2)
            UsePotion(Potions.Hp);
        if (status.CurrMp < status.FinalValue.maxMp / 2)
            UsePotion(Potions.Mp);
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
            add.meleePower += weaponData.addMeleePower;
            add.skillPower += weaponData.addSkillPower;
            add.meleeCriChance += weaponData.addMeleeCriChance;
            add.meleeCriDamage += weaponData.addMeleeCriDamage;
        }
        if (armorData != null)
        {
            add.str += armorData.addStr;
            add.dex += armorData.addDex;
            add.intel += armorData.addInt;
            add.meleeDef += armorData.addMeleeDef;
            add.skillDef += armorData.addSkillDef;
        }
        status.AddValue(add);
    }

    public int GetCount(string itemId)
    {
        int count = 0;
        foreach (var consumable in consumables)
        {
            if (consumable.id == itemId)
                count += consumable.count;
        }
        return count;
    }

    public void RefillPotions()
    {
        for (int i = 0; i < PotionCount.Length; ++i)
        {
            var count = GetCount(potionIds[i]);
            if (count < 3)
                PotionCount[i] = count;
            else
                PotionCount[i] = 3;
        }
    }

    public void UseConsumable(string id)
    {
        int len = consumables.Count;
        for (int i = len - 1; i >= 0; --i)
        {
            if (consumables[i].id == id)
            {
                var newConsumable = consumables[i];
                if (--newConsumable.count < 1)
                    consumables.Remove(consumables[i]);
                else
                    consumables[i] = newConsumable;
            }
        }
    }

    public void UsePotion(Potions potion)
    {
        if (PotionCount[(int)potion] < 1)
            return;
        switch (potion)
        {
            case Potions.Hp:
                if (status.CurrHp == status.FinalValue.maxHp)
                    return;
                status.CurrHp = status.FinalValue.maxHp;
                break;
            case Potions.Mp:
                if (status.CurrMp == status.FinalValue.maxMp)
                    return;
                status.CurrMp = status.FinalValue.maxMp;
                break;
        }
        PotionCount[(int)potion] -= 1;
        var potionId = potionIds[(int)potion];
        UseConsumable(potionId);
    }
}
