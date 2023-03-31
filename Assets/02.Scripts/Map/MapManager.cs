using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public Cinemachine.CinemachineVirtualCamera cinemachine;
    public PolygonCollider2D currentMapCollider;
    private List<StageController> maps = new();
    private string currentMapName;
    private string currentChapterName;
    private string prevChaperName;
    private static MapManager m_instance;
    public List<GameObject> outlines;
    private GameObject currentStageObject;
    private GameObject currentChapterObject;
    private GameObject map;

    public List<StageController> GetStageList() => maps;

    public GameObject GetCurrentStageObject() => currentStageObject;
    public GameObject GetCurrentChapterObject() => currentChapterObject;

    public static MapManager instance
    {
        get
        {
            if (m_instance == null)
                m_instance = FindObjectOfType<MapManager>();
            return m_instance;
        }
    }

    private void Awake()
    {                
        if (instance != this)
            Destroy(gameObject);

        map = GameObject.FindGameObjectWithTag("Map");
        var chapterCount = map.transform.childCount; 
        for (int i = 0; i < chapterCount; i++)
        {
            var chapter = map.transform.GetChild(i).gameObject;
            var temp = chapter.GetComponentsInChildren<StageController>(true);
            foreach (var t in temp)
            {
                maps.Add(t);
                if (t.name == "Village")
                    currentStageObject = t.gameObject;
            }
        }

        LoadProgress();
    }

    public void LoadProgress()
    {
        var saveData = SaveLoadSystem.Load(SaveData.Types.Stage) as SaveStageDataVer1;

        if (saveData == null)
        {
            return;
        }
           
        foreach (var stage in saveData.unlock)
        {
            foreach (var map in maps)
            {
                if (map.name == stage.Key)
                {
                    map.IsClear = stage.Value;
                    if (map.IsClear)
                        map.SetWorldMapButton();
                }
            }
        }
    }


    public void SaveProgress()
    {
        var originData = SaveLoadSystem.Load(SaveData.Types.Stage) as SaveStageDataVer1;
        if (originData == null)
        {
            var newsaveData = new SaveStageDataVer1();
            newsaveData.unlock = new Dictionary<string, bool>();
            foreach(var m in maps)
            {
                newsaveData.unlock.Add(m.name, false);
            }

            SaveLoadSystem.Save(newsaveData);

            originData = SaveLoadSystem.Load(SaveData.Types.Stage) as SaveStageDataVer1;
        }
        originData.unlock[currentMapName] = true;


        SaveLoadSystem.Save(originData);
    }

    public void SetCurrentMapName(string name)
    {
        if (currentStageObject != null)
        {
            currentStageObject.SetActive(false);
        }
        currentMapName = name;
        foreach (var map in maps)
        {
            if (map.name == name)
            {
                currentStageObject = map.gameObject;
                map.gameObject.SetActive(true);
            }
            else
                map.gameObject.SetActive(false);
        }
        currentStageObject = GameObject.Find(currentMapName);
        currentStageObject.SetActive(true);


        if (currentMapName != null)
        {

            var collider = GameObject.Find(currentMapName).GetComponentInChildren<PolygonCollider2D>();
            if (collider != null)
            {
                cinemachine.GetComponent<FollowCamera>().SetCollider(collider);
                currentMapCollider = collider;
            }
        }
    }

    public void SetcurrentChapterName(string name)
    {
        currentChapterName = name;        
        currentChapterObject = GameObject.Find(name);

        if (prevChaperName != currentChapterName)
        {
            prevChaperName = currentChapterName;
            var chapter = GameObject.FindGameObjectsWithTag("Chapter");
            foreach (var chapt in chapter)
            {
                if (chapt.name == currentChapterName)
                {
                    var chapterNumber = chapt.GetComponent<Chapter>().chapterNumber;
                    SoundManager.instance.ChangeBgm(chapterNumber);
                }
            }
            
            
        }
    }



    public void SetLastCheckpointMapTurnOn()
    {

        foreach (var map in maps)
        {
            if (map.name != PlayerDataManager.instance.lastSaveMapId)
            {
                if (map.gameObject.activeSelf)
                    map.gameObject.SetActive(false);
            }
        }
    }


    public string GetCurrentMapName()
    {
        if (currentChapterName == null)
            currentMapName = "Village";
        return currentMapName;
    }

    public string GetCurrentChapterName()
    {
        return currentChapterName;
    }

}