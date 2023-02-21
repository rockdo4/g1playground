using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICSVParsing
{
    string id { get; set; }
    public void Parse(Dictionary<string, string> line);
}
