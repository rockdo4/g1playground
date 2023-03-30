using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
        SetStage(stage.GetComponent<StageController>());
    }

    private void SetStage(StageController stage)
    {
        if (!GameObject.FindWithTag("Map").transform.Find(MapManager.instance.GetCurrentChapterName()).
            Find(MapManager.instance.GetCurrentMapName()).GetComponent<StageController>().IsClear)
        {
            return;
        }
        GameManager.instance.player.GetComponent<PlayerInventory>().RefillPotions();
        GameManager.instance.player.transform.SetParent(null);

        
        GameObject.FindWithTag("Map").transform.Find(MapManager.instance.GetCurrentChapterName()).
            Find(MapManager.instance.GetCurrentMapName()).gameObject.SetActive(false);


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
    }
}
