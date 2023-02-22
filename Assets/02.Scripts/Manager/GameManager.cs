using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager m_instance;

    public static GameManager instance
    {
        get
        {
            if (m_instance == null)
                m_instance = FindObjectOfType<GameManager>();
            return m_instance;
        }
    }

    private void Awake()
    {
        if (instance != this)
            Destroy(gameObject);
    }

    //player respawn func
    public void Respawn()
    {
        var player = GameObject.FindWithTag("Player");
        GameObject.Find(MapManager.instance.GetCurrentMapName()).SetActive(false);
        GameObject.FindWithTag("Map").transform.Find(PlayerDataManager.instance.lastMapId).gameObject.SetActive(true);           

        MapManager.instance.SetCurrentMapName(PlayerDataManager.instance.lastMapId);
        player.transform.position = PlayerDataManager.instance.lastPlayerPos;
        //Todo: Fill player HP
        //Todo: Fill player MP
        //
        // StartCoroutine(CoRespawn());
    }

    IEnumerator CoRespawn()
    {
        yield return null;
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) { 
            player.transform.position = PlayerDataManager.instance.lastPlayerPos;
           
        }

    }
}
