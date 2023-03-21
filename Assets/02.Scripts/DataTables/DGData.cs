using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DGData : ICSVParsing
{
    public string id { get; set; }
    public string name;
    public string desc;
    public string iconId;
    public string attribution;
    public int level;
    public string apMonster1Id;
    public int quantity1;
    public string apMonster2Id;
    public int quantity2;
    public string apMonster3Id;
    public int quantity3;
    public string apMonster4Id;
    public int quantity4;
    public string apMonster5Id;
    public int quantity5;

    public string rwItem1Id;
    public int rwItemQuantity1;
    public string rwItem2Id;
    public int rwItemQuantity2;

    public void Parse(Dictionary<string, string> line)
    {
        id = line["Dg_ID"];
        name = line["name"];
        desc = line["desc"];
        iconId = line["Icon_ID"];

        attribution = line["attribution"];
        level = int.Parse(line["level"]);

        apMonster1Id = line["ap_monster1_ID"];
        quantity1 = int.Parse(line["quantity"]);
        apMonster2Id = line["ap_monster1_ID"];
        quantity2 = int.Parse(line["quantity"]);
        apMonster3Id = line["ap_monster1_ID"];
        quantity3 = int.Parse(line["quantity"]);
        apMonster4Id = line["ap_monster1_ID"];
        quantity4 = int.Parse(line["quantity"]);
        apMonster5Id = line["ap_monster1_ID"];
        quantity5 = int.Parse(line["quantity"]);

        rwItem1Id = line["rw_item_ID"];
        rwItemQuantity1 = int.Parse(line["rw_item_quantity"]);
        rwItem2Id = line["rw_item_2_ID"];
        rwItemQuantity2 = int.Parse(line["rw_item_quantity_2"]);
    }
}
