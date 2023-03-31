using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;
using static PlayerInventory;

public class PlayerDataManager : MonoBehaviour
{
    private static PlayerDataManager m_instance;
    public static PlayerDataManager instance
    {
        get
        {
            if (m_instance == null)
                m_instance = FindObjectOfType<PlayerDataManager>();
            return m_instance;
        }
    }

    private void Awake()
    {
        if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    public string playerName;
    public string lastSaveMapId;
    public string lastSaveChapterName;
    public Vector3 lastPlayerPos;

    public int playerCurrHp;
    public int playerCurrMp;

    public List<string> weapons;
    public List<string> armors;
    public List<Consumable> consumables;
    public string currWeapon;
    public string currArmor;

    public List<string> possessedSkills;
    public int currskill1;
    public int currskill2;

    public int currLevel;
    public int currExp;

    public bool endTutorial = false;


    public void Start()
    {
        LoadFile();
    }

    public void SaveFile()
    {
        var PlayerStatus = GameManager.instance.player.GetComponent<PlayerStatus>();
        var saveData = new SavePlayerDataVer1();
        saveData.playerName = playerName;
        saveData.lastMapId = lastSaveMapId;
        saveData.lastChapter = lastSaveChapterName;
        //saveData.lastPlayerPos = player.transform.position;
        saveData.lastPlayerPos = lastPlayerPos;

        saveData.playerCurrHp = PlayerStatus.CurrHp;
        saveData.playerCurrMp = PlayerStatus.CurrMp;

        saveData.weapons = weapons;
        saveData.armors = armors;
        saveData.consumables = consumables;
        saveData.currWeapon = currWeapon;
        saveData.currArmor = currArmor;

        saveData.possessedSkills = possessedSkills;
        saveData.currskill1 = currskill1;
        saveData.currskill2 = currskill2;

        saveData.currLevel = GameManager.instance.player.GetComponent<PlayerLevelManager>().Level;
        GameManager.instance.uiManager.PlayerLevel(saveData.currLevel);
        saveData.currExp = GameManager.instance.player.GetComponent<PlayerLevelManager>().CurrExp;

        SaveLoadSystem.Save(saveData);
    }

    public void LoadFile()
    {
        var saveData = SaveLoadSystem.Load(SaveData.Types.Player) as SavePlayerDataVer1;
        if (saveData == null)
        {
            playerName = null;
            lastSaveMapId = null;
            lastSaveChapterName = null;
            lastPlayerPos = Vector3.zero;

            GameManager.instance.player.GetComponent<PlayerInventory>().SetDefault();
            GameManager.instance.player.GetComponent<PlayerSkills>().SetDefault();
            GameManager.instance.player.GetComponent<PlayerLevelManager>().SetDefault();
            GameManager.instance.uiManager.PlayerLevel(1);

            return;
        }
        playerName = saveData.playerName;
        lastSaveMapId = saveData.lastMapId;
        lastSaveChapterName = saveData.lastChapter;
        lastPlayerPos = saveData.lastPlayerPos;

        weapons = saveData.weapons;
        armors = saveData.armors;
        consumables = saveData.consumables;
        currWeapon = saveData.currWeapon;
        currArmor = saveData.currArmor;

        possessedSkills = saveData.possessedSkills;
        currskill1 = saveData.currskill1;
        currskill2 = saveData.currskill2;

        currLevel = saveData.currLevel;
        GameManager.instance.uiManager.PlayerLevel(saveData.currLevel);
        currExp = saveData.currExp;

        playerCurrHp = saveData.playerCurrHp;
        playerCurrMp = saveData.playerCurrMp;

        LoadPlayer();
    }

    public void SaveLastPos(string mapId, string chapter, Vector3 pos)
    {
        //lastChapter = chapter;
        lastSaveMapId = mapId;
        lastPlayerPos = pos;
        lastSaveChapterName = chapter;
    }

    public void MoveToLastPos(GameObject go)
    {
        //if (lastMapId == null)
        //{
        //    LoadFile();
        //}
        MapManager.instance.SetLastCheckpointMapTurnOn();
        go.transform.position = lastPlayerPos;

    }

    public void SavePlayer()
    {
        SaveLevel();   //
        SaveInventory(); //
        SaveSkills();//
        SavePlayerHpMp();
    }

    public void LoadPlayer()
    {
        LoadLevel();
        LoadInventory();
        LoadSkills();
        LoadPlayerHpMp();
    }

    public void SavePlayerHpMp()
    {
        var playerStatus = GameManager.instance.player.GetComponent<PlayerStatus>();
        playerCurrHp = playerStatus.CurrHp;
        playerCurrMp = playerStatus.CurrMp;
    }

    public void LoadPlayerHpMp()
    {
        var playerStatus = GameManager.instance.player.GetComponent<PlayerStatus>();
        playerStatus.CurrHp = playerCurrHp;
        playerStatus.CurrMp = playerCurrMp;
    }

    public void SaveInventory()
    {
        var playerInventory = GameManager.instance.player.GetComponent<PlayerInventory>();
        weapons = playerInventory.Weapons;
        armors = playerInventory.Armors;
        consumables = playerInventory.Consumables;
        currWeapon = playerInventory.CurrWeapon;
        currArmor = playerInventory.CurrArmor;
    }

    public void SaveDungeonProgress()
    {
        List<StageController> maps = MapManager.instance.GetStageList();


    }

    public void LoadInventory() => GameManager.instance.player.GetComponent<PlayerInventory>().Load(weapons, armors, consumables, currWeapon, currArmor);

    public void SaveSkills()
    {
        var playerSkills = GameManager.instance.player.GetComponent<PlayerSkills>();
        possessedSkills = playerSkills.PossessedSkills;
        currskill1 = playerSkills.GetCurrSkillIndex(0);
        currskill2 = playerSkills.GetCurrSkillIndex(1);
    }

    public void LoadSkills() => GameManager.instance.player.GetComponent<PlayerSkills>().Load(possessedSkills, currskill1, currskill2);

    public void SaveLevel()
    {
        var playerLevel = GameManager.instance.player.GetComponent<PlayerLevelManager>();
        currLevel = playerLevel.Level;
        currExp = playerLevel.CurrExp;
    }

    public void LoadLevel() => GameManager.instance.player.GetComponent<PlayerLevelManager>().Load(currLevel, currExp);
}