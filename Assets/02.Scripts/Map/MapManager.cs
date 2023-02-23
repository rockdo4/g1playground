using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
            if (map.active)
            {
                SetCurrentMapName(map.name);
            }
        }
        if (instance != this)
            Destroy(gameObject);
        //  DontDestroyOnLoad(gameObject);
    }


    //func for When the scene changes, find the door associated with the previous map in the new scene and set the player to that door position.
    public void SetCurrentMapName(string name)
    {
        maps = GameObject.FindGameObjectsWithTag("Stage").ToList();
        currentMapName = name;
        foreach (var map in maps)
        {
            if (map.name != currentMapName)
                map.SetActive(false);
        }


        var tempCurrentDoors = GameObject.Find(currentMapName).GetComponentsInChildren<Connector>();
        Debug.Log(tempCurrentDoors.Length);
        foreach (var map in tempCurrentDoors)
        {
            map.NextStage.SetActive(true);
        }


    }

    public string GetCurrentMapName()
    {
        return currentMapName;
    }

}
