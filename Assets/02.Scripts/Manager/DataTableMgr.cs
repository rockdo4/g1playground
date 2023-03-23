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
        tables.Add(typeof(IconData), new DataTable<IconData>("DataTables/Icon_Table"));
        tables.Add(typeof(WeaponData), new DataTable<WeaponData>("DataTables/Weapon_Table"));
        tables.Add(typeof(ArmorData), new DataTable<ArmorData>("DataTables/Armor_Table"));
        tables.Add(typeof(PlayerData), new DataTable<PlayerData>("DataTables/PC_Table"));
        tables.Add(typeof(ConsumeData), new DataTable<ConsumeData>("DataTables/Consume_Table"));
        tables.Add(typeof(SkillData), new DataTable<SkillData>("DataTables/Skill_Table"));
        tables.Add(typeof(AwakenData), new DataTable<AwakenData>("DataTables/Awaken_Table"));
        tables.Add(typeof(ComposeData), new DataTable<ComposeData>("DataTables/Compose_Table"));
        tables.Add(typeof(DescData), new DataTable<DescData>("DataTables/Desc_Table"));
        tables.Add(typeof(DGData), new DataTable<DGData>("DataTables/DG_Table"));
        tables.Add(typeof(DialogueData), new DataTable<DialogueData>("DataTables/Dialogue_Table"));
        tables.Add(typeof(DrawData), new DataTable<DrawData>("DataTables/Draw_Table"));
        tables.Add(typeof(SkillDrawData), new DataTable<SkillDrawData>("DataTables/SkillDraw_Table"));
        tables.Add(typeof(DropData), new DataTable<DropData>("DataTables/Drop_Table"));
        tables.Add(typeof(MenuData), new DataTable<MenuData>("DataTables/Menu_Table"));
        tables.Add(typeof(MonsterData), new DataTable<MonsterData>("DataTables/Monster_Table"));
        tables.Add(typeof(NameData), new DataTable<NameData>("DataTables/Name_Table"));
        tables.Add(typeof(PopUpData), new DataTable<PopUpData>("DataTables/Pop_up_Table"));
        tables.Add(typeof(RegionData), new DataTable<RegionData>("DataTables/Region_Table"));
        tables.Add(typeof(ReinforceData), new DataTable<ReinforceData>("DataTables/Reinforce_Table"));
        tables.Add(typeof(StoryData), new DataTable<StoryData>("DataTables/Story_Table"));
        tables.Add(typeof(TutorialData), new DataTable<TutorialData>("DataTables/Tutorial_Table"));
        tables.Add(typeof(MiniMapData), new DataTable<MiniMapData>("DataTables/MiniMapTable"));

        isLoaded = true;
    }

    public static DataTable<T> Load<T>(DataTable<T> table, string filepath) where T : ICSVParsing, new()
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