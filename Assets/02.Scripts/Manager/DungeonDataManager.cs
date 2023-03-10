using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonDataManager : MonoBehaviour
{
    private static PlayerDataManager m_instance;
    public static PlayerDataManager instance
    {
        get
        {
            if (m_instance == null)
                m_instance = FindObjectOfType<PlayerDataManager>();
            return m_instance;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
