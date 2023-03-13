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

    public GameObject player;
    public EffectManager effectManager;
    public UIManager uiManager;
    public AttackColliderManager attackColliderManager;

    private void Awake()
    {
        if (instance != this)
            Destroy(gameObject);
        player.GetComponent<DestructedEvent>().OnDestroyEvent = RestartGame;
    }

    public void Respawn()
    {
        GameObject.FindWithTag("Map").transform.Find(MapManager.instance.GetCurrentMapName()).gameObject.GetComponent<StageController>().PortalOpen();   
                       
        
        GameObject.FindWithTag("Map").transform.Find(PlayerDataManager.instance.lastMapId).gameObject.SetActive(true);
        player.transform.position = PlayerDataManager.instance.lastPlayerPos;
        Camera.main.transform.position = GameManager.instance.player.transform.position;
        MapManager.instance.SetCurrentMapName(PlayerDataManager.instance.lastMapId);
        GameObject.FindWithTag("Map").transform.Find(MapManager.instance.GetCurrentMapName()).gameObject.GetComponent<StageController>().PortalClose();

        PlayerDataManager.instance.FillPlayerHpMp();
        MapManager.instance.SetLastCheckpointMapTurnOn();
       
        //
        // StartCoroutine(CoRespawn());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Respawn();
        }
    }

    IEnumerator CoRespawn()
    {
        yield return null;
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.transform.position = PlayerDataManager.instance.lastPlayerPos;

        }
    }

    public void RestartGame() => SceneManager.LoadScene("Scene01");
}
