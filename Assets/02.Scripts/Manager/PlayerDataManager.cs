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

    public void SaveFile()
    {
        var saveData = new SavePlayerDataVer1();
        saveData.playerName = playerName;
        saveData.playerCurrHp = playerCurrHp;
        saveData.lastMapId = lastSaveMapId;
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

    public void SavePlayerHpMp()
    {
        var playerStatus = GameManager.instance.player.GetComponent<Status>();
        playerCurrHp = playerStatus.currHp;
        playerCurrMp = playerStatus.currMp;

    }

    public void SetPlayerHpMp()
    {
        var playerStatus = GameManager.instance.player.GetComponent<Status>();
        playerStatus.currHp = playerCurrHp;
        playerStatus.currMp = playerCurrMp;

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
