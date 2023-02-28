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
        tables.Add(typeof(ItemData), new DataTable<ItemData>("DataTables/ItemTable"));
        tables.Add(typeof(WeaponData), new DataTable<WeaponData>("DataTables/WeaponTable"));
        //tables.Add(typeof(SkillData), new DataTable<SkillData>("DataTables/SkillTable"));
        isLoaded = true;
    }

    public static DataTable<T> GetTable<T>() where T : ICSVParsing, new()
    {
        if (!isLoaded)
            LoadAll();

        return tables[typeof(T)] as DataTable<T>;
    }
}
