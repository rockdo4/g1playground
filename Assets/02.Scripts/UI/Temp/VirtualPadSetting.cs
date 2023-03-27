using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VirtualPadSetting : MonoBehaviour
{
    public Button leftBt;
    public Button rightBt;

    public GameObject popup;

    private void Start()
    {

    }


    private void Update()
    {
        if(popup.gameObject.activeSelf)
        {
            popup.gameObject.SetActive(false);
        }
    }
}
