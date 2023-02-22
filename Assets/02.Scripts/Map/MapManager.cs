using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
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
        var maps = GameObject.FindGameObjectsWithTag("Stage");

        foreach (var map in maps)
        {
            if (map.active) { 
                currentMapName= map.name;

            }
        }
        if (instance != this)
            Destroy(gameObject);
      //  DontDestroyOnLoad(gameObject);
    }
      

    //func for When the scene changes, find the door associated with the previous map in the new scene and set the player to that door position.
    public void SetCurrentMapName(string name)
    {
        currentMapName = name;
    }

    public string GetCurrentMapName()
    {
        return currentMapName;
    }

}
