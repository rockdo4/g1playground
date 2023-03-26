using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Pool;

public abstract class Status : MonoBehaviour
{
    public enum Types
    {
        Player,
        Monster,
    }

    [Serializable]
    public struct Value
    {
        public int str;
        public int dex;
        public int intel;
        public int meleePower;
        public int skillPower;
        public float meleeCriChance;
        public float meleeCriDamage;
        public float skillCriChance;
        public float skillCriDamage;
        public int meleeDef;
        public int skillDef;
        public int maxHp;
        public int maxMp;

        public static Value Zero
        {
            get
            {
                Value value;
                value.str = 0;
                value.dex = 0;
                value.intel = 0;
                value.meleePower = 0;
                value.skillPower = 0;
                value.meleeCriChance = 0;
                value.meleeCriDamage = 0;
                value.skillCriChance = 0;
                value.skillCriDamage = 0;
                value.meleeDef = 0;
                value.skillDef = 0;
                value.maxHp = 0;
                value.maxMp = 0;
                return value;
            }
        }

        public static Value operator +(Value lhs, Value rhs)
        {
            Value newValue;
            newValue.str = lhs.str + rhs.str;
            newValue.dex = lhs.dex + rhs.dex;
            newValue.intel = lhs.intel + rhs.intel;
            newValue.meleePower = lhs.meleePower + rhs.meleePower;
            newValue.skillPower = lhs.skillPower + rhs.skillPower;
            newValue.meleeCriChance = lhs.meleeCriChance + rhs.meleeCriChance;
            newValue.meleeCriDamage = lhs.meleeCriDamage + rhs.meleeCriDamage;
            newValue.skillCriChance = lhs.skillCriChance + rhs.skillCriChance;
            newValue.skillCriDamage = lhs.skillCriDamage + rhs.skillCriDamage;
            newValue.meleeDef = lhs.meleeDef + rhs.meleeDef;
            newValue.skillDef = lhs.skillDef + rhs.skillDef;
            newValue.maxHp = lhs.maxHp + rhs.maxHp;
            newValue.maxMp = lhs.maxMp + rhs.maxMp;
            return newValue;
        }
    }

    protected Types type;
    public string id;
    protected bool isLoaded = false;
    protected Value defaultValue;
    public Value DebuffValue { get; protected set; }
    [field: SerializeField] public Value FinalValue { get; protected set; }
    protected int currHp;
    public int CurrHp
    {
        get => currHp;
        set
        {
            currHp = value;
            SetHpUi();
        }
    }
    protected int currMp;
    public int CurrMp
    {
        get => currMp;
        set
        {
            currMp = value;
            SetMpUi();
        }
    }

    protected void Start()
    {
        LoadFromTable();
        CurrHp = FinalValue.maxHp;
        CurrMp = FinalValue.maxMp;
        SetHpUi();
        SetMpUi();
    }

    public void LoadFromTable()
    {
        StatusData data = null;
        switch (type)
        {
            case Types.Player:
                data = DataTableMgr.GetTable<PlayerData>().Get(id);
                break;
            case Types.Monster:
                data = DataTableMgr.GetTable<MonsterData>().Get(id);
                break;
        }
        defaultValue.str = data.str;
        defaultValue.dex = data.dex;
        defaultValue.intel = data.intel;
        defaultValue.meleeCriDamage = data.meleeCriDamage;
        defaultValue.skillCriDamage = data.skillCriDamage;
        //defaultValue.meleePower = data.meleePower;
        //defaultValue.skillPower = data.skillPower;
        //defaultValue.meleeCriChance = data.meleeCriChance;
        //defaultValue.skillCriChance = data.skillCriChance;
        //defaultValue.meleeDef = data.meleeDef;
        //defaultValue.skillDef = data.skillDef;
        defaultValue.maxHp = data.maxHp;
        defaultValue.maxMp = data.maxMp;
        isLoaded = true;
        defaultValue = CalculateValue(defaultValue);
        SetFinalValue();
    }

    protected abstract Value CalculateValue(Value value);
    public virtual void AddValue(List<Value> addValue) { }
    public void Debuff(Value value)
    {
        DebuffValue = value;
        SetFinalValue();
    }
    protected abstract void SetFinalValue();
    public virtual void SetHpUi() { }
    public virtual void SetMpUi() { }
}
