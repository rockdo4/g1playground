using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapData : ICSVParsing
{
    public string id { get; set; }
    public string miniMapId;

    public Sprite miniMapSprite;

    public void Parse(Dictionary<string, string> line)
    {
        id = line["ID"];
        miniMapId = line["Stage_Map"];


        miniMapSprite = Resources.Load<Sprite>(miniMapId);
    }
}
