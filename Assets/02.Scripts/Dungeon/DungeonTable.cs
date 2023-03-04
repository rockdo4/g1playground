using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonData : ICSVParsing
{
    public string id { get; set; }
    public string week;
    public string name;
    public int difficulty;
    public int level;
    public float countdown;
    public string item;
    public int itemcount;

    public void Parse(Dictionary<string, string> line)
    {
        id = line["ID"];
        //    type = (ItemTypes)System.Enum.Parse(typeof(ItemTypes), line["dg_name"]);
        week = line["week"];
        name = line["dg_name"];
        difficulty = Int32.Parse(line["difficulty"]);
        level = Int32.Parse(line["level"]);
        countdown = float.Parse(line["countdown"]);     
        item = line["rw_item"];
        itemcount = Int32.Parse(line["rw_item_quantity"]);

    }

   
}
