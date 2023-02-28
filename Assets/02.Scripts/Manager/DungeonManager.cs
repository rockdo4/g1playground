using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    // Start is called before the first frame update

    private DataTable<DungeonTable> dungeonTable;
      

    void Start()
    {
       

    }


    // Update is called once per frame
    void Update()
    {

    }

    public void EnterDungeon(string path)
    {
        DataTableMgr.Load(dungeonTable, path);
        
    }
}
