using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class WorldMap : MonoBehaviour
{  
    public void GotoStage(GameObject stage)
    {
        if (stage.GetComponent<StageController>().IsClear)
        {
            SetStage(stage.GetComponent<StageController>());            
        }
        else
        {
            return;
        }
    }

    public void GoHome(GameObject stage)
    {
        var player = GameManager.instance.player;
        var playerController = player.GetComponent<PlayerController>();
        NavMeshAgent agent = player.GetComponent<NavMeshAgent>();
        var linkMover = player.GetComponent<AgentLinkMover>();

        playerController.autoToggle.isOn = false;
        playerController.AgentOff();
        playerController.AgentOnOff();
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
        player.SetActive(false);
        player.SetActive(true);
        SetStage(stage.GetComponent<StageController>());
    }

    private void SetStage(StageController stage)
    {
        if (!MapManager.instance.GetCurrentStageObject().GetComponent<StageController>().IsClear)
        {
            return;
        }
        var player = GameManager.instance.player;
        var playerController = player.GetComponent<PlayerController>();
        NavMeshAgent agent = player.GetComponent<NavMeshAgent>();
        var linkMover = player.GetComponent<AgentLinkMover>();

        playerController.autoToggle.isOn = false;
        playerController.AgentOff();
        playerController.AgentOnOff();
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
        player.SetActive(false);
        player.SetActive(true);
        GameManager.instance.player.GetComponent<PlayerInventory>().RefillPotions();
        GameManager.instance.player.transform.SetParent(null);

        MapManager.instance.GetCurrentStageObject().SetActive(false);
        //GameObject.FindWithTag("Map").transform.Find(MapManager.instance.GetCurrentChapterName()).
        //    Find(MapManager.instance.GetCurrentMapName()).gameObject.SetActive(false);

        stage.gameObject.SetActive(true);
        stage.PrevStageName = MapManager.instance.GetCurrentMapName();
        var checkpoint = stage.GetComponentInChildren<Checkpoint>();

        GameManager.instance.player.transform.position = checkpoint.transform.position;
        GameManager.instance.player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        var playerStat = GameManager.instance.player.GetComponent<Status>();
        playerStat.CurrHp = playerStat.FinalValue.maxHp;
        playerStat.CurrMp = playerStat.FinalValue.maxMp;
        GameManager.instance.player.GetComponent<PlayerInventory>()?.RefillPotions();
       
        Camera.main.transform.position = GameManager.instance.player.transform.position;
        MapManager.instance.SetCurrentMapName(checkpoint.transform.parent.name);
        MapManager.instance.SetcurrentChapterName(checkpoint.transform.parent.parent.name);

        EventManager.instance.Resume();
        UI.Instance.popupPanel.worldMapPopUp.ActiveFalse();
    }

    public void SetVillage(StageController stage)
    {
        if (stage.gameObject.name != "Village")
        {
            GameManager.instance.player.GetComponent<PlayerInventory>().RefillPotions();
        }
        GameManager.instance.player.transform.SetParent(null);

        MapManager.instance.GetCurrentStageObject().GetComponent<StageController>().PortalOpen();
        MapManager.instance.GetCurrentStageObject().SetActive(false);
        //GameObject.FindWithTag("Map").transform.Find(MapManager.instance.GetCurrentChapterName()).
        //    Find(MapManager.instance.GetCurrentMapName()).gameObject.SetActive(false);

        stage.gameObject.SetActive(true);
        stage.PrevStageName = MapManager.instance.GetCurrentMapName();
        var checkpoint = stage.GetComponentInChildren<Checkpoint>();

        GameManager.instance.player.transform.position = checkpoint.transform.position;
        GameManager.instance.player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        var playerStat = GameManager.instance.player.GetComponent<Status>();
        playerStat.CurrHp = playerStat.FinalValue.maxHp;
        playerStat.CurrMp = playerStat.FinalValue.maxMp;
        GameManager.instance.player.GetComponent<PlayerInventory>()?.RefillPotions();

        Camera.main.transform.position = GameManager.instance.player.transform.position;
        MapManager.instance.SetCurrentMapName(checkpoint.transform.parent.name);
        MapManager.instance.SetcurrentChapterName(checkpoint.transform.parent.parent.name);

        EventManager.instance.Resume();
        UI.Instance.popupPanel.worldMapPopUp.ActiveFalse();
    }
}
