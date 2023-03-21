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
        tables.Add(typeof(PlayerData), new DataTable<PlayerData>("DataTables/PlayerTable"));
        tables.Add(typeof(EnemyData), new DataTable<EnemyData>("DataTables/EnemyTable"));
        tables.Add(typeof(ConsumeData), new DataTable<ConsumeData>("DataTables/Consume_Table"));
        tables.Add(typeof(SkillData), new DataTable<SkillData>("DataTables/Skill_Table"));
        tables.Add(typeof(StoryData), new DataTable<StoryData>("DataTables/Story_Table"));
        tables.Add(typeof(MiniMapData), new DataTable<MiniMapData>("DataTables/MiniMapTable"));

        //tables.Add(typeof(SkillData), new DataTable<SkillData>("DataTables/Skill_Table"));

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
