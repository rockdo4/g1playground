using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public Cinemachine.CinemachineVirtualCamera cinemachine;
    public PolygonCollider2D currentMapCollider;
    private List<GameObject> maps;
    private string currentMapName;
    private string currentChapterName;
    private static MapManager m_instance;
    public List<GameObject> outlines;
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
        maps = GameObject.FindGameObjectsWithTag("Stage").ToList();

        foreach (var map in maps)
        {
            if (map.activeSelf)
            {
                SetCurrentMapName(map.name);
                SetcurrentChapterName(map.transform.parent.name);
                if (PlayerDataManager.instance != null)
                {
                    PlayerDataManager.instance.lastSaveMapId = map.name;
                    PlayerDataManager.instance.lastSaveChapterName = map.transform.parent.name;
                }
               
            }
        }
        if (instance != this)
            Destroy(gameObject);
    }


    public void SetCurrentMapName(string name)
    {
        maps = GameObject.FindGameObjectsWithTag("Stage").ToList();
        currentMapName = name;

        var collider = GameObject.Find(currentMapName).GetComponentInChildren<PolygonCollider2D>();
        if (collider != null)
        {
            cinemachine.GetComponent<FollowCamera>().SetCollider(collider);
            currentMapCollider=collider;
        }

    }

    public void SetcurrentChapterName(string name)
    {
        currentChapterName = name;

    }

    public void SetLastCheckpointMapTurnOn()
    {
        if (maps == null)
        {
            maps = GameObject.FindGameObjectsWithTag("Stage").ToList();
        }

        foreach (var map in maps)
        {
            if (map.name != PlayerDataManager.instance.lastSaveMapId)
            {
                if (map.activeSelf)
                    map.SetActive(false);
            }
        }
    }


    public string GetCurrentMapName()
    {
        return currentMapName;
    }

    public string GetCurrentChapterName()
    {
        return currentChapterName;
    }

}