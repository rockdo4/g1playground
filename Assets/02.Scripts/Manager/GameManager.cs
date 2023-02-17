using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var data = CSVReader.Read("UserCSV/PlayerHealth");
        for (int i = 0; i < data.Count; i++)
        {
            //Debug.Log(data[i].Values);
            foreach (var j in data[i].Values)
            {
                Debug.Log(j);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}
