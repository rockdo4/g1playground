using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class WorldMap : MonoBehaviour
{
    [SerializeField] private GameObject[] chapters;
    private List<StageController> stageList = new List<StageController>();
    // Start is called before the first frame update
    void Start()
    {
        ////Add object with StageController to the list
        //foreach (var stages in chapters)
        //{
        //    int childCount = stages.transform.childCount;
        //    for (int i = 0; i < childCount; ++i)
        //    {
        //        var stage = stages.transform.GetChild(i).GetComponent<StageController>();

        //        stageList.Add(stage);

        //    }   
        //}
    }

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

    private void SetStage(StageController stage)
    {
        GameManager.instance.player.GetComponent<PlayerInventory>().RefillPotions();
        GameManager.instance.player.transform.SetParent(null);


        GameObject.FindWithTag("Map").transform.Find(MapManager.instance.GetCurrentChapterName()).
            Find(MapManager.instance.GetCurrentMapName()).gameObject.SetActive(false);

        //stage.gameObject.SetActive(true);

        ////change current state
        //GameManager.instance.player.transform.position = stage.gameObject.GetComponentInChildren<Checkpoint>().transform.position;
        //Camera.main.transform.position = GameManager.instance.player.transform.position;
        //MapManager.instance.SetCurrentMapName(PlayerDataManager.instance.lastSaveMapId);
        //MapManager.instance.SetcurrentChapterName(PlayerDataManager.instance.lastSaveChapterName);

        ////respawn state
        //GameObject.FindWithTag("Map").transform.Find(MapManager.instance.GetCurrentChapterName()).
        //    Find(MapManager.instance.GetCurrentMapName()).gameObject.GetComponent<StageController>().PortalClose();
        //PlayerDataManager.instance.FillPlayerHpMp();
        //MapManager.instance.SetLastCheckpointMapTurnOn();

        stage.gameObject.SetActive(true);
        stage.PrevStageName = MapManager.instance.GetCurrentMapName();
        var checkpoint = stage.GetComponentInChildren<Checkpoint>();


        GameManager.instance.player.transform.position = checkpoint.transform.position;
        GameManager.instance.player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        var playerStat = GameManager.instance.player.GetComponent<Status>();
        playerStat.CurrHp = playerStat.FinalValue.maxHp;
        playerStat.CurrMp = playerStat.FinalValue.maxMp;
        GameManager.instance.player.GetComponent<PlayerInventory>()?.RefillPotions();
        //portal.CanUse = true;
        //other.GetComponent<PlayerController>().AgentOnOff();

        
        Camera.main.transform.position = GameManager.instance.player.transform.position;
        MapManager.instance.SetCurrentMapName(checkpoint.transform.parent.name);
        MapManager.instance.SetcurrentChapterName(checkpoint.transform.parent.parent.name);

        if (checkpoint.transform.parent.name == "Village")
        {
            GameManager.instance.uiManager.PotionOff();
        }
        else
        {
            GameManager.instance.uiManager.PotionOn();
        }

        //transform.parent.gameObject.SetActive(false);

    }
}
