using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PlayerDataManager.instance.playerName = "minwoo";
        PlayerDataManager.instance.playerCurrHp = 150;
        PlayerDataManager.instance.lastMapId = 15;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
            PlayerDataManager.instance.Save();
        if (Input.GetKeyDown(KeyCode.S))
            ResetData();
        if (Input.GetKeyDown(KeyCode.D))
            PlayerDataManager.instance.Load();
    }

    void ResetData()
    {
        PlayerDataManager.instance.playerName = string.Empty;
        PlayerDataManager.instance.playerCurrHp = 0;
        PlayerDataManager.instance.lastMapId = 0;
    }
}
