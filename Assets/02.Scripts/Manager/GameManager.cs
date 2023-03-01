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

    public PlayerController playerController;
    public EffectManager effectManager;
    public ProjectileManager projectileManager;

    private void Awake()
    {
        if (instance != this)
            Destroy(gameObject);
    }

    public void Respawn()
    {
        var player = GameObject.FindWithTag("Player");
        // GameObject.Find(MapManager.instance.GetCurrentMapName()).SetActive(false);
        GameObject.FindWithTag("Map").transform.Find(PlayerDataManager.instance.lastMapId).gameObject.SetActive(true);
        player.transform.position = PlayerDataManager.instance.lastPlayerPos;
        MapManager.instance.SetCurrentMapName(PlayerDataManager.instance.lastMapId);
        //Todo: Fill player HP
        //Todo: Fill player MP
        //
        // StartCoroutine(CoRespawn());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("fuck");
            Respawn();
        }
    }

    IEnumerator CoRespawn()
    {
        yield return null;
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) { 
            player.transform.position = PlayerDataManager.instance.lastPlayerPos;
           
        }
    }

    public void RestartGame() => SceneManager.LoadScene("BuildMap");
}
