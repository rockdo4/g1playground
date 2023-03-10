using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class MapManager : MonoBehaviour
{

    private List<GameObject> maps;
    private string currentMapName;
    private static MapManager m_instance;
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
                PlayerDataManager.instance.lastMapId = map.name;
            }
        }
        if (instance != this)
            Destroy(gameObject);
    }


    public void SetCurrentMapName(string name)
    {
        maps = GameObject.FindGameObjectsWithTag("Stage").ToList();
        currentMapName = name;

    }

    public void SetLastCheckpointMapTurnOn()
    {
        foreach (var map in maps)
        {
            if (map.name != PlayerDataManager.instance.lastMapId)
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

}
