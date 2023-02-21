using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectorManager : MonoBehaviour
{
    private string previousMapName; 
    private static ConnectorManager m_instance;
    public static ConnectorManager instance
    {
        get
        {
            if (m_instance == null)
                m_instance = FindObjectOfType<ConnectorManager>();
            return m_instance;
        }
    }

    private void Awake()
    {
        if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    //func for When the scene changes, find the door associated with the previous map in the new scene and set the player to that door position.
    public void SetPreviousMapName(string name)
    {
        previousMapName = name;
    }

    public string GetPreviousMapName()
    {
        return previousMapName;
    }

}
