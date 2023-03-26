using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerStatus : Status
{
    private Value equipValue;

    private void Awake()
    {
        type = Types.Player;
    }

    public override void AddValue(List<Value> addValue)
    {
        Value value = new Value();
        for (int i = 0; i < addValue.Count; ++i)
        {
            addValue[i] = CalculateValue(addValue[i]);
            value += addValue[i];
        }
        equipValue = value;
        SetFinalValue();
        SetHpUi();
        SetMpUi();
    }

    protected override Value CalculateValue(Value value)
    {
        value.meleePower += (int)(value.str / 70f * value.dex / 30f);
        value.meleeCriChance += value.dex / 75f / 100f;
        value.meleeDef += (int)(value.str / 10f);
        value.skillPower += (int)(value.intel / 15f);
        value.skillCriChance += value.intel / 75f / 100f;
        value.skillDef += (int)(value.intel / 10f);
        return value;
    }

    public override void SetHpUi() => GameManager.instance.uiManager.PlayerHpBar(FinalValue.maxHp, CurrHp);
    public override void SetMpUi() => GameManager.instance.uiManager.PlayerMpBar(FinalValue.maxMp, CurrMp);

    protected override void SetFinalValue() => FinalValue = defaultValue + equipValue + DebuffValue;
}
