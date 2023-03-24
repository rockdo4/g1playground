using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Pool;

public class Status : MonoBehaviour
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

    public Types type;
    public string id;
    private Value defaultValue;
    [SerializeField] public Value FinalValue;//{ get; private set; }
    private int currHp;
    public int CurrHp
    {
        get => currHp;
        set
        {
            currHp = value;
            if (type == Types.Player)
                SetHpUi();
        }
    }
    private int currMp;
    public int CurrMp
    {
        get => currMp;
        set
        {
            currMp = value;
            if (type == Types.Player)
                SetMpUi();
        }
    }

    private void Start()
    {
        LoadFromTable();
        CurrHp = FinalValue.maxHp;
        CurrMp = FinalValue.maxMp;
        if (CompareTag("Player"))
        {
            SetHpUi();
            SetMpUi();
        }
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
        FinalValue = defaultValue;
        Calculate();
    }

    private void Calculate()
    {
        var value = FinalValue;
        switch (type)
        {
            case Types.Player:
                value.meleePower = (int)(value.str / 70f) + (int)(value.dex / 30f);
                value.meleeCriChance = value.dex / 75f / 100f;
                value.meleeDef = (int)(value.str / 10f);
                value.skillPower = (int)(value.intel / 15f);
                value.skillCriChance = value.intel / 75f / 100f;
                value.skillDef = (int)(value.intel / 10f);
                break;
            case Types.Monster:
                var damageFigure = DataTableMgr.GetTable<MonsterData>().Get(id).damageFigure;
                value.meleePower = (int)(value.str * 1.7f * damageFigure);
                value.meleeCriChance = value.dex / 200f / 100f;
                value.meleeDef = (int)(value.str / 10f);
                value.skillPower = (int)(value.intel / 5f * damageFigure);
                value.skillCriChance = value.intel / 200f / 100f;
                value.skillDef = (int)(value.intel / 10f);
                break;
        }
        FinalValue = value;
    }

    public void AddValue(Value addValue)
    {
        FinalValue = defaultValue + addValue;
        Calculate();
        var value = FinalValue;
        value.meleePower += addValue.meleePower;
        value.skillPower += addValue.skillPower;
        value.meleeDef += addValue.meleeDef;
        value.skillDef += addValue.skillDef;
        FinalValue = value;
    }

    public void SetHpUi() => GameManager.instance.uiManager.PlayerHpBar(FinalValue.maxHp, CurrHp);
    public void SetMpUi() => GameManager.instance.uiManager.PlayerMpBar(FinalValue.maxMp, CurrMp);
}
