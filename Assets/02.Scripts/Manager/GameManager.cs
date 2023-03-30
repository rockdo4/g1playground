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
    public UI ui;
    public GamblingManager gambling;
    public AttackColliderManager attackColliderManager;
    public EnemyManager enemyManager;


    private void Awake()
    {
        if (instance != this)
            Destroy(gameObject);
        player.GetComponent<DestructedEvent>().OnDestroyEvent = RestartGame;
    }

    public void Respawn()
    {
        if (SceneManager.GetActiveScene().name != "Scene02"|| PlayerDataManager.instance.lastSaveChapterName==null)
            return;

        player.GetComponent<PlayerInventory>().RefillPotions();
        player.transform.SetParent(null);
        player.GetComponent<PlayerController>().autoToggle.isOn = false;

        GameObject.FindWithTag("Map").transform.Find(MapManager.instance.GetCurrentChapterName()).Find(MapManager.instance.GetCurrentMapName()).gameObject.GetComponent<StageController>().PortalOpen();

        //GameObject.FindWithTag("Map").transform.Find(MapManager.instance.GetCurrentChapterName()).Find(MapManager.instance.GetCurrentMapName()).gameObject.GetComponent<StageController>().ResetObject();
        MapManager.instance.GetCurrentStageObject().gameObject.SetActive(false);

        GameObject.FindWithTag("Map").transform.Find(PlayerDataManager.instance.lastSaveChapterName).Find(PlayerDataManager.instance.lastSaveMapId).gameObject.SetActive(true);

        //change current state
        player.transform.position = PlayerDataManager.instance.lastPlayerPos;
        Camera.main.transform.position = player.transform.position;
        MapManager.instance.SetCurrentMapName(PlayerDataManager.instance.lastSaveMapId);
        MapManager.instance.SetcurrentChapterName(PlayerDataManager.instance.lastSaveChapterName);

        //respawn state
        MapManager.instance.GetCurrentStageObject().gameObject.GetComponent<StageController>().PortalClose();
        MapManager.instance.GetCurrentStageObject().gameObject.GetComponent<StageController>().EnemiesReset();
        player.GetComponent<Status>().Restore();
        player.GetComponent<AttackedCC>().Reset();
        player.GetComponent<PlayerSkills>().Reset();
        attackColliderManager.ReleaseAll();
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
