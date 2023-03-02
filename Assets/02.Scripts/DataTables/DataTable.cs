using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DataTable
{
    protected string csvFilPath = string.Empty;
}

public class DataTable<T> : DataTable where T : ICSVParsing, new()
{
    private Dictionary<string, T> tables = new Dictionary<string, T>();

    public DataTable(string path)
    {
        csvFilPath = path;
        Load();
    }

    public void Load()
    {
        var lines = CSVReader.Read(csvFilPath);
        foreach (var line in lines)
        {
            var data = new T();
            data.Parse(line);
            tables.Add(data.id, data);
        }
    }

    public List<string> GetAllIds()
    {
        return tables.Keys.ToList();
    }

    public T Get(string id)
    {
        if (!tables.ContainsKey(id))
            return default;
        return tables[id];
    }

    public Dictionary<string, T> GetTable()
    {
        return tables;
    }
}
