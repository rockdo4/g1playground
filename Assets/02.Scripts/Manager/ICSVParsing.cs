using System.Collections.Generic;

public interface ICSVParsing
{
    string id { get; set; }
    public void Parse(Dictionary<string, string> line);
}
