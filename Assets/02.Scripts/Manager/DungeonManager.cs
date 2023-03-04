using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public StringBuilder SelectedLevel { get; set; }
    public DataTable<DungeonTable> DungeonTable { get { return dungeonTable; } }

    private string dungeonname;
    [SerializeField]
    private Canvas dungeonLevel;
    [SerializeField]
    private Canvas dungeonDay;
    private List<GameObject> enemies;
    private int todayPlayCount;
    private bool isDungeon = false;


    void OnEnable()
    {
        if (m_instance == null)
        {
            SelectedLevel = new StringBuilder();

            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);


    }

    public void SelectDungeonDay(string path)
    {
        dungeonTable = DataTableMgr.Load(dungeonTable, path);
        dungeonDay.gameObject.SetActive(false);
        dungeonLevel.gameObject.SetActive(true);
        SetLevelUi();
    }

    private void Update()
    {
        if (isDungeon)
        {
            foreach (var enemy in enemies)
            {
                if (enemy.gameObject.activeSelf)
                {
                    break;
                }
                if (enemy.Equals(enemies.Last()))
                {
                    Time.timeScale = 0;
                    var winUI = GameObject.Find("Result").transform.Find("Win");

                    winUI.gameObject.SetActive(true);
                    isDungeon = false;
                }

            }
        }

        if (!isDungeon && Input.GetKeyDown(KeyCode.Escape))
        {
            if (dungeonLevel.gameObject.activeSelf)
            {

                dungeonLevel.gameObject.SetActive(false);
                dungeonDay.gameObject.SetActive(true);
            }
            else if (dungeonDay.gameObject.activeSelf)
            {
                dungeonDay.gameObject.SetActive(false);
            }


        }


    }

    public void ExitDungeon()
    {
        isDungeon = false;
        SceneManager.LoadScene("ParkMWTest");
        dungeonDay.gameObject.SetActive(true);
        dungeonLevel.gameObject.SetActive(false);
        GameObject.Find("DungeonManager").transform.Find("RemainingTime").gameObject.SetActive(false);
    }

    public void JoinDungeon()
    {
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
            GameObject.Find("Result").transform.Find("Win").gameObject.SetActive(false);
            GameObject.Find("Result").transform.Find("Lose").gameObject.SetActive(false);

        }
        StringBuilder scenename = new StringBuilder();

        scenename.Append(instance.dungeonTable.Get(instance.SelectedLevel.ToString()).week);
        scenename.Append("_");
        scenename.Append(instance.dungeonTable.Get(instance.SelectedLevel.ToString()).level);


        SceneManager.LoadScene(scenename.ToString());

        GameObject.Find("DungeonManager").transform.Find("RemainingTime").gameObject.SetActive(true);
        GameObject.Find("RemainingTime").GetComponentInChildren<FlowTime>().SetTime(dungeonTable.Get(SelectedLevel.ToString()).countdown);
        StartCoroutine(SetEnemy());

    }

    IEnumerator SetEnemy()
    {
        yield return null;
        enemies = GameObject.FindGameObjectsWithTag("Enemy").ToList();
        isDungeon = true;
        Debug.Log("steed");
    }

    private void SetLevelUi()
    {
        //search how many unlock level and clickable each level
    }


}
