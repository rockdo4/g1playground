using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
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
