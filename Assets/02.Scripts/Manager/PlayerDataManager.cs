using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public int playerCurrHp;
    public int playerCurrMp;
    public string lastSaveMapId;
    public string lastSaveChapterName;
    public Vector3 lastPlayerPos;
    public string currskill1;
    public string currskill2;


    public void SaveFile()
    {
        var saveData = new SavePlayerDataVer1();
        saveData.playerName = playerName;
        saveData.playerCurrHp = playerCurrHp;
        saveData.lastMapId = lastSaveMapId;
        saveData.skill1 = currskill1;
        saveData.skill2 = currskill2;  

        saveData.lastChapter= lastSaveChapterName;
        //saveData.lastPlayerPos = player.transform.position;
        saveData.lastPlayerPos = lastPlayerPos;

        SaveLoadSystem.Save(saveData);
    }

    public void LoadFile()
    {
        var saveData = SaveLoadSystem.Load(SaveData.Types.Player) as SavePlayerDataVer1;
        playerName = saveData.playerName;
        playerCurrHp = saveData.playerCurrHp;
        lastSaveMapId = saveData.lastMapId;
        lastSaveChapterName = saveData.lastChapter;
        lastPlayerPos = saveData.lastPlayerPos;
        currskill1 = saveData.skill1;
        currskill2 = saveData.skill2;
    }

    public void SaveLastPos(string mapId,string chapter, Vector3 pos)
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
        var playerStatus = GameManager.instance.player.GetComponent<Status>();
        playerCurrHp = playerStatus.currHp;
        playerCurrMp = playerStatus.currMp;
        var playerSkills = GameManager.instance.player.GetComponent<PlayerSkills>();
        currskill1 = playerSkills.GetCurrSkillID(0);
        currskill2 = playerSkills.GetCurrSkillID(1);
    }

    public void SetPlayerHpMp()
    {
        SetPlayerHpMp(playerCurrHp, playerCurrMp);
    }

    public void SetPlayerHpMp(int hp, int mp)
    {
        var playerStatus = GameManager.instance.player.GetComponent<Status>();
        playerStatus.currHp = hp;
        playerStatus.currMp = mp;

    }

    public void FillPlayerHpMp()
    {
        var playerStatus = GameManager.instance.player.GetComponent<Status>();
        playerStatus.currHp = playerStatus.FinalValue.maxHp;
        playerStatus.currMp = playerStatus.FinalValue.maxMp;

    }
}
