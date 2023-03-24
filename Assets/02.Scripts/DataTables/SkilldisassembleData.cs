using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ItemdisassembleData : ICSVParsing
{
    public string id { get; set; }
    public ItemClass itemClass;
    public int itemReinforce;
    public int powder;

    void ICSVParsing.Parse(Dictionary<string, string> line)
    {
        id = line["Itemdisassemble_ID"];
        itemClass = (ItemClass)System.Enum.Parse(typeof(ItemClass), line["Item_class"]);
        itemReinforce = int.Parse(line["Item_Reinforce"]);
        powder = int.Parse(line["Powder"]);

    }
}