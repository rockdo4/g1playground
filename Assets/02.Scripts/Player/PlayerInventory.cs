using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
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
    private string[] potionIds = new string[2];
    public int[] PotionCount { get; private set; } = new int[2];

    private void Awake()
    {
        status = GetComponent<Status>();
        weapons = weaponsTemp.ToList();
        armors = armorsTemp.ToList();
        consumables = consumablesTemp.ToList();
        var consumeTable = DataTableMgr.GetTable<ConsumeData>().GetTable();
        foreach (var data in consumeTable)
        {
            switch (data.Value.consumeType)
            {
                case ConsumeTypes.HpPotion:
                    potionIds[0] = data.Value.id;
                    break;
                case ConsumeTypes.MpPotion:
                    potionIds[1] = data.Value.id;
                    break;
            }
        }
    }

    public void SetWeapon(int index)
    {
        if (weapons[index] == null)
            return;
        var temp = CurrWeapon;
        CurrWeapon = weapons[index];
        weapons[index] = temp;
        ApplyStatus();
    }

    public void SetArmor(int index)
    {
        if (armors[index] == null)
            return;
        var temp = CurrArmor;
        CurrArmor = armors[index];
        armors[index] = temp;
        ApplyStatus();
    }

    private void Update()
    {
        if (status.CurrHp < status.FinalValue.maxHp / 2)
            UseHpPotion();
        if (status.CurrMp < status.FinalValue.maxMp / 2)
            UseMpPotion();
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

    public void Reinforce(ItemTypes type, int index, string newId)
    {
        switch (type)
        {
            case ItemTypes.Weapon:
                if (index < 0)
                    CurrWeapon = newId;
                else
                    weapons[index] = newId;
                break;
            case ItemTypes.Armor:
                if (index < 0)
                    CurrArmor = newId;
                else
                    armors[index] = newId;
                break;
            default:
                break;
        }
    }

    public int GetConsumableCount(string itemId)
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
            var count = GetConsumableCount(potionIds[i]);
            if (count < 3)
                PotionCount[i] = count;
            else
                PotionCount[i] = 3;
        }
    }

    public void UseConsumable(string id, int need = 1)
    {
        int len = consumables.Count;
        for (int i = len - 1; i >= 0; --i)
        {
            if (consumables[i].id == id)
            {
                var newConsumable = consumables[i];
                if (need < newConsumable.count)
                {
                    newConsumable.count -= need;
                    consumables[i] = newConsumable;
                    return;
                }
                else
                {
                    need -= newConsumable.count;
                    consumables.Remove(consumables[i]);
                }
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

    public void UseHpPotion() => UsePotion(Potions.Hp);
    public void UseMpPotion() => UsePotion(Potions.Mp);
}
