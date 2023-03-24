using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

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

    public List<string> Weapons { get; private set; } = new List<string>();
    public List<string> Armors { get; private set; } = new List<string>();
    public List<Consumable> Consumables { get; private set; } = new List<Consumable>();
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
        Weapons = weaponsTemp.ToList();
        Armors = armorsTemp.ToList();
        Consumables = consumablesTemp.ToList();
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
        if (Weapons[index] == null)
            return;
        var temp = CurrWeapon;
        CurrWeapon = Weapons[index];
        Weapons[index] = temp;
        ApplyStatus();
    }

    public void SetArmor(int index)
    {
        if (Armors[index] == null)
            return;
        var temp = CurrArmor;
        CurrArmor = Armors[index];
        Armors[index] = temp;
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
            //add.meleeCriChance += weaponData.addMeleeCriChance;
            //add.meleeCriDamage += weaponData.addMeleeCriDamage;
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

    public void AddWeapon(string id)
    {
        var len = Weapons.Count;
        for (int i = 0; i < len; ++i)
        {
            if (string.IsNullOrEmpty(Weapons[i]))
            {
                Weapons[i] = id;
                return;
            }
        }
        Weapons.Add(id);
    }

    public void AddArmor(string id)
    {
        var len = Armors.Count;
        for (int i = 0; i < len; ++i)
        {
            if (string.IsNullOrEmpty(Armors[i]))
            {
                Armors[i] = id;
                return;
            }
        }
        Armors.Add(id);
    }

    public void AddConsumable(string id, int count)
    {
        var len = Consumables.Count;
        var maxCount = DataTableMgr.GetTable<ConsumeData>().Get(id).carryoverlap;
        for (int i = 0; i < len; ++i)
        {
            if (!string.Equals(Consumables[i].id, id))
                continue;

            if (Consumables[i].count < maxCount)
            {
                var newConsumable = Consumables[i];
                var addMax = maxCount - Consumables[i].count;
                if (count > addMax)
                {
                    newConsumable.count = maxCount;
                    count -= addMax;
                }
                else
                {
                    newConsumable.count += count;
                    Consumables[i] = newConsumable;
                    return;
                }
            }
        }
    }

    public void Reinforce(ItemTypes type, int index, string newId)
    {
        switch (type)
        {
            case ItemTypes.Weapon:
                if (index < 0)
                    CurrWeapon = newId;
                else
                    Weapons[index] = newId;
                break;
            case ItemTypes.Armor:
                if (index < 0)
                    CurrArmor = newId;
                else
                    Armors[index] = newId;
                break;
            default:
                break;
        }
        ApplyStatus();
    }

    public void Compose(ItemTypes type, ComposeData data, int[] indexs)
    {
        if (type == ItemTypes.Consumable)
            return;

        switch (type)
        {
            case ItemTypes.Weapon:
                foreach (var index in indexs)
                {
                    if (index < 0)
                        CurrWeapon = null;
                    else
                        Weapons[index] = null;
                }
                AddWeapon(data.resultItem);
                break;
            case ItemTypes.Armor:
                foreach (var index in indexs)
                {
                    if (index < 0)
                        CurrArmor = null;
                    else
                        Armors[index] = null;
                }
                AddArmor(data.resultItem);
                break;
        }
        ApplyStatus();
    }

    public int GetConsumableCount(string itemId)
    {
        int count = 0;
        foreach (var consumable in Consumables)
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
        int len = Consumables.Count;
        for (int i = len - 1; i >= 0; --i)
        {
            if (Consumables[i].id == id)
            {
                var newConsumable = Consumables[i];
                if (need < newConsumable.count)
                {
                    newConsumable.count -= need;
                    Consumables[i] = newConsumable;
                    return;
                }
                else
                {
                    need -= newConsumable.count;
                    Consumables.Remove(Consumables[i]);
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
