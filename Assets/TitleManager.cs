using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    [SerializeField]
    private Button countiune;

    private void OnEnable()
    {
        string savepath = Application.persistentDataPath +"/Save/Save_Player.bin";
        if (!File.Exists(savepath))
        {
            countiune.interactable = false;
        }
    }

}
