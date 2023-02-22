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
        //tables.Add(typeof(데이터), new DataTable<데이터형>(경로));
        isLoaded = true;
    }

    public static DataTable<T> GetTable<T>() where T : ICSVParsing, new()
    {
        if (!isLoaded)
            LoadAll();

        return tables[typeof(T)] as DataTable<T>;
    }
}
