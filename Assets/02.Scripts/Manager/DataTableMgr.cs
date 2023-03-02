using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataTableMgr
{
    private static Dictionary<Type, DataTable> tables = new Dictionary<Type, DataTable>();
    private static bool isLoaded = false;

    public static void LoadAll()
    {
        tables.Add(typeof(WeaponData), new DataTable<WeaponData>("DataTables/WeaponTable"));
        tables.Add(typeof(ArmorData), new DataTable<ArmorData>("DataTables/ArmorTable"));

        //tables.Add(typeof(SkillData), new DataTable<SkillData>("DataTables/SkillTable"));

        isLoaded = true;
    }

    public static DataTable<T> Load<T>(DataTable<T> table,string filepath) where T : ICSVParsing, new()
    {
        if (tables.Count != 0 && tables.ContainsKey(typeof(T)))
        {
            tables.Remove(typeof(T));
        }
        tables.Add(typeof(T), new DataTable<T>(filepath));
        return tables[typeof(T)] as DataTable<T>;
    }

    public static DataTable<T> GetTable<T>() where T : ICSVParsing, new()
    {
        if (!isLoaded)
            LoadAll();

        return tables[typeof(T)] as DataTable<T>;
    }
}
