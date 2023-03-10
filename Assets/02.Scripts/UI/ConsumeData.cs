using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ConsumeTypes
{
    Potion,
    Powder,
    Two,
    Three,
}

public class ConsumeData : ItemData
{
    public ConsumeTypes consumeType;
    public string iconId;
    public int carryoverlap;

    public override void Parse(Dictionary<string, string> line)
    {
        base.Parse(line);
        consumeType = (ConsumeTypes)System.Enum.Parse(typeof(ConsumeTypes), line["Type"]);
        carryoverlap = int.Parse(line["carry_overlap"]);
    }
}
