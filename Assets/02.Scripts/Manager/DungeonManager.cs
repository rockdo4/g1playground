using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using TMPro;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

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

    private float time;
    [SerializeField]
    private TextMeshProUGUI text;


    private DataTable<DungeonTable> dungeonTable;

    private string today;
    private int lv;
    public StringBuilder SelectedLevel { get; set; }
    public DataTable<DungeonTable> DungeonTable { get { return dungeonTable; } }

    [SerializeField]
    private Canvas dungeonLevel;
    public Canvas DungeonLevel { get { return dungeonLevel; } set { dungeonLevel = value; } }
    [SerializeField]
    private Canvas dungeonDay;
    public Canvas DungeonDay { get { return dungeonDay; } set { dungeonDay = value; } }
    [SerializeField]
    private Canvas result;
    public Canvas Result { get { return result; } set { result = value; } }


    [SerializeField]
    private Canvas remaningtime;

    private List<GameObject> enemies;
    private int todayPlayCount;
    private bool isDungeon = false;

    public Dictionary<string, int> unlock;


    void OnEnable()
    {
        if (m_instance == null)
        {
            SelectedLevel = new StringBuilder();

            DontDestroyOnLoad(gameObject);
            text = transform.Find("RemainingTime").transform.GetComponentInChildren<TextMeshProUGUI>();

            SceneManager.sceneLoaded += ExitedDungeon;
        }
        else
        {
            Destroy(gameObject);
        }


    }

    private void SaveFile()
    {
        var saveData = new SaveDugeonDataVer1();
        var origindata = SaveLoadSystem.Load(SaveData.Types.Dungeon) as SaveDugeonDataVer1;
        saveData.mondaylv = origindata.mondaylv;
        saveData.tuesdaylv = origindata.tuesdaylv;
        saveData.wednesdaylv = origindata.wednesdaylv;
        saveData.thursdaylv = origindata.thursdaylv;
        saveData.fridaylv = origindata.fridaylv;

        switch (today)
        {
            case "Mon":
                saveData.mondaylv = lv.ToString();
                break;
            case "Tue":
                saveData.tuesdaylv = lv.ToString();
                break;
            case "Wed":
                saveData.wednesdaylv = lv.ToString();
                break;
            case "Thu":
                saveData.thursdaylv = lv.ToString();
                break;
            case "Fri":
                saveData.fridaylv = lv.ToString();
                break;
        }

        SaveLoadSystem.Save(saveData);
    }

    private void ExitedDungeon(Scene scene, LoadSceneMode mode)
    {

        if (scene.name == "Map1") { 
            remaningtime.gameObject.SetActive(false);
            PlayerDataManager.instance.SetPlayerHpMp();
            PlayerDataManager.instance.MoveToLastPos(GameManager.instance.player);
        }

    }
    private void LoadFile()
    {
        var saveData = SaveLoadSystem.Load(SaveData.Types.Dungeon) as SaveDugeonDataVer1;
        if (saveData == null)
        {
            var newsaveData = new SaveDugeonDataVer1();
            newsaveData.mondaylv = "1";
            newsaveData.tuesdaylv = "1";
            newsaveData.wednesdaylv = "1";
            newsaveData.thursdaylv = "1";
            newsaveData.fridaylv = "1";
            SaveLoadSystem.Save(newsaveData);
            saveData = SaveLoadSystem.Load(SaveData.Types.Dungeon) as SaveDugeonDataVer1;
        }
        switch (today)
        {
            case "Mon":
                lv = Int32.Parse(saveData.mondaylv);
                break;
            case "Tue":
                lv = Int32.Parse(saveData.tuesdaylv);
                break;
            case "Wed":
                lv = Int32.Parse(saveData.wednesdaylv);
                break;
            case "Thu":
                lv = Int32.Parse(saveData.thursdaylv);
                break;
            case "Fri":
                lv = Int32.Parse(saveData.fridaylv);
                break;
        }

    }
    public void SelectDungeonDay(string path)
    {

        dungeonTable = DataTableMgr.Load(dungeonTable, path);
        today = dungeonTable.Get("Level1").week;
        LoadFile();
        dungeonDay.gameObject.SetActive(false);
        dungeonLevel.gameObject.SetActive(true);
        SetLevelUi();
    }
    private void SetLevelUi()
    {
        for (int i = 0; i < lv; i++)
        {
            StringBuilder levs = new StringBuilder();
            levs.Append("Level");
            levs.Append((i + 1).ToString());
            dungeonLevel.transform.Find("Level").transform.Find(levs.ToString()).GetComponent<Button>().interactable = true;
        }

    }

    private void Update()
    {

        if (isDungeon && enemies != null)
        {
            Time.timeScale = 1;
            Result.gameObject.SetActive(false);
            Result.transform.Find("Win").gameObject.SetActive(false);
            Result.transform.Find("Lose").gameObject.SetActive(false);
            if (time > 0)
            {
                text.text = ((int)(time -= Time.deltaTime)).ToString();
            }
            else if (time <= 0)
            {
                Time.timeScale = 0;
                Result.gameObject.SetActive(true);
                Result.transform.Find("Lose").gameObject.SetActive(true);
                Result.transform.Find("Lose").transform.Find("PlayedTime").GetComponentInChildren<TextMeshProUGUI>().text = ((int)((dungeonTable.Get(SelectedLevel.ToString()).countdown - time))).ToString();

                isDungeon = false;

            }

            foreach (var enemy in enemies)
            {
                if (enemy.gameObject.activeSelf)
                {
                    break;
                }
                if (enemy.Equals(enemies.Last()))
                {
                    Time.timeScale = 0;

                    Result.gameObject.SetActive(true);
                    Result.transform.Find("Win").gameObject.SetActive(true);
                    Result.transform.Find("Win").transform.Find("PlayedTime").GetComponentInChildren<TextMeshProUGUI>().text = ((int)((dungeonTable.Get(SelectedLevel.ToString()).countdown - time))).ToString();
                    Result.transform.Find("Win").transform.Find("Reward").transform.Find("RewardCount").GetComponentInChildren<TextMeshProUGUI>().text = dungeonTable.Get(SelectedLevel.ToString()).itemcount.ToString();
                   // Result.transform.Find("Win").transform.Find("Reward").GetComponentInChildren<UnityEngine.UI.Image>().sprite=
                    if (lv == instance.dungeonTable.Get(instance.SelectedLevel.ToString()).level)
                        ++lv;
                    SaveFile();
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
        result.gameObject.SetActive(false);
        dungeonDay.gameObject.SetActive(false);
        dungeonLevel.gameObject.SetActive(false);
        Time.timeScale = 1;
        SceneManager.LoadScene("Map1");

    }

    public void JoinDungeon()
    {
        isDungeon = true;

        Result.gameObject.SetActive(false);
        Result.transform.Find("Win").gameObject.SetActive(false);
        Result.transform.Find("Lose").gameObject.SetActive(false);
        StringBuilder scenename = new StringBuilder();
        dungeonLevel.gameObject.SetActive(false);
        scenename.Append(instance.dungeonTable.Get(instance.SelectedLevel.ToString()).week);
        scenename.Append("_");
        scenename.Append(instance.dungeonTable.Get(instance.SelectedLevel.ToString()).level);
        PlayerDataManager.instance.SavePlayerHpMp();       

        SceneManager.LoadScene(scenename.ToString());

        remaningtime.gameObject.SetActive(true);
        time = dungeonTable.Get(SelectedLevel.ToString()).countdown;
        StartCoroutine(SetEnemy());

    }

    IEnumerator SetEnemy()
    {
        yield return null;
        enemies = GameObject.FindGameObjectsWithTag("Enemy").ToList();
        isDungeon = true;
        yield break;
    }

    public void Restart()
    {

        Time.timeScale = 1;
        Result.transform.Find("Win").gameObject.SetActive(false);
        Result.transform.Find("Lose").gameObject.SetActive(false);
        remaningtime.gameObject.SetActive(true);
        time = dungeonTable.Get(SelectedLevel.ToString()).countdown;

        StringBuilder scenename = new StringBuilder();
        dungeonLevel.gameObject.SetActive(false);
        scenename.Append(instance.dungeonTable.Get(instance.SelectedLevel.ToString()).week);
        scenename.Append("_");
        scenename.Append(instance.dungeonTable.Get(instance.SelectedLevel.ToString()).level);
        SceneManager.LoadScene(scenename.ToString());

        remaningtime.gameObject.SetActive(true);
        time = dungeonTable.Get(SelectedLevel.ToString()).countdown;
        StartCoroutine(SetEnemy());
        //foreach (var enemy in enemies)
        //{
        //    enemy.gameObject.SetActive(true);
        //}


    }


}
