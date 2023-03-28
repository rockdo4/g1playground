using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : Status
{
    private void Awake()
    {
        type = Types.Monster;
    }

    protected void OnEnable()
    {
        if (!isLoaded)
            LoadFromTable();
        if (type == Types.Monster)
            CurrHp = FinalValue.maxHp;
    }

    protected override Value CalculateValue(Value value)
    {
        var damageFigure = DataTableMgr.GetTable<MonsterData>().Get(id).damageFigure;
        value.meleePower += (int)(value.str * 1.7f * damageFigure);
        value.meleeCriChance += value.dex / 200f / 100f;
        value.meleeDef += (int)(value.str / 10f);
        value.skillPower += (int)(value.intel / 5f * damageFigure);
        value.skillCriChance += value.intel / 200f / 100f;
        value.skillDef += (int)(value.intel / 10f);
        return value;
    }

    protected override void SetFinalValue()
    {
        var value = defaultValue;
        value.meleeDef *= (int)(1f - reduceDef);
        value.skillDef *= (int)(1f - reduceDef);
        FinalValue = value;
    }
}
