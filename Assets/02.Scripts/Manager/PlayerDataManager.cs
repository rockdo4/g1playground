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
    public int lastMapId;

    public void Save()
    {
        var saveData = new SaveDataVer1();
        saveData.playerName = playerName;
        saveData.playerCurrHp = playerCurrHp;
        saveData.lastMapId = lastMapId;
        //saveData.lastPlayerPos = player.transform.position;

        SaveLoadSystem.Save(SaveLoadSystem.Types.Player, saveData);
    }

    public void Load()
    {
        var saveData = SaveLoadSystem.Load(SaveLoadSystem.Types.Player) as SaveDataVer1;
        playerName = saveData.playerName;
        playerCurrHp = saveData.playerCurrHp;
        lastMapId = saveData.lastMapId;
    }
}
