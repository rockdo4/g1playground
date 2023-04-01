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

    public Value AddValue(List<Value> addValue)
    {
        Value value = new Value();
        for (int i = 0; i < addValue.Count; ++i)
        {
            addValue[i] = ItemValue(addValue[i]);
            value += addValue[i];
        }
        return(value);
    }

    public void Equip(List<Value> addValue)
    {
        equipValue = AddValue(addValue);
        SetFinalValue();
        SetHpUi();
        SetMpUi();
    }

    protected override Value CalculateValue(Value value)
    {
        value.meleePower = (int)(value.str / 70f * value.dex / 30f);
        value.meleeCriChance = value.dex / 75f / 100f;
        value.meleeDef = (int)(value.str / 10f);
        value.skillPower = (int)(value.intel / 15f);
        value.skillCriChance = value.intel / 75f / 100f;
        value.skillDef = (int)(value.intel / 10f);
        return value;
    }

    private Value ItemValue(Value value)
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

    protected override void SetFinalValue()
    {
        var value = DefaultValue + equipValue;
        value.meleeDef *= (int)(1f - reduceDef);
        value.skillDef *= (int)(1f - reduceDef);
        FinalValue = value;
    }

    public void InitSetFinalValue()
    {
        SetFinalValue();
    }
}
