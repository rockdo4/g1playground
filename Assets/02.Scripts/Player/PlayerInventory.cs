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

    private PlayerStatus status;
    private PlayerAttack playerAttack;
    public string[] defaultWeapons;    // for test
    public string[] defaultArmors;     // for test
    [SerializeField] public Consumable[] defaultConsumables;   // for test

    public List<string> Weapons { get; private set; } = new List<string>();
    public List<string> Armors { get; private set; } = new List<string>();
    public List<Consumable> Consumables { get; private set; } = new List<Consumable>();
    [field: SerializeField] public string CurrWeapon { get; private set; }
    [field: SerializeField] public string CurrArmor { get; private set; }

    // if in-game consumable item except potion exists, need to change code
    public enum Potions
    {
        Hp,
        Mp,
    }
    public int[] PotionCount { get; private set; } = new int[2];
    public float[] potionRate = new float[2];

    private void Awake()
    {
        status = GetComponent<PlayerStatus>();
        playerAttack = GetComponent<PlayerAttack>();
    }

    public void SetDefault()
    {
        Weapons = defaultWeapons.ToList();
        Armors = defaultArmors.ToList();
        Consumables = defaultConsumables.ToList();
        SetWeapon(0);
        SetArmor(0);
    }

    public void Load(List<string> weapons, List<string> armors, List<Consumable> consumables, string currWeapon, string currArmor)
    {
        Weapons = weapons;
        Armors = armors;
        Consumables = consumables;
        CurrWeapon = currWeapon;
        if (CurrWeapon != "" && CurrWeapon != null)
        {
            playerAttack.SetWeapon(DataTableMgr.GetTable<WeaponData>().Get(CurrWeapon).weaponType);
        }
        CurrArmor = currArmor;
        ApplyStatus();
    }

    public void SetWeapon(int index)
    {
        if (string.IsNullOrEmpty(Weapons[index]))
            return;
        var temp = CurrWeapon;
        CurrWeapon = Weapons[index];
        if (string.IsNullOrEmpty(temp))
            RemoveWeapon(index);
        else
            Weapons[index] = temp;
        playerAttack.SetWeapon(DataTableMgr.GetTable<WeaponData>().Get(CurrWeapon).weaponType);
        ApplyStatus();
        PlayerDataManager.instance.SaveInventory();
        PlayerDataManager.instance.SaveFile();
    }

    public void SetArmor(int index)
    {
        if (string.IsNullOrEmpty(Armors[index]))
            return;
        var temp = CurrArmor;
        CurrArmor = Armors[index];
        if (string.IsNullOrEmpty(temp))
            RemoveArmor(index);
        else
            Armors[index] = temp;
        ApplyStatus();
        PlayerDataManager.instance.SaveInventory();
        PlayerDataManager.instance.SaveFile();
    }

    public void SetEmpty(ItemTypes itemType)
    {
        switch (itemType)
        {
            case ItemTypes.Weapon:
                if (string.IsNullOrEmpty(CurrWeapon))
                    return;
                AddWeapon(CurrWeapon);
                CurrWeapon = null;
                playerAttack.SetWeapon(WeaponTypes.None);
                break;
            case ItemTypes.Armor:
                AddArmor(CurrArmor);
                CurrArmor = null;
                break;
            default:
                return;
        }
        ApplyStatus();
        PlayerDataManager.instance.SaveInventory();
        PlayerDataManager.instance.SaveFile();
    }

    public void ApplyStatus()
    {
        List<Status.Value> values = new List<Status.Value>();
        values.Add(GetWeaponStat(CurrWeapon));
        values.Add(GetArmorStat(CurrArmor));
        status.Equip(values);
    }

    public Status.Value GetWeaponStat(string weaponId)
    {
        var weaponValue = Status.Value.Zero;
        WeaponData weaponData = null;
        if (!string.IsNullOrEmpty(weaponId))
        {
            weaponData = DataTableMgr.GetTable<WeaponData>().Get(weaponId);
            if (weaponData != null)
            {
                weaponValue.str += weaponData.addStr;
                weaponValue.dex += weaponData.addDex;
                weaponValue.intel += weaponData.addInt;
                weaponValue.meleePower += weaponData.addMeleePower;
                weaponValue.skillPower += weaponData.addSkillPower;
                //add.meleeCriChance += weaponData.addMeleeCriChance;
                //add.meleeCriDamage += weaponData.addMeleeCriDamage;
            }
        }
        return weaponValue;
    }

    public Status.Value GetArmorStat(string armorId)
    {
        var armorValue = Status.Value.Zero;
        ArmorData armorData = null;
        if (!string.IsNullOrEmpty(armorId))
        {
            armorData = DataTableMgr.GetTable<ArmorData>().Get(armorId);
            if (armorData != null)
            {
                armorValue.str += armorData.addStr;
                armorValue.dex += armorData.addDex;
                armorValue.intel += armorData.addInt;
                armorValue.meleeDef += armorData.addMeleeDef;
                armorValue.skillDef += armorData.addSkillDef;
            }
        }
        return armorValue;
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
        PlayerDataManager.instance.SaveInventory();
        PlayerDataManager.instance.SaveFile();
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
        PlayerDataManager.instance.SaveInventory();
        PlayerDataManager.instance.SaveFile();
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
                    count = 0;
                    break;
                }
            }
        }

        while (count > 0)
        {
            Consumable newConsumable;
            newConsumable.id = id;
            if (count < maxCount)
            {
                newConsumable.count = count;
                count = 0;
            }
            else
            {
                newConsumable.count = maxCount;
                count -= maxCount;
            }
            Consumables.Add(newConsumable);
        }

        PlayerDataManager.instance.SaveInventory();
        PlayerDataManager.instance.SaveFile();
    }

    public void RemoveWeapon(int index)
    {
        if (index < 0)
        {
            CurrWeapon = null;
            playerAttack.SetWeapon(WeaponTypes.None);
        }
        else
            Weapons.RemoveAt(index);
        PlayerDataManager.instance.SaveInventory();
        PlayerDataManager.instance.SaveFile();
    }

    public void RemoveArmor(int index)
    {
        if (index < 0)
            CurrArmor = null;
        else
            Armors.RemoveAt(index);
        PlayerDataManager.instance.SaveInventory();
        PlayerDataManager.instance.SaveFile();
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
        PlayerDataManager.instance.SaveInventory();
        PlayerDataManager.instance.SaveFile();
    }

    public void Disassemble(ItemTypes type, int index)
    {
        if (type == ItemTypes.Consumable)
            return;

        int powderCount = 0;
        var table = DataTableMgr.GetTable<ItemdisassembleData>().GetTable();
        switch (type)
        {
            case ItemTypes.Weapon:
                {
                    string id;
                    if (index < 0)
                        id = CurrWeapon;
                    else
                        id = Weapons[index];
                    RemoveWeapon(index);
                    var weaponData = DataTableMgr.GetTable<WeaponData>().Get(id);
                    foreach (var data in table)
                    {
                        if (weaponData.weaponClass == data.Value.itemClass && weaponData.reinforce == data.Value.itemReinforce)
                        {
                            powderCount = data.Value.powder;
                            break;
                        }
                    }
                }
                break;
            case ItemTypes.Armor:
                {
                    string id;
                    if (index < 0)
                        id = CurrArmor;
                    else
                        id = Armors[index];
                    RemoveArmor(index);
                    var armorData = DataTableMgr.GetTable<ArmorData>().Get(id);
                    foreach (var data in table)
                    {
                        if (armorData.armorClass == data.Value.itemClass && armorData.reinforce == data.Value.itemReinforce)
                        {
                            powderCount = data.Value.powder;
                            break;
                        }
                    }
                }
                break;
            default:
                break;
        }
        var consumeTable = DataTableMgr.GetTable<ConsumeData>().GetTable();
        string powderId = null;
        foreach (var data in consumeTable)
        {
            if (data.Value.consumeType == ConsumeTypes.Powder)
            {
                powderId = data.Value.id;
                break;
            }
        }
        AddConsumable(powderId, powderCount);
        ApplyStatus();
        PlayerDataManager.instance.SaveInventory();
        PlayerDataManager.instance.SaveFile();
    }

    public void Compose(ItemTypes type, ComposeData data, int[] indexs)
    {
        if (type == ItemTypes.Consumable)
            return;

        Array.Reverse(indexs);
        var len = indexs.Length;
        switch (type)
        {
            case ItemTypes.Weapon:
                for (int i = 0; i < len; ++i)
                {
                    RemoveWeapon(indexs[i]);
                }
                AddWeapon(data.resultItem);
                break;
            case ItemTypes.Armor:
                for (int i = 0; i < len; ++i)
                {
                    RemoveArmor(indexs[i]);
                }
                AddArmor(data.resultItem);
                break;
        }
        ApplyStatus();
        PlayerDataManager.instance.SaveInventory();
        PlayerDataManager.instance.SaveFile();
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
        PlayerDataManager.instance.SaveInventory();
        PlayerDataManager.instance.SaveFile();
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
                status.CurrHp += (int)(status.FinalValue.maxHp * potionRate[0]);
                if (status.CurrHp > status.FinalValue.maxHp)
                    status.CurrHp = status.FinalValue.maxHp;
                break;
            case Potions.Mp:
                if (status.CurrMp == status.FinalValue.maxMp)
                    return;
                status.CurrMp += (int)(status.FinalValue.maxMp * potionRate[1]);
                if (status.CurrMp > status.FinalValue.maxMp)
                    status.CurrMp = status.FinalValue.maxMp;
                break;
        }
        PotionCount[(int)potion] -= 1;
    }

    public void UseHpPotion() => UsePotion(Potions.Hp);
    public void UseMpPotion() => UsePotion(Potions.Mp);
}