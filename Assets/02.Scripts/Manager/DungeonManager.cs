using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using TMPro;
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
    public Canvas DungeonLevel { get { return dungeonLevel; } set { dungeonLevel = value; } }
    [SerializeField]
    private Canvas dungeonDay;
    public Canvas DungeonDay { get { return dungeonDay; } set { dungeonDay = value; } }

    [SerializeField]
    private Canvas remaningtime;

    private List<GameObject> enemies;
    private int todayPlayCount;
    private bool isDungeon = false;

    private event Action scene;

    void OnEnable()
    {
        if (m_instance == null)
        {
            SelectedLevel = new StringBuilder();

            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += ExtiedDungeon;
        }
        else
        {
            Destroy(gameObject);
        }


    }

    private void ExtiedDungeon(Scene scene, LoadSceneMode mode)
    {

        if (scene.name == "Map1")
            remaningtime.gameObject.SetActive(false);

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
        if (isDungeon && enemies != null)
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
                    winUI.Find("PlayedTime").GetComponentInChildren<TextMeshProUGUI>().text = ((int)((dungeonTable.Get(SelectedLevel.ToString()).countdown - remaningtime.gameObject.GetComponent<FlowTime>().Times))).ToString();

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
        dungeonDay.gameObject.SetActive(true);
        dungeonLevel.gameObject.SetActive(false);
        SceneManager.LoadScene("Map1");

    }

    public void JoinDungeon()
    {
        isDungeon = true;
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
            GameObject.Find("Result").transform.Find("Win").gameObject.SetActive(false);
            GameObject.Find("Result").transform.Find("Lose").gameObject.SetActive(false);

        }
        StringBuilder scenename = new StringBuilder();
        dungeonLevel.gameObject.SetActive(false);
        scenename.Append(instance.dungeonTable.Get(instance.SelectedLevel.ToString()).week);
        scenename.Append("_");
        scenename.Append(instance.dungeonTable.Get(instance.SelectedLevel.ToString()).level);


        SceneManager.LoadScene(scenename.ToString());

        remaningtime.gameObject.SetActive(true);
        remaningtime.gameObject.GetComponentInChildren<FlowTime>().SetTime(dungeonTable.Get(SelectedLevel.ToString()).countdown);
        StartCoroutine(SetEnemy());

    }

    IEnumerator SetEnemy()
    {
        yield return null;
        enemies = GameObject.FindGameObjectsWithTag("Enemy").ToList();
        isDungeon = true;
    }

    private void SetLevelUi()
    {
        //search how many unlock level and clickable each level
    }


}
