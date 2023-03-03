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
    public struct Value
    {
        public int str;
        public int dex;
        public int intel;
        public int atkPower;
        public int skillPower;
        public float criticalChance;
        public float criticalDamage;
        public int defense;
        public int maxHp;
        public int maxMp;

        public static Value operator+(Value lhs, Value rhs)
        {
            Value newValue;
            newValue.str = lhs.str + rhs.str;
            newValue.dex = lhs.dex + rhs.dex;
            newValue.intel = lhs.intel + rhs.intel;
            newValue.atkPower = lhs.atkPower + rhs.atkPower;
            newValue.skillPower = lhs.skillPower + rhs.skillPower;
            newValue.criticalChance = lhs.criticalChance + rhs.criticalChance;
            newValue.criticalDamage = lhs.criticalDamage + rhs.criticalDamage;
            newValue.defense = lhs.defense + rhs.defense;
            newValue.maxHp = lhs.maxHp + rhs.maxHp;
            newValue.maxMp = lhs.maxMp + rhs.maxMp;
            return newValue;
        }
    }

    public Types type;
    public string id;
    private Value defaultValue;
    public Value FinalValue { get; private set; }
    public int currHp;
    public int currMp;

    private void Start()
    {
        LoadFromTable();
        currHp = FinalValue.maxHp;
        currMp = FinalValue.maxMp;
        if (CompareTag("Player"))
        {
            SetHpUi();
            SetMpUi();
        }
    }

    private void LoadFromTable()
    {
        StatusData data = null;
        switch (type)
        {
            case Types.Player:
                data = DataTableMgr.GetTable<PlayerData>().Get(id);
                break;
            case Types.Monster:
                data = DataTableMgr.GetTable<EnemyData>().Get(id);
                break;
        }
        defaultValue.str = data.str;
        defaultValue.dex = data.dex;
        defaultValue.intel = data.intel;
        defaultValue.atkPower = data.attackPower;
        defaultValue.skillPower = data.skillPower;
        defaultValue.criticalChance = data.criticalChance;
        defaultValue.criticalDamage = data.criticalDamage;
        defaultValue.defense = data.defense;
        defaultValue.maxHp = data.maxHp;
        defaultValue.maxMp = data.maxMp;
        FinalValue = defaultValue;
    }

    public void AddValue(Value addValue) => FinalValue = defaultValue + addValue;

    public void SetHpUi() => GameManager.instance.uiManager.PlayerHpBar(FinalValue.maxHp, currHp);
    public void SetMpUi() => GameManager.instance.uiManager.PlayerMpBar(FinalValue.maxMp, currMp);
}
