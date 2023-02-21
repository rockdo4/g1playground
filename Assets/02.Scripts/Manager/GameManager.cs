using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }
    public Player player;
    
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        //var data = CSVReader.Read("UserCSV/PlayerHealth");
        //for (int i = 0; i < data.Count; i++)
        //{
        //    //Debug.Log(data[i].Values);
        //    foreach (var j in data[i].Values)
        //    {
        //        Debug.Log(j);
        //    }
        //}
    }
    public void Restart()
    {
        SceneManager.LoadScene("ChoiJ");
    }
}
