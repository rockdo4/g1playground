using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ConsumeTypes
{
    HpPotion,
    MpPowder,
    Powder,
    Essence,
}

public class ConsumeData : ItemData
{
    public ConsumeTypes consumeType;
    public int carryoverlap;

    public override void Parse(Dictionary<string, string> line)
    {
        base.Parse(line);
        id = line["ID"];
        consumeType = (ConsumeTypes)System.Enum.Parse(typeof(ConsumeTypes), line["Type"]);
        carryoverlap = int.Parse(line["carry_overlap"]);
    }
}
