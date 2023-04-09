using System;
using System.Collections;
using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
using System.Linq;
using System.Text;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

    public GameObject uiDungeonSelect;

    private float time;
    private TextMeshProUGUI text;

    private DataTable<DungeonTable> dungeonTable;

    private string today;
    private string attempt;
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
    private Canvas limitwarning;

    [SerializeField]
    private Canvas refillsus;

    [SerializeField]
    private Canvas refillfail;

    [SerializeField]
    private Canvas exitButton;

    [SerializeField]
    private Canvas pannel;

    [SerializeField]
    private Canvas homeButton;

    [SerializeField]
    private Canvas popUp;

    [SerializeField]
    private Canvas remaningtime;

    private List<GameObject> enemies;
    public List<GameObject> Enemies { get { return enemies; } set { enemies = value; } }
    private int todayPlayCount;
    private bool isDungeon = false;

    public Dictionary<string, int> unlock;
    private Status playerStatus;

    private PlayerInventory inven;
    StringBuilder scenename = new StringBuilder();

    private bool weekend;
    void OnEnable()
    {


        StartCoroutine(COnEnable());
    }

    private IEnumerator COnEnable()
    {
        yield return null;
        if (m_instance == null)
        {
            SelectedLevel = new StringBuilder();

            DontDestroyOnLoad(gameObject);
            text = transform.Find("RemainingTime").transform.GetComponentInChildren<TextMeshProUGUI>();
            if (inven == null)
            {
                inven = GameManager.instance.player.GetComponent<PlayerInventory>();
            }

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
        saveData.playedtime = attempt;
        saveData.playedday = DateTime.Now.ToString();
        if (DateTime.Now.DayOfWeek == DayOfWeek.Sunday || DateTime.Now.DayOfWeek == DayOfWeek.Saturday)
        {
            saveData.weekend = DateTime.Now.DayOfWeek.ToString();
        }
        else
            saveData.weekend = "";
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
        Debug.Log($"Save lv is {lv} and attemp is {attempt}");
        SaveLoadSystem.Save(saveData);
    }

    private void ExitedDungeon(Scene scene, LoadSceneMode mode)
    {

        if (scene.name == "Scene02")
        {
            remaningtime.gameObject.SetActive(false);
            // PlayerDataManager.instance.LoadPlayerHpMp();
            PlayerDataManager.instance.LoadFile();
            GameManager.instance.player.GetComponent<Status>().Restore();

            // PlayerDataManager.instance.MoveToLastPos(GameManager.instance.player);
        }

    }
    private void LoadFile()
    {
        var saveData = SaveLoadSystem.Load(SaveData.Types.Dungeon) as SaveDugeonDataVer1;
        if (saveData == null || saveData.weekend == null)
        {
            var newsaveData = new SaveDugeonDataVer1();
            newsaveData.mondaylv = "1";
            newsaveData.tuesdaylv = "1";
            newsaveData.wednesdaylv = "1";
            newsaveData.thursdaylv = "1";
            newsaveData.fridaylv = "1";
            newsaveData.playedday = DateTime.Now.ToString();
            newsaveData.playedtime = "0";
            newsaveData.weekend = "";
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


        if ((DateTime.Now - DateTime.Parse(saveData.playedday)).Milliseconds > 0 && DateTime.Now.DayOfWeek != DateTime.Parse(saveData.playedday).DayOfWeek)
        {
            if (!weekend && saveData.weekend != DateTime.Now.DayOfWeek.ToString())
                saveData.playedtime = "0";
            if (DateTime.Now.DayOfWeek == DayOfWeek.Sunday || DateTime.Now.DayOfWeek == DayOfWeek.Saturday)
                weekend = true;
        }
        attempt = saveData.playedtime;

    }

    public void PlayedTimeResetCheck()
    {

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
        Debug.Log($"lv is {lv}");
        for (int i = 0; i < lv; i++)
        {
            StringBuilder levs = new StringBuilder();
            levs.Append("Level");
            levs.Append((i + 1).ToString());

            dungeonLevel.transform.Find("Horizontal").transform.Find("Level").transform.Find(levs.ToString()).GetComponent<Button>().interactable = true;
            dungeonLevel.transform.Find("Horizontal").transform.Find("Level").transform.Find(levs.ToString()).transform.Find("Image").gameObject.SetActive(false);

        }

    }

    private void Update()
    {
        //Debug.Log(Time.timeScale);

        if (isDungeon && enemies != null)
        {
            // Time.timeScale = 1;
            Result.gameObject.SetActive(false);
            Result.transform.Find("Win").gameObject.SetActive(false);
            Result.transform.Find("Lose").gameObject.SetActive(false);
            if (time > 0)
            {
                text.text = ((int)(time -= Time.deltaTime)).ToString();
            }
            if (time <= 0 || playerStatus.CurrHp <= 0)
            {
                // Time.timeScale = 0;
                Result.transform.Find("Lose").transform.Find("Retry").gameObject.GetComponentInChildren<TextMeshProUGUI>().text = $"({attempt}/3 retry)";
                GameManager.instance.player.gameObject.SetActive(false);
                Result.gameObject.SetActive(true);
                Result.transform.Find("Lose").gameObject.SetActive(true);
                //Result.transform.Find("Lose").transform.Find("PlayedTime").GetComponentInChildren<TextMeshProUGUI>().text = ((int)((dungeonTable.Get(SelectedLevel.ToString()).countdown - time))).ToString();

                isDungeon = false;

            }

            foreach (var enemy in enemies)
            {
                if (enemy.GetComponent<Enemy>().GetIsLive())
                {
                    break;
                }
                if (enemy.Equals(enemies.Last()))
                {
                    SetReward();

                    Result.transform.Find("Win").transform.Find("Retry").gameObject.GetComponentInChildren<TextMeshProUGUI>().text = $"({attempt}/3 retry)";

                    //  Time.timeScale = 0;
                    Result.gameObject.SetActive(true);
                    Result.transform.Find("Win").gameObject.SetActive(true);
                    //Result.transform.Find("Win").transform.Find("PlayedTime").GetComponentInChildren<TextMeshProUGUI>().text = ((int)((dungeonTable.Get(SelectedLevel.ToString()).countdown - time))).ToString();
                    //Result.transform.Find("Win").transform.Find("Reward").transform.Find("RewardCount").GetComponentInChildren<TextMeshProUGUI>().text = dungeonTable.Get(SelectedLevel.ToString()).itemcount.ToString();
                    // Result.transform.Find("Win").transform.Find("Reward").GetComponentInChildren<UnityEngine.UI.Image>().sprite=
                    if (lv == instance.dungeonTable.Get(instance.SelectedLevel.ToString()).level)
                        ++lv;
                    SaveFile();


                    PlayerDataManager.instance.SaveInventory();
                    PlayerDataManager.instance.SaveLevel();
                    PlayerDataManager.instance.SaveSkills();
                    isDungeon = false;
                }

            }
        }

    }

    private void SetReward()
    {

        var rewardUi = Result.transform.Find("Win").Find("Reward").Find("RewardImages");


        List<GameObject> rewardUiList = new List<GameObject>();
        string rewardId = scenename.ToString();
        var rewardTable = DataTableMgr.GetTable<RewardData>();
        var powder = rewardTable.Get(rewardId.ToString()).powder;
        Debug.Log($"Reward Powder is {powder}");
        var essnece = rewardTable.Get(rewardId.ToString()).essence;
        var skillpiece = rewardTable.Get(rewardId.ToString()).skill_piece;
        var equipePiece = rewardTable.Get(rewardId.ToString()).equipe_piece;
        var exp = rewardTable.Get(rewardId.ToString()).exp;

        var secRewardID = rewardId + "S";
        var powderSec = rewardTable.Get(secRewardID.ToString()).powder;
        var essneceSec = rewardTable.Get(secRewardID.ToString()).essence;
        var skillpieceSec = rewardTable.Get(secRewardID.ToString()).skill_piece;
        var equipePieceSec = rewardTable.Get(secRewardID.ToString()).equipe_piece;
        var expSec = rewardTable.Get(secRewardID.ToString()).exp;


        for (int i = 0; i < rewardUi.transform.childCount; i++)
        {
            rewardUiList.Add(rewardUi.transform.GetChild(i).gameObject);
        }

        if (lv == instance.dungeonTable.Get(instance.SelectedLevel.ToString()).level)
        {
            //poweder set
            rewardUiList[0].transform.Find("Count").GetComponent<TextMeshProUGUI>().text = powder.ToString();
            GameManager.instance.player.GetComponent<PlayerInventory>().AddConsumable("40003", powder);
            Debug.Log(powder);
            rewardUiList[1].transform.Find("Count").GetComponent<TextMeshProUGUI>().text = essnece.ToString();
            GameManager.instance.player.GetComponent<PlayerInventory>().AddConsumable("40004", essnece);

            rewardUiList[2].transform.Find("Count").GetComponent<TextMeshProUGUI>().text = skillpiece.ToString();

            rewardUiList[3].transform.Find("Count").GetComponent<TextMeshProUGUI>().text = equipePiece.ToString();

            rewardUiList[4].transform.Find("Count").GetComponent<TextMeshProUGUI>().text = exp.ToString();
            GameManager.instance.player.GetComponent<PlayerLevelManager>().CurrExp += exp;
        }
        else
        {
            //poweder set
            rewardUiList[0].transform.Find("Count").GetComponent<TextMeshProUGUI>().text = powderSec.ToString();
            GameManager.instance.player.GetComponent<PlayerInventory>().AddConsumable("40003", powderSec);

            rewardUiList[1].transform.Find("Count").GetComponent<TextMeshProUGUI>().text = essneceSec.ToString();
            GameManager.instance.player.GetComponent<PlayerInventory>().AddConsumable("40004", essneceSec);

            rewardUiList[2].transform.Find("Count").GetComponent<TextMeshProUGUI>().text = skillpieceSec.ToString();

            rewardUiList[3].transform.Find("Count").GetComponent<TextMeshProUGUI>().text = equipePieceSec.ToString();

            rewardUiList[4].transform.Find("Count").GetComponent<TextMeshProUGUI>().text = expSec.ToString();
            GameManager.instance.player.GetComponent<PlayerLevelManager>().CurrExp += expSec;

        }

        

    }

    public void HomeOpen()
    {
        pannel.gameObject.SetActive(true);
        pannel.transform.Find("Retry").gameObject.GetComponentInChildren<TextMeshProUGUI>().text = $"({attempt}/3 retry)";

        Time.timeScale = 0;

    }

    public void CloseUi()
    {

        if (dungeonLevel.gameObject.activeSelf)
        {

            dungeonLevel.gameObject.SetActive(false);
            dungeonDay.gameObject.SetActive(true);
        }
        else if (dungeonDay.gameObject.activeSelf)
        {
            dungeonDay.gameObject.SetActive(false);
            exitButton.gameObject.SetActive(false);
        }

    }

    public void Popup()
    {
        popUp.gameObject.SetActive(true);
    }

    public void Countinue()
    {
        pannel.gameObject.SetActive(false);
        popUp.gameObject.SetActive(false);
        Time.timeScale = 1;
    }
    public void ExitDungeon()
    {

        isDungeon = false;
        result.gameObject.SetActive(false);
        dungeonDay.gameObject.SetActive(false);
        dungeonLevel.gameObject.SetActive(false);
        Time.timeScale = 1;
        pannel.gameObject.SetActive(false);
        popUp.gameObject.SetActive(false);
        homeButton.gameObject.SetActive(false);
        // SceneManager.LoadScene("Game", LoadSceneMode.Single);
        SceneManager.LoadScene("Scene02");



    }

    public void JoinDungeon()
    {
        scenename.Clear();
        int temp = Int32.Parse(attempt);

        if (temp >= 3)
        {
            //garu popup
            limitwarning.gameObject.SetActive(true);

            return;
        }
        Debug.Log($"attempt is {attempt}");
        attempt = (++temp).ToString();
        SaveFile();
        isDungeon = true;
        exitButton.gameObject.SetActive(false);
        Result.gameObject.SetActive(false);
        Result.transform.Find("Win").gameObject.SetActive(false);
        Result.transform.Find("Lose").gameObject.SetActive(false);
        dungeonLevel.gameObject.SetActive(false);
        scenename.Append(instance.dungeonTable.Get(instance.SelectedLevel.ToString()).week);
        scenename.Append("_");
        scenename.Append(instance.dungeonTable.Get(instance.SelectedLevel.ToString()).level);
        PlayerDataManager.instance.SaveFile();

        SceneManager.LoadScene("Game", LoadSceneMode.Single);
        Debug.Log(scenename);
        SceneManager.LoadScene(scenename.ToString(), LoadSceneMode.Additive);


        remaningtime.gameObject.SetActive(true);
        time = dungeonTable.Get(SelectedLevel.ToString()).countdown;
        StartCoroutine(SetEnemy());

    }

    IEnumerator SetEnemy()
    {
        yield return null;
        yield return null;

        enemies = GameObject.FindGameObjectsWithTag("Enemy").ToList();
        //  Debug.Log("enemy");
        isDungeon = true;
        playerStatus = GameManager.instance.player.GetComponent<Status>();
        var player = GameManager.instance.player;
        player.GetComponent<PlayerInventory>().RefillPotions();
        //Debug.Log("PlayerInventory");

        //player.GetComponent<PlayerSkills>().SetEmpty();
        //Debug.Log("empty");

        player.GetComponent<PlayerSkills>().SetSkill(0, PlayerDataManager.instance.currskill1);
        //Debug.Log("SetSkill0");

        player.GetComponent<PlayerSkills>().SetSkill(1, PlayerDataManager.instance.currskill2);
        // Debug.Log("SetSkill1");

        homeButton.gameObject.SetActive(true);

        GameManager.instance.player.transform.position = GameObject.FindGameObjectWithTag("StartPoint").transform.position;
        //   Debug.Log("startpoint");
        PlayerDataManager.instance.LoadPlayer();
        PlayerDataManager.instance.LoadInventory();
        PlayerDataManager.instance.LoadSkills();
        player.GetComponent<Status>().Restore();
    }

    public void RefillAttempt()
    {
        limitwarning.gameObject.SetActive(false);

        if (inven.GetConsumableCount("1") < 3000)
        {
            refillfail.gameObject.SetActive(true);
            StopCoroutine(GaruRefillResultturnoff());
            StartCoroutine(GaruRefillResultturnoff());
            return;
        }
        inven.UseConsumable("1", 3000);
        refillsus.gameObject.SetActive(true);
        attempt = "0";
        SaveFile();
        StopCoroutine(GaruRefillResultturnoff());
        StartCoroutine(GaruRefillResultturnoff());

    }

    IEnumerator GaruRefillResultturnoff()
    {
        yield return new WaitForSeconds(2.0f);
        refillfail.gameObject.SetActive(false);
        refillsus.gameObject.SetActive(false);

    }


    public void Restart()
    {
        int temp = Int32.Parse(attempt);

        if (temp >= 3)
        {
            //garu popup
            limitwarning.gameObject.SetActive(true);
            return;
        }

        attempt = (++temp).ToString();
        SaveFile();



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

        SceneManager.LoadScene("Game", LoadSceneMode.Single);
        SceneManager.LoadScene(scenename.ToString(), LoadSceneMode.Additive);

        remaningtime.gameObject.SetActive(true);
        pannel.gameObject.SetActive(false);
        time = dungeonTable.Get(SelectedLevel.ToString()).countdown;

        // Debug.Log("loaded");
        StartCoroutine(SetEnemy());
        //foreach (var enemy in enemies)
        //{
        //    enemy.gameObject.SetActive(true);
        //}


    }


}