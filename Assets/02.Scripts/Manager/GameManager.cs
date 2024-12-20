using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
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
    public GameObject followCamera;

    private void Awake()
    {
        if (instance != this)
            Destroy(gameObject);
        if (string.Equals(SceneManager.GetActiveScene().name, "Tutorial"))
            player.GetComponent<DestructedEvent>().OnDestroyEvent = player.GetComponent<Status>().Restore;
    }

    public void Respawn()
    {
        if (SceneManager.GetActiveScene().name != "Scene02" || PlayerDataManager.instance.lastSaveChapterName == null)
            return;

        if (PlayerDataManager.instance.lastSaveChapterName == null || PlayerDataManager.instance.lastSaveMapId == null)
            return;

        var playerController = player.GetComponent<PlayerController>();
        NavMeshAgent agent = player.GetComponent<NavMeshAgent>();
        var linkMover = player.GetComponent<AgentLinkMover>();
        playerController.autoToggle.isOn = false;
        playerController.AgentOff();
        playerController.AgentOnOff();
        player.GetComponent<PlayerInventory>().RefillPotions();
        player.transform.SetParent(null);

        if (agent.isOnOffMeshLink)
        {
            agent.ResetPath();
            linkMover.enabled = false;
            linkMover.enabled = true;
        }
        else if (agent.isOnNavMesh && !agent.currentOffMeshLinkData.valid)
        {
            agent.ResetPath();
        }

        GameObject.FindWithTag("Map").transform.Find(MapManager.instance.GetCurrentChapterName()).Find(MapManager.instance.GetCurrentMapName()).gameObject.GetComponent<StageController>().PortalOpen();

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
        attackColliderManager.ReleaseAll();
        player.GetComponent<PlayerSkills>().Reset();
        MapManager.instance.SetLastCheckpointMapTurnOn();

        player.SetActive(false);
        player.SetActive(true);
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

    public void RestartGame() => SceneManager.LoadScene("Scene02");
}
