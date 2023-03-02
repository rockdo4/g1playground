using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    // Start is called before the first frame update
    private static DungeonManager m_instance;
    public static DungeonManager instance
    {
        get
        {
            if (m_instance == null)
                m_instance = FindObjectOfType<DungeonManager>();
            return m_instance;
        }
    }

    private DataTable<DungeonTable> dungeonTable;
    public DataTable<DungeonTable> DungeonTable { get { return dungeonTable; }}
    private string dungeonname;
    [SerializeField]
    private Canvas DungeonLevel;
    [SerializeField]
    private Canvas DungeonDay;

    private int todayPlayCount;

    void Start()
    { 
      

    }


    // Update is called once per frame
    void Update()
    {

    }

    public void SelectDungeonDay(string path)
    {
        dungeonTable = DataTableMgr.Load(dungeonTable, path);
        DungeonDay.gameObject.SetActive(false);
        DungeonLevel.gameObject.SetActive(true);
        SetLevelUi();
    }

    public void JoinDungeon()
    {

        //write the Dungeon scene name on a table or should I Attach Day and Level names and then change to the attached name scene?



    }

    private void SetLevelUi()
    {
        //search how many unlock level and clickable each level
    }

   
}
